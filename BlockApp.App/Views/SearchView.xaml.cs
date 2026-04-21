namespace BlockApp.App.Views;

public partial class SearchView : ContentView
{
    public SearchView()
    {
        InitializeComponent();
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        // TODO: debounce + call search API
    }
}
