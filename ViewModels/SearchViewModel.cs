using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class SearchViewModel : OfferListViewModelBase
{
	private readonly IReadOnlyList<JobOffer> _all;

	public ObservableCollection<SearchFilter> AppliedFilters { get; } = new();
	public ObservableCollection<SearchFilter> SuggestedFilters { get; } = new();
	public ObservableCollection<JobOffer> Results { get; } = new();

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasQuery))]
	private string searchText = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasResults))]
	private bool isEmpty;

	public bool HasQuery => !string.IsNullOrEmpty(SearchText);
	public bool HasResults => !IsEmpty;
	public bool HasFilters => AppliedFilters.Count > 0;
	public string FilterCountLabel => AppliedFilters.Count <= 1
		? $"{AppliedFilters.Count} filtre"
		: $"{AppliedFilters.Count} filtres";

	public SearchViewModel(IDataService data)
	{
		_all = data.GetOffers();

		SuggestedFilters.Add(new SearchFilter("category", "Data"));
		SuggestedFilters.Add(new SearchFilter("category", "DevOps"));
		SuggestedFilters.Add(new SearchFilter("category", "Python"));
		SuggestedFilters.Add(new SearchFilter("duration", "6 mois"));
		SuggestedFilters.Add(new SearchFilter("skill", "Docker"));
		SuggestedFilters.Add(new SearchFilter("skill", "SQL"));

		// État initial = celui de la maquette (recherche "Power BI" + 3 filtres)
		AppliedFilters.CollectionChanged += (_, _) =>
		{
			OnPropertyChanged(nameof(HasFilters));
			OnPropertyChanged(nameof(FilterCountLabel));
			Search();
		};
		AppliedFilters.Add(new SearchFilter("category", "Data"));
		AppliedFilters.Add(new SearchFilter("duration", "6 mois"));
		AppliedFilters.Add(new SearchFilter("skill", "Power BI"));
		SearchText = "Power BI";
	}

	partial void OnSearchTextChanged(string value) => Search();

	[RelayCommand]
	private void ClearQuery() => SearchText = string.Empty;

	[RelayCommand]
	private void ClearAll()
	{
		SearchText = string.Empty;
		AppliedFilters.Clear();
	}

	[RelayCommand]
	private void AddFilter(SearchFilter? filter)
	{
		if (filter is null || AppliedFilters.Contains(filter)) return;
		AppliedFilters.Add(filter);
	}

	[RelayCommand]
	private void RemoveFilter(SearchFilter? filter)
	{
		if (filter is null) return;
		AppliedFilters.Remove(filter);
	}

	private void Search()
	{
		var query = SearchText.Trim();
		IEnumerable<JobOffer> items = _all;

		if (query.Length > 0)
		{
			items = items.Where(o =>
				o.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				o.Company.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				o.Location.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				o.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)));
		}

		foreach (var filter in AppliedFilters)
		{
			var f = filter;
			items = items.Where(o => Matches(o, f));
		}

		var list = items.ToList();
		Results.Clear();
		foreach (var o in list) Results.Add(o);
		IsEmpty = Results.Count == 0;
	}

	private static bool Matches(JobOffer offer, SearchFilter filter) => filter.Kind switch
	{
		"category" => offer.Category.Contains(filter.Label, StringComparison.OrdinalIgnoreCase),
		"duration" => offer.Duration.StartsWith(filter.Label, StringComparison.OrdinalIgnoreCase),
		"skill" => offer.Skills.Any(s => s.Equals(filter.Label, StringComparison.OrdinalIgnoreCase)) ||
		           offer.Tags.Any(t => t.Equals(filter.Label, StringComparison.OrdinalIgnoreCase)),
		_ => true
	};
}
