using StageLink.Models;

namespace StageLink.Services;

/// <summary>Source de données en mémoire (singleton) alimentée avec le jeu d'essai des maquettes.</summary>
public class DataService : IDataService
{
	private readonly List<JobOffer> _offers;
	private readonly List<JobApplication> _applications;
	private readonly List<AlertRule> _alertRules;
	private readonly List<NotificationItem> _notifications;
	private int _nextApplicationId = 100;

	public DataService()
	{
		_offers = new()
		{
			new JobOffer
			{
				Id = 1,
				Title = "Développeur .NET MAUI / C#",
				DetailTitle = "Stagiaire Développeur .NET MAUI & C#",
				Company = "InnoTech Solutions",
				Location = "Paris (Hybride)",
				Duration = "6 mois (PFE)",
				Salary = "1 350 € / mois",
				StartDate = "Février 2027",
				Category = "C# / .NET",
				IsFeatured = true,
				Tags = new[] { "C#", ".NET" },
				Skills = new[] { "C#", ".NET MAUI", "SQLite", "Docker" },
				Missions = new MissionItem[]
				{
					new("Développement mobile :", "Implémentation de parcours cross-platform sur iOS, Android et macOS via .NET MAUI et MVVM Toolkit."),
					new("UI éco-conçue :", "Conception d'interfaces réactives et légères préservant l'autonomie et les performances sur écrans OLED."),
					new("Intégration d'APIs :", "Connexion aux services REST, sérialisation des données et gestion sécurisée du cache local.")
				}
			},
			new JobOffer
			{
				Id = 2,
				Title = "Data Analyst / BI",
				Company = "DataCorp Analytics",
				Location = "Lyon",
				Duration = "6 mois",
				Salary = "1 200 € / mois",
				StartDate = "Mars 2027",
				Category = "Data & BI",
				Tags = new[] { "Python", "SQL", "Power BI" },
				Skills = new[] { "Python", "SQL", "Power BI" },
				Missions = new MissionItem[]
				{
					new("Tableaux de bord :", "Conception de rapports Power BI à destination des équipes métier."),
					new("Qualité des données :", "Nettoyage, modélisation et fiabilisation des jeux de données SQL."),
					new("Analyse :", "Production d'indicateurs et recommandations à partir des données collectées.")
				}
			},
			new JobOffer
			{
				Id = 3,
				Title = "Ingénieur DevOps Junior",
				Company = "CloudNative Lab",
				Location = "Télétravail",
				Duration = "6 mois",
				Salary = "1 400 € / mois",
				StartDate = "Février 2027",
				Category = "DevOps",
				Tags = new[] { "Docker", "CI/CD" },
				Skills = new[] { "Docker", "CI/CD", "Git", "Linux" },
				Missions = new MissionItem[]
				{
					new("Pipelines :", "Mise en place de chaînes d'intégration et de déploiement continus."),
					new("Conteneurs :", "Conteneurisation des services et optimisation des images Docker."),
					new("Supervision :", "Suivi de la disponibilité et des performances des environnements.")
				}
			}
		};

		_applications = new()
		{
			new JobApplication { Id = 1, Company = "InnoTech Solutions", Position = "Développeur .NET MAUI / C#", Status = ApplicationStatus.Interview, DateLabel = "18 Janv. à 14h30" },
			new JobApplication { Id = 2, Company = "DataCorp Analytics", Position = "Data Analyst Junior (Power BI / SQL)", Status = ApplicationStatus.InReview, DateLabel = "Il y a 3 jours" },
			new JobApplication { Id = 3, Company = "GreenTech Digital", Position = "Consultant Éco-Conception Web", Status = ApplicationStatus.Accepted, DateLabel = "12 Janv." },
			new JobApplication { Id = 4, Company = "Nexus Cloud Engineering", Position = "Ingénieur Cloud DevOps Junior", Status = ApplicationStatus.Sent, DateLabel = "Hier" }
		};

		_alertRules = new()
		{
			new AlertRule { Title = "Stages .NET MAUI / C#", Detail = "Douala • Quotidien", IsActive = true },
			new AlertRule { Title = "Stages Data & Power BI", Detail = "Yaoundé • En temps réel", IsActive = true }
		};

		_notifications = new()
		{
			new NotificationItem { Title = "Nouvelle offre chez InnoTech", Message = "Stage Développeur .NET MAUI & C#", TimeLabel = "Il y a 2h", IsUnread = true },
			new NotificationItem { Title = "Candidature consultée chez DataCorp", Message = "Le recruteur a consulté votre CV", TimeLabel = "Hier" },
			new NotificationItem { Title = "Rappel convention de stage", Message = "Échéance de signature avant le 15 mars", TimeLabel = "Il y a 3j" }
		};
	}

	public IReadOnlyList<JobOffer> GetOffers() => _offers;

	public JobOffer? GetOffer(int id) => _offers.FirstOrDefault(o => o.Id == id);

	public IReadOnlyList<JobApplication> GetApplications() => _applications;

	public bool HasApplied(int offerId) => _applications.Any(a => a.OfferId == offerId);

	public JobApplication Apply(JobOffer offer)
	{
		var existing = _applications.FirstOrDefault(a => a.OfferId == offer.Id);
		if (existing is not null) return existing;

		var application = new JobApplication
		{
			Id = _nextApplicationId++,
			OfferId = offer.Id,
			Company = offer.Company,
			Position = offer.Title,
			Status = ApplicationStatus.Sent,
			DateLabel = "À l'instant"
		};
		_applications.Insert(0, application);
		return application;
	}

	public IReadOnlyList<AlertRule> GetAlertRules() => _alertRules;

	public IReadOnlyList<NotificationItem> GetNotifications() => _notifications;
}
