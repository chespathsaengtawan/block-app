using BlockApp.App.Pages;
using BlockApp.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockApp.App.Views;

public partial class ProfileView : ContentView
{
    private const string AvatarPreferenceKey = "user_avatar_path";
    private readonly PointsPaymentService? _pointsPaymentService;

    public ProfileView()
    {
        InitializeComponent();
        
        // Get service from DI
        var services = IPlatformApplication.Current?.Services;
        _pointsPaymentService = services?.GetService<PointsPaymentService>();
        
        PhoneLabel.Text = Preferences.Get("phone_number", "ไม่ระบุ");
        LoadVersionInfo();
        LoadSavedAvatar();
        LoadPointsBalance();
    }

    private void LoadVersionInfo()
    {
        var appVersion = AppInfo.Current.VersionString;
        AppLabelVersion.Text = $"แอป: {appVersion}";
        
        // For API version, you can get it from your API service or hardcode it
        ApiLabelVersion.Text = "API: 1.0.0";
    }

    private void LoadSavedAvatar()
    {
        var savedPath = Preferences.Get(AvatarPreferenceKey, string.Empty);
        if (!string.IsNullOrEmpty(savedPath) && File.Exists(savedPath))
        {
            AvatarImage.Source = ImageSource.FromFile(savedPath);
            AvatarImage.IsVisible = true;
            AvatarPlaceholder.IsVisible = false;
        }
    }

    private async void OnAvatarTapped(object? sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "เลือกรูปโปรไฟล์"
            });

            if (result == null) return;

            var stream = await result.OpenReadAsync();
            
            var localPath = Path.Combine(FileSystem.AppDataDirectory, "avatar.jpg");
            
            using (var fileStream = File.Create(localPath))
            {
                await stream.CopyToAsync(fileStream);
            }

            Preferences.Set(AvatarPreferenceKey, localPath);
            
            AvatarImage.Source = ImageSource.FromFile(localPath);
            AvatarImage.IsVisible = true;
            AvatarPlaceholder.IsVisible = false;
        }
        catch (Exception ex)
        {
            var page = Application.Current!.Windows[0].Page!;
            await page.DisplayAlert("ข้อผิดพลาด", $"ไม่สามารถอัพโหลดรูปได้: {ex.Message}", "ตลก");
        }
    }

    private async void OnLogoutTapped(object? sender, EventArgs e)
    {
        var page = Application.Current!.Windows[0].Page!;
        bool confirm = await page.DisplayAlert(
            "ออกจากระบบ", "คุณต้องการออกจากระบบใช่หรือไม่?", "ออกจากระบบ", "ยกเลิก");

        if (!confirm) return;

        Preferences.Remove("access_token");
        Preferences.Remove("refresh_token");
        Preferences.Remove("phone_number");

        var services = IPlatformApplication.Current!.Services;
        var splash = services.GetRequiredService<SplashPage>();
        Application.Current!.Windows[0].Page = new NavigationPage(splash);
    }

    private async void OnBuyPointsTapped(object? sender, EventArgs e)
    {
        try
        {
            var services = IPlatformApplication.Current!.Services;
            var buyPointsPage = services.GetRequiredService<BuyPointsPage>();

            EventHandler? handler = null;
            handler = (s, args) =>
            {
                buyPointsPage.Disappearing -= handler;
                RefreshPointsBalance();
            };
            buyPointsPage.Disappearing += handler;

            var navigationPage = Application.Current!.Windows[0].Page as NavigationPage;
            if (navigationPage != null)
            {
                await navigationPage.PushAsync(buyPointsPage);
            }
        }
        catch (Exception ex)
        {
            var page = Application.Current!.Windows[0].Page!;
            await page.DisplayAlert("ข้อผิดพลาด", $"ไม่สามารถเปิดหน้าซื้อพอยต์ได้: {ex.Message}", "ตกลง");
        }
    }

    private async void LoadPointsBalance()
    {
        try
        {
            if (_pointsPaymentService == null) return;

            var balance = await _pointsPaymentService.GetBalanceAsync();
            TotalPoint.Text = $"คงเหลือ {balance:N0} พอยต์";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading points balance: {ex.Message}");
            TotalPoint.Text = "คงเหลือ 0 พอยต์";
        }
    }

    public void RefreshPointsBalance()
    {
        LoadPointsBalance();
    }

}
