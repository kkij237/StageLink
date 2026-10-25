using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class OffersViewModel : OfferListViewModelBase
{
	private readonly IReadOnlyList<JobOffer> _all;

	public ObservableCollection<JobOffer> Offers { get; } = new();
	public ObservableCollection<FilterChip> Filters { get; } = new();

	[ObservableProperty] private string searchText = string.Empty;
	[ObservableProperty] private bool isEmpty;

	public OffersViewModel(IDataService data)
	{
		_all = data.GetOffers();

		Filters.Add(new FilterChip { Key = "all", Label = "Tous", IsSelected = true });
		Filters.Add(new FilterChip { Key = "C# / .NET", Label = "C# / .NET" });
		Filters.Add(new FilterChip { Key = "Python", Label = "Python" });
		Filters.Add(new FilterChip { Key = "Data & BI", Label = "Data & BI" });

		ApplyFilter();
	}

	partial void OnSearchTextChanged(string value) => ApplyFilter();

	[RelayCommand]
	private void SelectFilter(FilterChip? chip)
	{
		if (chip is null) return;
		foreach (var f in Filters) f.IsSelected = f == chip;
		ApplyFilter();
	}

	private void ApplyFilter()
	{
		var selected = Filters.FirstOrDefault(f => f.IsSelected)?.Key ?? "all";
		var query = SearchText.Trim();

		var items = _all.Where(o =>
			(selected == "all" || o.Category == selected || o.Tags.Contains(selected)) &&
			(query.Length == 0 ||
			 o.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
			 o.Company.Contains(query, StringComparison.OrdinalIgnoreCase) ||
			 o.Location.Contains(query, StringComparison.OrdinalIgnoreCase) ||
			 o.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)))).ToList();

		Offers.Clear();
		foreach (var item in items) Offers.Add(item);
		IsEmpty = Offers.Count == 0;
	}
}
