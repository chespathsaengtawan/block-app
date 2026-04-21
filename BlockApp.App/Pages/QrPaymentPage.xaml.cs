using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Payment;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;

namespace BlockApp.App.Pages;

public partial class QrPaymentPage : ContentPage
{
    private readonly PointsPaymentService _pointsPaymentService;
    private readonly PaymentResponseDto _payment;
    private System.Timers.Timer? _pollingTimer;
    private System.Timers.Timer? _countdownTimer;
    private DateTime _expiresAt;
    private bool _isChecking = false;

    public QrPaymentPage(PaymentResponseDto payment)
    {
        InitializeComponent();
        _pointsPaymentService = IPlatformApplication.Current!.Services.GetRequiredService<PointsPaymentService>();
        _payment = payment;
        _expiresAt = payment.ExpiresAt;

        InitializePayment();
    }

    private void InitializePayment()
    {
        // Display payment info
        QrCodeImage.Source = _payment.QrCodeUrl;
        AmountLabel.Text = $"฿{_payment.PointsAmount}";
        PointsLabel.Text = $"{_payment.PointsAmount:N0} แต้ม";

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
                await DisplayAlert("หมดเวลา", "QR Code หมดอายุแล้ว กรุณาลองใหม่อีกครั้ง", "ตกลง");
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
                StatusLabel.Text = "กำลังตรวจสอบการชำระเงิน...";
                StatusLoader.IsRunning = true;
            });

            var status = await _pointsPaymentService.CheckPaymentStatusAsync(_payment.PaymentId);

            if (status.IsCompleted && status.Status == "Success")
            {
                _pollingTimer?.Stop();
                _countdownTimer?.Stop();

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    StatusLabel.Text = "? ชำระเงินสำเร็จ!";
                    StatusLoader.IsRunning = false;
                    StatusBorder.BackgroundColor = Color.FromArgb("#F0FDF4");
                    StatusBorder.Stroke = Color.FromArgb("#BBF7D0");

                    await Task.Delay(1000);

                    await DisplayAlert(
                        "สำเร็จ! ??",
                        $"ได้รับ {status.PointsAdded:N0} แต้มเรียบร้อยแล้ว",
                        "เยี่ยม");

                    // Go back to previous page
                    await Navigation.PopAsync();
                });
            }
            else if (userTriggered)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    StatusLabel.Text = "ยังไม่พบการชำระเงิน กรุณารอสักครู่";
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
                    await DisplayAlert("ผิดพลาด", $"ไม่สามารถตรวจสอบสถานะได้: {ex.Message}", "ตกลง");
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
    }
}
