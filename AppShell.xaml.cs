using StageLink.Views;

namespace StageLink;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Pages "poussées" (hors barre d'onglets), appelées par chaîne de caractères brute
		Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
		Routing.RegisterRoute("OfferDetailPage", typeof(OfferDetailPage));
		Routing.RegisterRoute("AlertsPage", typeof(AlertsPage));
	}
}
