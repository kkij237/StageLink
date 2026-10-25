using CommunityToolkit.Mvvm.ComponentModel;

namespace StageLink.Models;

public partial class AlertRule : ObservableObject
{
	public string Title { get; init; } = string.Empty;
	public string Detail { get; init; } = string.Empty;

	[ObservableProperty]
	private bool isActive;
}
