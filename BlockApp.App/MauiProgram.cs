using Microsoft.Extensions.Logging;
using BlockApp.App.Services;
using BlockApp.App.Pages;

namespace BlockApp.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<BlocklistCacheService>();
		builder.Services.AddSingleton<ApiService>();
		builder.Services.AddSingleton<PinService>();
		builder.Services.AddSingleton<HistoryService>();
		builder.Services.AddSingleton<ContactsService>();
		builder.Services.AddSingleton<PointsPaymentService>();
		builder.Services.AddTransient<SplashPage>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<OtpPage>();
		builder.Services.AddTransient<CreatePinPage>();
		builder.Services.AddTransient<ConfirmPinPage>();
		builder.Services.AddTransient<EnterPinPage>();
		builder.Services.AddTransient<BuyPointsPage>();
		builder.Services.AddTransient<QrPaymentPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

	// Inside MauiProgram.cs -> CreateMauiApp
	Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
	{
	    #if ANDROID
	    // Removes the native Android underline
	    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
	    #elif WINDOWS
	    // Removes the border/underline on Windows
	    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
	    #elif IOS
	    // iOS Entry controls do not have an underline by default, but this removes the border
	    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
	    #endif
	});

		return builder.Build();
	}
}
