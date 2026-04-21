namespace BlockApp.App.Pages;

public partial class AddBlockPage : ContentPage
{
    public AddBlockPage()
    {
        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync(animated: true);
    }

    private async void OnSubmitTapped(object? sender, EventArgs e)
    {
        var phone = PhoneEntry.Text?.Trim();

        if (string.IsNullOrEmpty(phone))
        {
            await DisplayAlert("แจ้งเตือน", "กรุณากรอกเบอร์โทรศัพท์", "ตกลง");
            return;
        }

        SubmitLbl.IsVisible = false;
        SubmitLoader.IsRunning = true;
        SubmitLoader.IsVisible = true;

        // TODO: Call API to block phone number
        await Task.Delay(600);

        SubmitLoader.IsRunning = false;
        SubmitLoader.IsVisible = false;
        SubmitLbl.IsVisible = true;

        await DisplayAlert("สำเร็จ", $"บล็อกเบอร์ {phone} เรียบร้อยแล้ว", "ตกลง");
        await Navigation.PopModalAsync(animated: true);
    }
}
