using BlockApp.App.Pages;
using BlockApp.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockApp.App;

public partial class App : Application
{
	public App(IServiceProvider services)
	{
		InitializeComponent();
		_services = services;
	}

	private readonly IServiceProvider _services;

	protected override Window CreateWindow(IActivationState? activationState)
	{
		//#if DEBUG
		//    return new Window(new NavigationPage(new MainPage()));
		//#else
		var splashPage = _services.GetRequiredService<SplashPage>();
		return new Window(new NavigationPage(splashPage));
		//#endif

	}

	protected override void OnResume()
	{
		base.OnResume();
		// Silently sync blocklist with API in background
		_ = _services.GetRequiredService<ApiService>().SyncBlocklistAsync();
	}

}