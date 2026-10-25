using StageLink.ViewModels;

namespace StageLink.Views;

public partial class ApplicationsPage : ContentPage
{
	private readonly ApplicationsViewModel _viewModel;

	public ApplicationsPage(ApplicationsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.AppearingCommand.Execute(null);
	}
}
