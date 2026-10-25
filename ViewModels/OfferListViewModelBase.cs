using CommunityToolkit.Mvvm.Input;
using StageLink.Models;

namespace StageLink.ViewModels;

/// <summary>Commandes communes aux écrans qui affichent des cartes d'offres.</summary>
public abstract partial class OfferListViewModelBase : BaseViewModel
{
	[RelayCommand]
	private async Task OpenOfferAsync(JobOffer? offer)
	{
		if (offer is null) return;
		await Shell.Current.GoToAsync($"OfferDetailPage?offerId={offer.Id}");
	}

	[RelayCommand]
	private void ToggleBookmark(JobOffer? offer)
	{
		if (offer is null) return;
		offer.IsBookmarked = !offer.IsBookmarked;
	}
}
