using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using StageLink.Services;
using StageLink.ViewModels;
using StageLink.Views;

namespace StageLink;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		// Supprime le soulignement natif des Entry (les champs sont habillés par un Border)
		EntryHandler.Mapper.AppendToMapping("StageLinkNoUnderline", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
		});

		// Services
		builder.Services.AddSingleton<IDialogService, DialogService>();
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IDataService, DataService>();
		builder.Services.AddSingleton<IDemoTourService, DemoTourService>();

		// ViewModels
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<OffersViewModel>();
		builder.Services.AddTransient<SearchViewModel>();
		builder.Services.AddTransient<ApplicationsViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<OfferDetailViewModel>();
		builder.Services.AddTransient<AlertsViewModel>();

		// Vues
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<OffersPage>();
		builder.Services.AddTransient<SearchPage>();
		builder.Services.AddTransient<ApplicationsPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<OfferDetailPage>();
		builder.Services.AddTransient<AlertsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
