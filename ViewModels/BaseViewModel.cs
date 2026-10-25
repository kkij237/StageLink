using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StageLink.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotBusy))]
	private bool isBusy;

	[ObservableProperty]
	private string title = string.Empty;

	public bool IsNotBusy => !IsBusy;

	[RelayCommand]
	private async Task OpenAlertsAsync() => await Shell.Current.GoToAsync("AlertsPage");

	[RelayCommand]
	private async Task OpenProfileAsync() => await Shell.Current.GoToAsync("//MainTabs/profil");

	[RelayCommand]
	private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
