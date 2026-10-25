using CommunityToolkit.Mvvm.ComponentModel;

namespace StageLink.Models;

public partial class JobOffer : ObservableObject
{
	public int Id { get; init; }
	public string Title { get; init; } = string.Empty;
	public string DetailTitle { get; init; } = string.Empty;
	public string Company { get; init; } = string.Empty;
	public string Location { get; init; } = string.Empty;
	public string Duration { get; init; } = string.Empty;
	public string Salary { get; init; } = string.Empty;
	public string StartDate { get; init; } = string.Empty;
	public string Category { get; init; } = string.Empty;
	public bool IsFeatured { get; init; }
	public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
	public IReadOnlyList<string> Skills { get; init; } = Array.Empty<string>();
	public IReadOnlyList<MissionItem> Missions { get; init; } = Array.Empty<MissionItem>();

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(BookmarkGlyph))]
	private bool isBookmarked;

	public string HeaderTitle => string.IsNullOrEmpty(DetailTitle) ? Title : DetailTitle;
	public string Subtitle => $"{Company} • {Location}";
	public string DurationSalary => $"{Duration} • {Salary}";
	public string ActionLabel => IsFeatured ? "Postuler" : "Voir l'offre";
	public string BookmarkGlyph => IsBookmarked ? "★" : "☆";
}
