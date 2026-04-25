using BlockApp.App.Models;
using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Payment;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;

namespace BlockApp.App.Pages;

public partial class QrPaymentPage : ContentPage
{
    private readonly PointsPaymentService _pointsPaymentService;
    private readonly ApiService _apiService;
    private readonly HistoryService _historyService;
    private readonly PaymentResponseDto _payment;
    private System.Timers.Timer? _pollingTimer;
    private System.Timers.Timer? _countdownTimer;
    private DateTime _expiresAt;
    private bool _isChecking = false;
    private Stream? _qrStream;

    public QrPaymentPage(PaymentResponseDto payment)
    {
        InitializeComponent();
        _pointsPaymentService = IPlatformApplication.Current!.Services.GetRequiredService<PointsPaymentService>();
        _apiService = IPlatformApplication.Current!.Services.GetRequiredService<ApiService>();
        _historyService = IPlatformApplication.Current!.Services.GetRequiredService<HistoryService>();
        _payment = payment;
        _expiresAt = payment.ExpiresAt;

        InitializePayment();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadQrImageAsync();
    }

    private async Task LoadQrImageAsync()
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
            QrDebugLabel.Text = "กำลังโหลด QR...");
        try 
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
                QrDebugLabel.Text = $"Fetching /payment/{_payment.PaymentId}/qr...");

            var bytes = await _pointsPaymentService.GetQrImageBytesAsync(_payment.PaymentId);
            
            // Keep the stream alive as a field — MAUI reads it lazily
            _qrStream?.Dispose();
            _qrStream = new MemoryStream(bytes);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                QrDebugLabel.Text = $"Loaded {bytes.Length:N0} bytes";
                //QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                QrCodeImage.Source = ImageSource.FromStream(_ => Task.FromResult(_qrStream));
            });
        } 
        catch (Exception ex)
        { 
            await MainThread.InvokeOnMainThreadAsync(() =>
                QrDebugLabel.Text = $"Error: {ex.GetType().Name}: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"[QrPaymentPage] Failed to load QR image: {ex.Message}");
        } 
    } 

    private void InitializePayment()
    {
        AmountLabel.Text = $"฿{_payment.PointsAmount}";
        PointsLabel.Text = $"{_payment.PointsAmount:N0} พอยต์";

        // Start countdown timer
        StartCountdownTimer();

        // Start auto-checking (every 5 seconds)
        StartPolling();
    } 

    private void StartCountdownTimer()
    {
        _countdownTimer = new System.Timers.Timer(1000); // Every second
        _countdownTimer.Elapsed += OnCountdownTick;
        _countdownTimer.Start();
    }

    private void OnCountdownTick(object? sender, ElapsedEventArgs e)
    {
        var remaining = _expiresAt - DateTime.UtcNow;
        
        if (remaining.TotalSeconds <= 0)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                _countdownTimer?.Stop();
                _pollingTimer?.Stop();
                _historyService.Log(
                    HistoryAction.TopUpExpired,
                    note: $"{_payment.PointsAmount:N0} พอยต์ ({_payment.PointsAmount} บาท)");
                await DisplayAlert("หมดเวลา", "QR Code หมดอายุแล้ว", "ตกลง");
                await Navigation.PopAsync();
            });
            return;
        }

        var minutes = (int)remaining.TotalMinutes;
        var seconds = remaining.Seconds;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            TimerLabel.Text = $"เหลือเวลา {minutes:D2}:{seconds:D2} นาที";
        });
    }

    private void StartPolling()
    {
        _pollingTimer = new System.Timers.Timer(5000); // Every 5 seconds
        _pollingTimer.Elapsed += async (s, e) => await CheckPaymentAsync();
        _pollingTimer.Start();
    }

    private async void OnCheckPaymentTapped(object sender, EventArgs e)
    {
        await CheckPaymentAsync(userTriggered: true);
    }

    private async Task CheckPaymentAsync(bool userTriggered = false)
    {
        if (_isChecking)
            return;

        _isChecking = true;

        try
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StatusBorder.IsVisible = true;
                StatusLabel.Text = "กำลังตรวจสอบสถานะการชำระเงิน...";
                StatusLoader.IsRunning = true;
            });

            var status = await _pointsPaymentService.CheckPaymentStatusAsync(_payment.PaymentId);

            if (status.IsCompleted && status.Status == "Success")
            {
                _pollingTimer?.Stop();
                _countdownTimer?.Stop();

                _historyService.Log(
                    HistoryAction.TopUpSuccess,
                    note: $"{status.PointsAdded:N0} พอยต์");

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    StatusLabel.Text = "ชำระเงินสำเร็จ!";
                    StatusLoader.IsRunning = false;
                    StatusBorder.BackgroundColor = Color.FromArgb("#F0FDF4");
                    StatusBorder.Stroke = Color.FromArgb("#BBF7D0");

                    await Task.Delay(1000);

                    await DisplayAlert(
                        "สำเร็จ!",
                        $"คุณได้รับ {status.PointsAdded:N0} พอยต์แล้ว",
                        "ตกลง");

                    // Go back to previous page
                    await Navigation.PopAsync();
                });
            }
            else if (status.Status == "Failed")
            {
                _pollingTimer?.Stop();
                _countdownTimer?.Stop();

                _historyService.Log(
                    HistoryAction.TopUpFailed,
                    note: $"{_payment.PointsAmount:N0} พอยต์");

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    StatusLabel.Text = "การชำระเงินล้มเหลว";
                    StatusLoader.IsRunning = false;
                    StatusBorder.BackgroundColor = Color.FromArgb("#FEF2F2");
                    StatusBorder.Stroke = Color.FromArgb("#FECACA");

                    await DisplayAlert("ชำระเงินไม่สำเร็จ", "การชำระเงินถูกปฏิเสธหรือหมดเวลา กรุณาลองใหม่", "ตกลง");
                    await Navigation.PopAsync();
                });
            }
            else if (userTriggered)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    StatusLabel.Text = "ตรวจสอบสถานะการชำระเงินไม่สำเร็จ";
                    StatusLoader.IsRunning = false;
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    StatusBorder.IsVisible = false;
                });
            }
        }
        catch (Exception ex)
        {
            if (userTriggered)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("เกิดข้อผิดพลาด", $"เกิดข้อผิดพลาดในการตรวจสอบสถานะการชำระเงิน: {ex.Message}", "ตกลง");
                });
            }
        }
        finally
        {
            _isChecking = false;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _pollingTimer?.Stop();
        _countdownTimer?.Stop();
        _pollingTimer?.Dispose();
        _countdownTimer?.Dispose();
        _qrStream?.Dispose();
        _qrStream = null;
    }
}
