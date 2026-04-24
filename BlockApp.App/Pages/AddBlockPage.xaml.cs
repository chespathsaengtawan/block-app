using BlockApp.App.Models;
using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Blocklist;
using BlockApp.Shared.Enums;

namespace BlockApp.App.Pages;

public partial class AddBlockPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly HistoryService _historyService;
    private BlockEntryType _selectedType = BlockEntryType.Phone;

    public AddBlockPage()
    {
        InitializeComponent();
        _apiService     = IPlatformApplication.Current!.Services.GetRequiredService<ApiService>();
        _historyService = IPlatformApplication.Current!.Services.GetRequiredService<HistoryService>();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync(animated: true);
    }

    private void OnTypePhoneTapped(object? sender, EventArgs e)
    {
        _selectedType = BlockEntryType.Phone;
        PhoneSection.IsVisible = true;
        BankSection.IsVisible = false;

        PhoneTypeBtn.BackgroundColor = Color.FromArgb("#7C3AED");
        PhoneTypeBtn.StrokeThickness = 0;
        PhoneTypeLbl.TextColor = Colors.White;

        BankTypeBtn.BackgroundColor = Color.FromArgb("#F3F4F6");
        BankTypeBtn.StrokeThickness = 1;
        BankTypeLbl.TextColor = Color.FromArgb("#6B7280");
    }

    private void OnTypeBankTapped(object? sender, EventArgs e)
    {
        _selectedType = BlockEntryType.BankAccount;
        PhoneSection.IsVisible = false;
        BankSection.IsVisible = true;

        BankTypeBtn.BackgroundColor = Color.FromArgb("#7C3AED");
        BankTypeBtn.StrokeThickness = 0;
        BankTypeLbl.TextColor = Colors.White;

        PhoneTypeBtn.BackgroundColor = Color.FromArgb("#F3F4F6");
        PhoneTypeBtn.StrokeThickness = 1;
        PhoneTypeLbl.TextColor = Color.FromArgb("#6B7280");
    }

    private void OnOtherReasonCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        OtherReasonInputContainer.IsVisible = e.Value;
    }

    private async void OnSubmitTapped(object? sender, EventArgs e)
    {
        // Validate
        if (_selectedType == BlockEntryType.Phone)
        {
            if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
            {
                await DisplayAlert("แจ้งเตือน", "กรุณากรอกเบอร์โทรศัพท์", "ตกลง");
                return;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(BankNameEntry.Text) || string.IsNullOrWhiteSpace(AccountNumberEntry.Text))
            {
                await DisplayAlert("แจ้งเตือน", "กรุณากรอกชื่อธนาคารและเลขที่บัญชี", "ตกลง");
                return;
            }
        }

        // Collect reasons
        var reasons = BlockReason.None;
        if (ReasonSpamCheck.IsChecked)  reasons |= BlockReason.SpamCall;
        if (ReasonScamCheck.IsChecked)  reasons |= BlockReason.Scam;
        if (ReasonMuleCheck.IsChecked)  reasons |= BlockReason.MuleAccount;
        if (ReasonOtherCheck.IsChecked) reasons |= BlockReason.Other;

        var dto = new CreateBlockEntryDto
        {
            EntryType         = _selectedType,
            PhoneNumber       = _selectedType == BlockEntryType.Phone ? PhoneEntry.Text?.Trim() : null,
            BankName          = _selectedType == BlockEntryType.BankAccount ? BankNameEntry.Text?.Trim() : null,
            AccountNumber     = _selectedType == BlockEntryType.BankAccount ? AccountNumberEntry.Text?.Trim() : null,
            AccountHolderName = _selectedType == BlockEntryType.BankAccount ? AccountHolderEntry.Text?.Trim() : null,
            Note              = NoteEditor.Text?.Trim(),
            Reasons           = reasons,
            OtherReason       = ReasonOtherCheck.IsChecked ? OtherReasonEntry.Text?.Trim() : null
        };

        SubmitLbl.IsVisible = false;
        SubmitLoader.IsRunning = true;
        SubmitLoader.IsVisible = true;

        var result = await _apiService.AddBlockEntryAsync(dto);

        SubmitLoader.IsRunning = false;
        SubmitLoader.IsVisible = false;
        SubmitLbl.IsVisible = true;

        if (result != null)
        {
            var target = _selectedType == BlockEntryType.Phone
                ? dto.PhoneNumber
                : dto.AccountNumber;

            _historyService.Log(
                result.AlreadyExisted ? HistoryAction.BlockAlreadyExisted : HistoryAction.BlockAdded,
                target: target,
                bankName: dto.BankName,
                note: dto.Note);

            var msg = result.AlreadyExisted
                ? $"มีผู้ใช้งาน {result.BlockedByCount} คนบล็อกหมายเลข/บัญชีนี้แล้ว ระบบเพิ่มให้คุณอัตโนมัติ"
                : $"บล็อกสำเร็จ มีผู้ใช้บล็อกรายการนี้ทั้งหมด {result.BlockedByCount} คน";
            await DisplayAlert("สำเร็จ", msg, "ตกลง");
            await Navigation.PopModalAsync(animated: true);
        }
        else
        {
            await DisplayAlert("เกิดข้อผิดพลาด", "ไม่สามารถบล็อกได้ กรุณาลองใหม่", "ตกลง");
        }
    }
}
