using StageLink.ViewModels;

namespace StageLink.Views;

public partial class AlertsPage : ContentPage
{
	public AlertsPage(AlertsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
