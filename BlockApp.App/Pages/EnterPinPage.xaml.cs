using BlockApp.App.Services;

namespace BlockApp.App.Pages;

public partial class EnterPinPage : ContentPage
{
    private readonly PinService _pinService;
    private readonly ApiService _apiService;
    private string _pin = "";
    private int _attempts = 0;
    private const int MaxAttempts = 5;

    private readonly Border[] _dots;

    public EnterPinPage(PinService pinService, ApiService apiService)
    {
        InitializeComponent();
        _pinService = pinService;
        _apiService = apiService;
        _dots = [Dot1, Dot2, Dot3, Dot4, Dot5, Dot6];
    }

    private void OnDigitTapped(object? sender, TappedEventArgs e)
    {
        if (_pin.Length >= 6) return;

        _pin += e.Parameter?.ToString() ?? "";
        StatusLabel.IsVisible = false;
        UpdateDots();

        if (_pin.Length == 6)
            VerifyPin();
    }

    private void OnBackspaceTapped(object? sender, TappedEventArgs e)
    {
        if (_pin.Length == 0) return;
        _pin = _pin[..^1];
        StatusLabel.IsVisible = false;
        UpdateDots();
    }

    private void UpdateDots()
    {
        for (int i = 0; i < _dots.Length; i++)
            _dots[i].BackgroundColor = i < _pin.Length
                ? Color.FromArgb("#7C3AED")
                : Color.FromArgb("#E5E7EB");
    }

    private async void VerifyPin()
    {
        var correct = await _pinService.VerifyPinAsync(_pin);
        if (correct)
        {
            await _apiService.TryRestoreSessionAsync();
            Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
        }
        else
        {
            _attempts++;
            await ShakeDotsAsync();

            if (_attempts >= MaxAttempts)
            {
                await _apiService.ClearSessionAsync();
                await _pinService.ClearPinAsync();
                await DisplayAlert("ล็อกบัญชี", "กรอก PIN ผิดเกิน 5 ครั้ง กรุณาเข้าสู่ระบบใหม่", "ตกลง");
                Application.Current!.Windows[0].Page =
                    new NavigationPage(new LoginPage(_apiService, _pinService));
                return;
            }

            StatusLabel.Text = $"PIN ไม่ถูกต้อง เหลืออีก {MaxAttempts - _attempts} ครั้ง";
            StatusLabel.IsVisible = true;
            _pin = "";
            UpdateDots();
        }
    }

    private async void OnLogoutTapped(object? sender, TappedEventArgs e)
    {
        var confirm = await DisplayAlert("ออกจากระบบ", "คุณต้องการออกจากระบบใช่ไหม?", "ใช่", "ยกเลิก");
        if (!confirm) return;

        await _apiService.ClearSessionAsync();
        await _pinService.ClearPinAsync();
        Application.Current!.Windows[0].Page =
            new NavigationPage(new LoginPage(_apiService, _pinService));
    }

    private async Task ShakeDotsAsync()
    {
        uint d = 60;
        await Task.WhenAll(PinDotsRow.TranslateTo(-10, 0, d), Task.Delay((int)d));
        await Task.WhenAll(PinDotsRow.TranslateTo(10, 0, d), Task.Delay((int)d));
        await Task.WhenAll(PinDotsRow.TranslateTo(-6, 0, d), Task.Delay((int)d));
        await Task.WhenAll(PinDotsRow.TranslateTo(6, 0, d), Task.Delay((int)d));
        await PinDotsRow.TranslateTo(0, 0, d);
    }
}
