using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
	private readonly IAuthService _auth;
	private readonly IDialogService _dialog;

	[ObservableProperty] private UserProfile profile;

	public ProfileViewModel(IAuthService auth, IDialogService dialog)
	{
		_auth = auth;
		_dialog = dialog;
		profile = auth.CurrentUser ?? UserProfile.CreateDemo();
	}

	[RelayCommand]
	private void Appearing() => Profile = _auth.CurrentUser ?? UserProfile.CreateDemo();

	[RelayCommand]
	private async Task DownloadCvAsync()
		=> await _dialog.ShowAlertAsync("Document", $"{Profile.CvLabel} : téléchargement simulé.");

	[RelayCommand]
	private async Task LogoutAsync()
	{
		_auth.Logout();
		await Shell.Current.GoToAsync("//LoginPage");
	}
}
