using BlockApp.App.Services;

namespace BlockApp.App.Pages;

public partial class ConfirmPinPage : ContentPage
{
    private readonly PinService _pinService;
    private readonly string _originalPin;
    private string _pin = "";

    private readonly Border[] _dots;

    public ConfirmPinPage(PinService pinService, string originalPin)
    {
        InitializeComponent();
        _pinService = pinService;
        _originalPin = originalPin;
        _dots = [Dot1, Dot2, Dot3, Dot4, Dot5, Dot6];
    }

    private void OnDigitTapped(object? sender, TappedEventArgs e)
    {
        if (_pin.Length >= 6) return;

        _pin += e.Parameter?.ToString() ?? "";
        StatusLabel.IsVisible = false;
        UpdateDots();

        if (_pin.Length == 6)
            VerifyAndSave();
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

    private async void VerifyAndSave()
    {
        if (_pin == _originalPin)
        {
            await _pinService.SavePinAsync(_pin);
            Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
        }
        else
        {
            // Shake animation + show error
            await ShakeDotsAsync();
            StatusLabel.Text = "PIN ไม่ตรงกัน กรุณาลองใหม่";
            StatusLabel.IsVisible = true;
            _pin = "";
            UpdateDots();
        }
    }

    private async Task ShakeDotsAsync()
    {
        // Brief left-right shake on the dot row
        uint d = 60;
        await Task.WhenAll(
            PinDotsRow.TranslateTo(-10, 0, d),
            Task.Delay((int)d));
        await Task.WhenAll(
            PinDotsRow.TranslateTo(10, 0, d),
            Task.Delay((int)d));
        await Task.WhenAll(
            PinDotsRow.TranslateTo(-6, 0, d),
            Task.Delay((int)d));
        await Task.WhenAll(
            PinDotsRow.TranslateTo(6, 0, d),
            Task.Delay((int)d));
        await PinDotsRow.TranslateTo(0, 0, d);
    }
}
