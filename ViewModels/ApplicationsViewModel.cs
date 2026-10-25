using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class ApplicationsViewModel : BaseViewModel
{
	private readonly IDataService _data;
	private readonly IDialogService _dialog;
	private string _filterKey = "all";

	public ObservableCollection<JobApplication> Applications { get; } = new();
	public ObservableCollection<FilterChip> Filters { get; } = new();

	[ObservableProperty] private string countLabel = string.Empty;
	[ObservableProperty] private bool isEmpty;

	public ApplicationsViewModel(IDataService data, IDialogService dialog)
	{
		_data = data;
		_dialog = dialog;

		Filters.Add(new FilterChip { Key = "all", Label = "Toutes" });
		Filters.Add(new FilterChip { Key = "sent", Label = "Envoyées" });
		Filters.Add(new FilterChip { Key = "review", Label = "En cours de revue" });
		Filters.Add(new FilterChip { Key = "interview", Label = "Entretien" });
		Filters.Add(new FilterChip { Key = "final", Label = "Finalisées" });

		Refresh();
	}

	[RelayCommand]
	private void Appearing() => Refresh();

	[RelayCommand]
	private void SelectFilter(FilterChip? chip)
	{
		if (chip is null) return;
		_filterKey = chip.Key;
		Refresh();
	}

	[RelayCommand]
	private async Task OpenActionAsync(JobApplication? application)
	{
		if (application is null) return;

		var message = application.Status switch
		{
			ApplicationStatus.Interview => $"Ouverture du lien de visioconférence pour l'entretien chez {application.Company} (simulation).",
			ApplicationStatus.Accepted => $"Téléchargement de la convention de stage de {application.Company} (simulation).",
			ApplicationStatus.InReview => $"Votre candidature « {application.Position} » est en cours d'examen par {application.Company}.",
			_ => $"Votre candidature « {application.Position} » a bien été transmise à {application.Company}."
		};

		await _dialog.ShowAlertAsync(application.Company, message);
	}

	private void Refresh()
	{
		var all = _data.GetApplications();

		foreach (var chip in Filters)
		{
			chip.Count = chip.Key == "all" ? all.Count : all.Count(a => a.FilterKey == chip.Key);
			chip.IsSelected = chip.Key == _filterKey;
		}

		var items = _filterKey == "all" ? all.ToList() : all.Where(a => a.FilterKey == _filterKey).ToList();

		Applications.Clear();
		foreach (var item in items) Applications.Add(item);

		CountLabel = all.Count <= 1 ? $"{all.Count} candidature" : $"{all.Count} candidatures";
		IsEmpty = Applications.Count == 0;
	}
}
