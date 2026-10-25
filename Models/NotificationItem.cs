using CommunityToolkit.Mvvm.ComponentModel;

namespace StageLink.Models;

public partial class NotificationItem : ObservableObject
{
	public string Title { get; init; } = string.Empty;
	public string Message { get; init; } = string.Empty;
	public string TimeLabel { get; init; } = string.Empty;

	[ObservableProperty]
	private bool isUnread;
}
