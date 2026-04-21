using BlockApp.App.Pages;

namespace BlockApp.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // ─── Tab Navigation ────────────────────────────────────────

    private void OnTab1Tapped(object? sender, EventArgs e) => SelectTab(0);
    private void OnTab2Tapped(object? sender, EventArgs e) => SelectTab(1);
    private void OnTab3Tapped(object? sender, EventArgs e) => SelectTab(2);
    private void OnTab4Tapped(object? sender, EventArgs e) => SelectTab(3);

    private void SelectTab(int tab)
    {
        HomeViewEl.IsVisible    = tab == 0;
        SearchViewEl.IsVisible  = tab == 1;
        HistoryViewEl.IsVisible = tab == 2;
        ProfileViewEl.IsVisible = tab == 3;

        ApplyTabState(Tab1Pill, Tab1Icon, Tab1Lbl, tab == 0);
        ApplyTabState(Tab2Pill, Tab2Icon, Tab2Lbl, tab == 1);
        ApplyTabState(Tab3Pill, Tab3Icon, Tab3Lbl, tab == 2);
        ApplyTabState(Tab4Pill, Tab4Icon, Tab4Lbl, tab == 3);
    }

    private static void ApplyTabState(Border pill, Label icon, Label label, bool selected)
    {
        pill.BackgroundColor  = selected ? Color.FromArgb("#EDE9FF") : Colors.Transparent;
        icon.TextColor        = selected ? Color.FromArgb("#7C3AED") : Color.FromArgb("#9CA3AF");
        label.TextColor       = selected ? Color.FromArgb("#7C3AED") : Color.FromArgb("#9CA3AF");
        label.FontAttributes  = selected ? FontAttributes.Bold : FontAttributes.None;
    }

    // ─── FAB ────────────────────────────────────────────────

    private async void OnFabTapped(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddBlockPage(), animated: true);
    }
}
