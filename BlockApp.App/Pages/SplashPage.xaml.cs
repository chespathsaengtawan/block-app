using BlockApp.App.Services;

namespace BlockApp.App.Pages;

public partial class SplashPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly PinService _pinService;

    public SplashPage(ApiService apiService, PinService pinService)
    {
        InitializeComponent();
        _apiService = apiService;
        _pinService = pinService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(400); // brief visual pause
        await RouteAsync();
    }

    private async Task RouteAsync()
    {
        bool tokenValid = await _apiService.IsAccessTokenValidAsync();

        if (!tokenValid)
        {
            // Try refresh token
            tokenValid = await _apiService.RefreshTokenAsync();
        }

        if (tokenValid)
        {
            await _apiService.TryRestoreSessionAsync();
            bool hasPin = await _pinService.HasPinAsync();

            Page nextPage = hasPin
                ? new EnterPinPage(_pinService, _apiService)
                : (Page)new CreatePinPage(_pinService);

            Application.Current!.Windows[0].Page = new NavigationPage(nextPage);
        }
        else
        {
            Application.Current!.Windows[0].Page =
                new NavigationPage(new LoginPage(_apiService, _pinService));
        }
    }
}
