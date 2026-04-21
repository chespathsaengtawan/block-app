using Microsoft.UI.Xaml;

namespace BlockApp.App.WinUI;

public partial class App : MauiWinUIApplication
{
	public App()
	{
		AppDomain.CurrentDomain.UnhandledException += (_, e) =>
		{
			var ex = e.ExceptionObject as Exception;
			File.WriteAllText(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "blockapp-crash.txt"),
				$"{DateTime.Now}\n{ex?.GetType()}\n{ex?.Message}\n{ex?.StackTrace}");
		};
		try
		{
			this.InitializeComponent();
		}
		catch (Exception ex)
		{
			File.WriteAllText(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "blockapp-crash.txt"),
				$"InitializeComponent failed\n{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}");
			throw;
		}
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

