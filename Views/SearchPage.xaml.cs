using StageLink.ViewModels;

namespace StageLink.Views;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
