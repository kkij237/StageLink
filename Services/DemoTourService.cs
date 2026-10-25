namespace StageLink.Services;

public class DemoTourService : IDemoTourService
{
	public async Task RunAsync(CancellationToken cancellationToken = default)
	{
		// Durées en ms : 2 + 4 + 5 + 4 + 4 + 4 + 4 = 27 s (le script d'enregistrement ajoute le démarrage)
		await Task.Delay(2000, cancellationToken);                                  // Écran de connexion
		await Step("//MainTabs/offres", 4000, cancellationToken);                  // Liste des offres
		await Step("OfferDetailPage?offerId=1", 5000, cancellationToken);          // Détail d'une offre
		await Step("//MainTabs/recherche", 4000, cancellationToken);               // Recherche + filtres
		await Step("//MainTabs/candidatures", 4000, cancellationToken);            // Suivi des candidatures
		await Step("//MainTabs/profil", 4000, cancellationToken);                  // Profil étudiant
		await Step("AlertsPage", 4000, cancellationToken);                         // Alertes & notifications
		await Shell.Current.GoToAsync("//LoginPage");                              // Retour à l'accueil
	}

	private static async Task Step(string route, int delayMs, CancellationToken ct)
	{
		await Shell.Current.GoToAsync(route);
		await Task.Delay(delayMs, ct);
	}
}
