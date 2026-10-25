using StageLink.ViewModels;

namespace StageLink.Views;

public partial class OffersPage : ContentPage
{
	public OffersPage(OffersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
