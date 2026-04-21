using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Auth;
using BlockApp.Shared.Enums;

namespace BlockApp.App.Pages;

public partial class OtpPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly PinService _pinService;
    private readonly string _phoneNumber;
    private SmsProvider _fromService;
    private string? _providerToken;
    private int _resendCooldown = 0;
    private CancellationTokenSource? _resendCts;

    public OtpPage(ApiService apiService, PinService pinService, string phoneNumber,
        RequestOtpResultDto? otpResult = null)
    {
        InitializeComponent();
        _apiService = apiService;
        _pinService = pinService;
        _phoneNumber = phoneNumber;
        _fromService = otpResult?.FromService ?? SmsProvider.ThaibulkSMS;
        _providerToken = otpResult?.ProviderToken;
        SubtitleLabel.Text = $"กรุณากรอกรหัส OTP ที่ส่งไปยัง\nหมายเลข {phoneNumber}";
        StartResendCooldown();
    }

    // ─── Auto-advance OTP boxes ──────────────────────────────────────────────

    private void OnOtp1Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box1Border, e.NewTextValue);
        if (e.NewTextValue?.Length == 1) Otp2.Focus();
    }

    private void OnOtp2Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box2Border, e.NewTextValue);
        if (e.NewTextValue?.Length == 1) Otp3.Focus();
        else if (string.IsNullOrEmpty(e.NewTextValue)) Otp1.Focus();
    }

    private void OnOtp3Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box3Border, e.NewTextValue);
        if (e.NewTextValue?.Length == 1) Otp4.Focus();
        else if (string.IsNullOrEmpty(e.NewTextValue)) Otp2.Focus();
    }

    private void OnOtp4Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box4Border, e.NewTextValue);
        if (e.NewTextValue?.Length == 1) Otp5.Focus();
        else if (string.IsNullOrEmpty(e.NewTextValue)) Otp3.Focus();
    }

    private void OnOtp5Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box5Border, e.NewTextValue);
        if (e.NewTextValue?.Length == 1) Otp6.Focus();
        else if (string.IsNullOrEmpty(e.NewTextValue)) Otp4.Focus();
    }

    private void OnOtp6Changed(object? sender, TextChangedEventArgs e)
    {
        HighlightBox(Box6Border, e.NewTextValue);
        if (string.IsNullOrEmpty(e.NewTextValue)) Otp5.Focus();
    }

    private static void HighlightBox(Border box, string? text)
    {
        box.Stroke = string.IsNullOrEmpty(text)
            ? Color.FromArgb("#E5E7EB")
            : Color.FromArgb("#7C3AED");
    }

    // ─── Continue / Verify ──────────────────────────────────────────────────

    private async void OnContinueTapped(object? sender, EventArgs e)
    {
        var code = $"{Otp1.Text}{Otp2.Text}{Otp3.Text}{Otp4.Text}{Otp5.Text}{Otp6.Text}";

        if (code.Length < 6)
        {
            await DisplayAlert("แจ้งเตือน", "กรุณากรอกรหัส OTP ให้ครบ 6 หลัก", "ตกลง");
            return;
        }

        SetLoading(true);

        var result = await _apiService.VerifyOtpAsync(_phoneNumber, code, _fromService, _providerToken);

        SetLoading(false);

        if (result?.AccessToken != null)
        {
            bool hasPin = await _pinService.HasPinAsync();
            Page nextPage = hasPin
                ? new EnterPinPage(_pinService, _apiService)
                : (Page)new CreatePinPage(_pinService);

            // Replace navigation stack
            Application.Current!.Windows[0].Page = new NavigationPage(nextPage);
        }
        else
        {
            await DisplayAlert("รหัส OTP ไม่ถูกต้อง", "รหัส OTP ที่กรอกไม่ถูกต้องหรือหมดอายุ กรุณาลองใหม่", "ตกลง");
            ClearOtpBoxes();
        }
    }

    // ─── Resend OTP ─────────────────────────────────────────────────────────

    private async void OnResendTapped(object? sender, EventArgs e)
    {
        if (_resendCooldown > 0) return;

        ResendLabel.IsEnabled = false;

        var result = await _apiService.RequestOtpAsync(_phoneNumber);

        if (result != null)
        {
            _fromService = result.FromService;
            _providerToken = result.ProviderToken;
            ClearOtpBoxes();
            StartResendCooldown();
            await DisplayAlert("ส่งรหัสแล้ว", "ส่งรหัส OTP ใหม่ไปยัง " + _phoneNumber + " แล้ว", "ตกลง");
        }
        else
        {
            ResendLabel.IsEnabled = true;
            await DisplayAlert("เกิดข้อผิดพลาด", "ไม่สามารถส่ง OTP ได้ กรุณาลองใหม่อีกครั้ง", "ตกลง");
        }
    }

    private void StartResendCooldown(int seconds = 300)
    {
        _resendCts?.Cancel();
        _resendCts = new CancellationTokenSource();
        var token = _resendCts.Token;

        _resendCooldown = seconds;
        ResendLabel.IsEnabled = false;

        Task.Run(async () =>
        {
            while (_resendCooldown > 0 && !token.IsCancellationRequested)
            {
                var remaining = _resendCooldown;
                var display = $"{remaining / 60:D2}:{remaining % 60:D2}";
                MainThread.BeginInvokeOnMainThread(() =>
                    ResendLabel.Text = $"ส่งรหัสใหม่ ({display})");
                await Task.Delay(1000, token).ContinueWith(_ => { });
                _resendCooldown--;
            }

            if (!token.IsCancellationRequested)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ResendLabel.Text = "ส่งรหัสใหม่";
                    ResendLabel.IsEnabled = true;
                });
            }
        }, token);
    }

    // ─── Helpers ────────────────────────────────────────────────────────────

    private void ClearOtpBoxes()
    {
        Otp1.Text = "";
        Otp2.Text = "";
        Otp3.Text = "";
        Otp4.Text = "";
        Otp5.Text = "";
        Otp6.Text = "";
        Otp1.Focus();
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsRunning = isLoading;
        LoadingIndicator.IsVisible = isLoading;
        ContinueLbl.IsVisible = !isLoading;
        ContinueBtn.IsEnabled = !isLoading;
        Otp1.IsEnabled = !isLoading;
        Otp2.IsEnabled = !isLoading;
        Otp3.IsEnabled = !isLoading;
        Otp4.IsEnabled = !isLoading;
        Otp5.IsEnabled = !isLoading;
        Otp6.IsEnabled = !isLoading;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _resendCts?.Cancel();
    }
}
