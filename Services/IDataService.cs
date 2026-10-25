using StageLink.Models;

namespace StageLink.Services;

public interface IDataService
{
	IReadOnlyList<JobOffer> GetOffers();
	JobOffer? GetOffer(int id);
	IReadOnlyList<JobApplication> GetApplications();
	bool HasApplied(int offerId);
	JobApplication Apply(JobOffer offer);
	IReadOnlyList<AlertRule> GetAlertRules();
	IReadOnlyList<NotificationItem> GetNotifications();
}
