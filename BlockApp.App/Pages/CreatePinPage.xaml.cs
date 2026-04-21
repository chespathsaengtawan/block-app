using BlockApp.App.Services;

namespace BlockApp.App.Pages;

public partial class CreatePinPage : ContentPage
{
    private readonly PinService _pinService;
    private string _pin = "";

    private readonly Border[] _dots;

    public CreatePinPage(PinService pinService)
    {
        InitializeComponent();
        _pinService = pinService;
        _dots = [Dot1, Dot2, Dot3, Dot4, Dot5, Dot6];
    }

    private void OnDigitTapped(object? sender, TappedEventArgs e)
    {
        if (_pin.Length >= 6) return;

        _pin += e.Parameter?.ToString() ?? "";
        UpdateDots();

        if (_pin.Length == 6)
            ProceedToConfirm();
    }

    private void OnBackspaceTapped(object? sender, TappedEventArgs e)
    {
        if (_pin.Length == 0) return;
        _pin = _pin[..^1];
        UpdateDots();
    }

    private void UpdateDots()
    {
        for (int i = 0; i < _dots.Length; i++)
            _dots[i].BackgroundColor = i < _pin.Length
                ? Color.FromArgb("#7C3AED")
                : Color.FromArgb("#E5E7EB");
    }

    private async void ProceedToConfirm()
    {
        await Navigation.PushAsync(new ConfirmPinPage(_pinService, _pin));
        // Reset for back-navigation
        _pin = "";
        UpdateDots();
    }
}
