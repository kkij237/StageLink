using StageLink.ViewModels;

namespace StageLink.Views;

public partial class LoginPage : ContentPage
{
	private readonly LoginViewModel _viewModel;

	public LoginPage(LoginViewModel viewModel)
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
