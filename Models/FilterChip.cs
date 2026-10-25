using CommunityToolkit.Mvvm.ComponentModel;

namespace StageLink.Models;

public partial class FilterChip : ObservableObject
{
	public string Key { get; init; } = string.Empty;
	public string Label { get; init; } = string.Empty;

	[ObservableProperty]
	private bool isSelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasCount))]
	private int count = -1;

	public bool HasCount => Count >= 0;
}
