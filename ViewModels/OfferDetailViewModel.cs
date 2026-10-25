using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class OfferDetailViewModel : BaseViewModel, IQueryAttributable
{
	private readonly IDataService _data;

	[ObservableProperty] private JobOffer? offer;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(ApplyButtonText), nameof(IsApplyDone))]
	private bool hasApplied;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(ApplyButtonText))]
	private bool isApplying;

	public bool IsApplyDone => HasApplied;

	public string ApplyButtonText => HasApplied
		? "Candidature transmise avec succès !"
		: IsApplying ? "Transmission du profil..." : "✈  Postuler";

	public OfferDetailViewModel(IDataService data)
	{
		_data = data;
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("offerId", out var raw) && int.TryParse(raw?.ToString(), out var id))
		{
			Offer = _data.GetOffer(id);
			HasApplied = _data.HasApplied(id);
		}
	}

	[RelayCommand]
	private void ToggleBookmark()
	{
		if (Offer is null) return;
		Offer.IsBookmarked = !Offer.IsBookmarked;
	}

	[RelayCommand]
	private async Task ApplyAsync()
	{
		if (Offer is null || HasApplied || IsApplying) return;

		IsApplying = true;
		try
		{
			await Task.Delay(700); // simulation de l'envoi du profil
			_data.Apply(Offer);
			HasApplied = true;

			try
			{
				Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(80));
			}
			catch (FeatureNotSupportedException)
			{
				// Pas de vibreur : sans conséquence
			}
		}
		finally
		{
			IsApplying = false;
		}
	}
}
