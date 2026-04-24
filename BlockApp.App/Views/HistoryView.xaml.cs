using BlockApp.App.Services;

namespace BlockApp.App.Views;

public partial class HistoryView : ContentView
{
    private readonly HistoryService _historyService;

    public HistoryView()
    {
        InitializeComponent();
        _historyService = IPlatformApplication.Current!.Services.GetRequiredService<HistoryService>();
        Loaded += (_, _) => LoadHistory();
    }

    public void LoadHistory()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        EmptyState.IsVisible = false;
        HistoryCollection.IsVisible = false;

        var entries = _historyService.GetAll();

        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;

        if (entries.Count == 0)
        {
            EmptyState.IsVisible = true;
        }
        else
        {
            SubtitleLabel.Text = $"{entries.Count} รายการ";
            HistoryCollection.ItemsSource = entries;
            HistoryCollection.IsVisible = true;
        }
    }

    private async void OnClearHistoryTapped(object? sender, TappedEventArgs e)
    {
        var confirmed = await Application.Current!.Windows[0].Page!.DisplayAlert(
            "ล้างประวัติ", "ต้องการลบประวัติทั้งหมดใช่หรือไม่?", "ใช่", "ยกเลิก");
        if (!confirmed) return;

        _historyService.Clear();
        LoadHistory();
    }
}
