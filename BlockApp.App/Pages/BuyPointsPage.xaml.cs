using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Points;
using Microsoft.Extensions.DependencyInjection;

namespace BlockApp.App.Pages;

public partial class BuyPointsPage : ContentPage
{
    private readonly PointsPaymentService _pointsPaymentService;

    public BuyPointsPage()
    {
        InitializeComponent();
        _pointsPaymentService = IPlatformApplication.Current!.Services.GetRequiredService<PointsPaymentService>();
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        try
        {
            // Load balance
            var balance = await _pointsPaymentService.GetBalanceAsync();
            BalanceLabel.Text = $"คงเหลือ: {balance:N0} แต้ม";

            // Load packages
            var packages = await _pointsPaymentService.GetPackagesAsync();
            PackagesCollection.ItemsSource = packages;
        }
        catch (Exception ex)
        {
            await DisplayAlert("ผิดพลาด", $"ไม่สามารถโหลดข้อมูลได้: {ex.Message}", "ตกลง");
        }
    }

    private async void OnPackageTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not PointsPackageDto package)
            return;

        var confirm = await DisplayAlert(
            "ยืนยันการซื้อ",
            $"ซื้อแต้ม {package.Points:N0} แต้ม{(package.BonusPoints > 0 ? $" + โบนัส {package.BonusPoints} แต้ม" : "")} \nราคา {package.PriceTHB:N0} บาท",
            "ยืนยัน",
            "ยกเลิก");

        if (!confirm)
            return;

        try
        {
            // Create payment and get QR code
            var payment = await _pointsPaymentService.CreatePaymentAsync(package.PriceTHB);

            // Navigate to QR payment page
            await Navigation.PushAsync(new QrPaymentPage(payment));
        }
        catch (Exception ex)
        {
            await DisplayAlert("ผิดพลาด", $"ไม่สามารถสร้างการชำระเงินได้: {ex.Message}", "ตกลง");
        }
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Converter for bonus points visibility
public class IsNotZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is int intValue)
            return intValue > 0;
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
