using BlockApp.App.Services;

namespace BlockApp.App.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly PinService _pinService;

    private readonly List<(string Display, string Code)> _countryCodes =
    [
        ("+66  TH", "+66")
    ];

    public LoginPage(ApiService apiService, PinService pinService)
    {
        InitializeComponent();
        _apiService = apiService;
        _pinService = pinService;
        SetupCountryCodePicker();
    }

    private void SetupCountryCodePicker()
    {
        CountryCodePicker.ItemsSource = _countryCodes.Select(c => c.Display).ToList();
        CountryCodePicker.SelectedIndex = 0; // Default: Thailand +66
    }

    private void OnCountryCodeChanged(object? sender, EventArgs e) { }

    private async void OnContinueTapped(object? sender, EventArgs e)
    {
        var phone = PhoneEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlert("แจ้งเตือน", "กรุณากรอกหมายเลขโทรศัพท์", "ตกลง");
            return;
        }

        var selectedCode = _countryCodes[CountryCodePicker.SelectedIndex].Code;
        var fullPhoneNumber = selectedCode + phone.TrimStart('0');

        SetLoading(true);

        var (otpResult, error) = await _apiService.RequestOtpAsync(fullPhoneNumber);

        SetLoading(false);

        if (otpResult != null)
        {
            await Navigation.PushAsync(new OtpPage(_apiService, _pinService, fullPhoneNumber, otpResult));
        }
        else
        {
            await DisplayAlert("เกิดข้อผิดพลาด", error ?? "ไม่สามารถส่ง OTP ได้ กรุณาลองใหม่อีกครั้ง", "ตกลง");
        }
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsRunning = isLoading;
        LoadingIndicator.IsVisible = isLoading;
        ContinueLbl.IsVisible = !isLoading;
        ContinueBtn.IsEnabled = !isLoading;
        PhoneEntry.IsEnabled = !isLoading;
        CountryCodePicker.IsEnabled = !isLoading;
    }
}
