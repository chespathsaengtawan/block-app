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
            BalanceLabel.Text = $"�������: {balance:N0} ���";

            // Load packages
            var packages = await _pointsPaymentService.GetPackagesAsync();
            PackagesCollection.ItemsSource = packages;
        }
        catch (Exception ex)
        {
            await DisplayAlert("�Դ��Ҵ", $"�������ö��Ŵ��������: {ex.Message}", "��ŧ");
        }
    }

    private async void OnPackageTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not PointsPackageDto package)
            return;

        var confirm = await DisplayAlert(
            "�׹�ѹ��ë���",
            $"������� {package.Points:N0} ���{(package.BonusPoints > 0 ? $" + ⺹�� {package.BonusPoints} ���" : "")} \n�Ҥ� {package.PriceTHB:N0} �ҷ",
            "�׹�ѹ",
            "¡��ԡ");

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
            await DisplayAlert("�Դ��Ҵ", $"�������ö���ҧ��ê����Թ��: {ex.Message}", "��ŧ");
        }
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}


