using StageLink.ViewModels;

namespace StageLink.Views;

public partial class OfferDetailPage : ContentPage
{
	public OfferDetailPage(OfferDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
