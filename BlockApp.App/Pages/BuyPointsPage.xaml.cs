using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Points;
using Microsoft.Extensions.DependencyInjection;

namespace BlockApp.App.Pages;

public partial class BuyPointsPage : ContentPage
{
    private readonly PointsPaymentService _pointsPaymentService;
    private bool _isFirstLoad = true;

    public BuyPointsPage()
    {
        InitializeComponent();
        _pointsPaymentService = IPlatformApplication.Current!.Services.GetRequiredService<PointsPaymentService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isFirstLoad)
        {
            _isFirstLoad = false;
            await LoadDataAsync();
        }
        else
        {
            // Refresh balance only when returning from QrPaymentPage
            await RefreshBalanceAsync();
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var balance = await _pointsPaymentService.GetBalanceAsync();
            BalanceLabel.Text = $"ยอดคงเหลือ : {balance:N0} พอยต์";

            var packages = await _pointsPaymentService.GetPackagesAsync();
            PackagesCollection.ItemsSource = packages;
        }
        catch (Exception ex)
        {
            await DisplayAlert("เกิดข้อผิดพลาด", $"เกิดข้อผิดพลาดในการโหลดข้อมูล: {ex.Message}", "ตกลง");
        }
    }

    private async Task RefreshBalanceAsync()
    {
        try
        {
            var balance = await _pointsPaymentService.GetBalanceAsync();
            BalanceLabel.Text = $"ยอดคงเหลือ : {balance:N0} พอยต์";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[BuyPointsPage] Error refreshing balance: {ex.Message}");
        }
    }

    private async void OnPackageTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not PointsPackageDto package)
            return;

        var confirm = await DisplayAlert(
            "ยืนยันการซื้อ",
            $"คุณต้องการซื้อ {package.Points:N0} พอยต์{(package.BonusPoints > 0 ? $" + โบนัส {package.BonusPoints} พอยต์" : "")} \nราคา {package.PriceTHB:N0} บาท",
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
            await DisplayAlert("เกิดข้อผิดพลาด", $"เกิดข้อผิดพลาดในการสร้างการชำระเงิน: {ex.Message}", "ตกลง");
        }
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}


