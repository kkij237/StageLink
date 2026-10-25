using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class AlertsViewModel : BaseViewModel
{
	public IReadOnlyList<AlertRule> Rules { get; }
	public IReadOnlyList<NotificationItem> Notifications { get; }

	[ObservableProperty] private bool isQuietMode = true;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(MarkAllText))]
	private bool allRead;

	public string MarkAllText => AllRead ? "Toutes lues ✓" : "Tout marquer comme lu";

	public string ActiveCountLabel
	{
		get
		{
			var n = Rules.Count(r => r.IsActive);
			return n <= 1 ? $"{n} active" : $"{n} actives";
		}
	}

	public AlertsViewModel(IDataService data)
	{
		Rules = data.GetAlertRules();
		Notifications = data.GetNotifications();

		foreach (var rule in Rules)
		{
			rule.PropertyChanged += (_, _) => OnPropertyChanged(nameof(ActiveCountLabel));
		}

		AllRead = Notifications.All(n => !n.IsUnread);
	}

	[RelayCommand]
	private void MarkAllRead()
	{
		foreach (var n in Notifications) n.IsUnread = false;
		AllRead = true;
	}
}
