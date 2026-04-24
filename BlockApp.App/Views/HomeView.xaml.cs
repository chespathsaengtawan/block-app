using BlockApp.App.Models;
using BlockApp.App.Services;
using BlockApp.Shared.DTOs.Blocklist;

namespace BlockApp.App.Views;

public partial class HomeView : ContentView
{
    private readonly ApiService _apiService;
    private readonly HistoryService _historyService;

    public HomeView()
    {
        InitializeComponent();
        _apiService      = IPlatformApplication.Current!.Services.GetRequiredService<ApiService>();
        _historyService  = IPlatformApplication.Current!.Services.GetRequiredService<HistoryService>();
        Loaded += async (_, _) => await LoadAsync();
    }

    public async Task LoadAsync()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        EmptyState.IsVisible = false;
        BlocklistCollection.IsVisible = false;

        var list = await _apiService.GetBlocklistAsync();

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;

        if (list.Count == 0)
        {
            EmptyState.IsVisible = true;
        }
        else
        {
            SubtitleLabel.Text = $"{list.Count} รายการที่บล็อก";
            BlocklistCollection.ItemsSource = list;
            BlocklistCollection.IsVisible = true;
        }
    }

    private async void OnDeleteTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is not BlockEntryDto item) return;

        var target = item.PhoneNumber ?? item.AccountNumber ?? "รายการนี้";

        // Ask reason for deletion
        var reason = await Application.Current!.Windows[0].Page!.DisplayPromptAsync(
            "ยกเลิกการบล็อก",
            $"ระบุเหตุผลที่ต้องการยกเลิกบล็อก\n{target}",
            placeholder: "เช่น เบอร์ผิด, แก้ไขข้อมูล...",
            accept: "ยืนยัน",
            cancel: "ยกเลิก");

        // null = pressed cancel
        if (reason == null) return;

        var confirmed = await Application.Current!.Windows[0].Page!.DisplayAlert(
            "ยืนยัน",
            $"ต้องการยกเลิกบล็อก {target} ใช่หรือไม่?",
            "ใช่", "ไม่ใช่");

        if (!confirmed) return;

        var ok = await _apiService.DeleteBlockEntryAsync(item.UserBlockEntryId);
        if (ok)
        {
            _historyService.Log(
                HistoryAction.BlockDeleted,
                target: target,
                bankName: item.BankName,
                note: string.IsNullOrWhiteSpace(reason) ? null : reason);

            await LoadAsync();
        }
        else
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert(
                "เกิดข้อผิดพลาด", "ไม่สามารถลบรายการได้ กรุณาลองใหม่", "ตกลง");
        }
    }
}
