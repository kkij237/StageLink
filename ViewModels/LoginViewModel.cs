using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
	private readonly IAuthService _auth;
	private readonly IDemoTourService _demo;
	private readonly IDialogService _dialog;

	[ObservableProperty] private string email = string.Empty;
	[ObservableProperty] private string password = string.Empty;
	[ObservableProperty] private bool rememberMe = true;
	[ObservableProperty] private bool isDemoRunning;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(PasswordToggleText))]
	private bool isPasswordHidden = true;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasError))]
	private string errorMessage = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(LoginButtonText))]
	private bool isSigningIn;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsStudent), nameof(IsRecruiter), nameof(RoleHint), nameof(EmailPlaceholder))]
	private string selectedRole = "student";

	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
	public bool IsStudent => SelectedRole == "student";
	public bool IsRecruiter => SelectedRole == "recruiter";
	public string RoleHint => IsStudent ? "univ-tech.edu" : "entreprise.com";
	public string EmailPlaceholder => IsStudent ? "prenom.nom@univ-tech.edu" : "recrutement@entreprise.com";
	public string PasswordToggleText => IsPasswordHidden ? "Afficher" : "Masquer";
	public string LoginButtonText => IsSigningIn ? "Authentification..." : "Se connecter  →";
	public string DemoHint => $"Compte de démo : {AuthService.DemoEmail} / {AuthService.DemoPassword}";

	public LoginViewModel(IAuthService auth, IDemoTourService demo, IDialogService dialog)
	{
		_auth = auth;
		_demo = demo;
		_dialog = dialog;
		Email = auth.RememberedEmail;
	}

	[RelayCommand]
	private void SelectRole(string? role) => SelectedRole = role == "recruiter" ? "recruiter" : "student";

	[RelayCommand]
	private void TogglePassword() => IsPasswordHidden = !IsPasswordHidden;

	[RelayCommand]
	private async Task LoginAsync()
	{
		if (IsSigningIn) return;
		ErrorMessage = string.Empty;

		if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
		{
			ErrorMessage = "Veuillez renseigner votre email et votre mot de passe.";
			return;
		}

		IsSigningIn = true;
		try
		{
			await Task.Delay(500); // simulation du temps d'authentification
			var result = await _auth.LoginAsync(Email, Password);
			if (!result.Success)
			{
				ErrorMessage = result.Message;
				return;
			}

			_auth.SaveRememberedEmail(RememberMe ? Email : null);
			Password = string.Empty;
			await Shell.Current.GoToAsync("//MainTabs/offres");
		}
		finally
		{
			IsSigningIn = false;
		}
	}

	[RelayCommand]
	private async Task GoToRegisterAsync() => await Shell.Current.GoToAsync("RegisterPage");

	[RelayCommand]
	private async Task ForgotPasswordAsync()
		=> await _dialog.ShowAlertAsync("Mot de passe oublié",
			"Un lien de réinitialisation sera envoyé à votre adresse académique (fonction simulée).");

	[RelayCommand]
	private async Task StartDemoAsync()
	{
		if (IsDemoRunning) return;
		IsDemoRunning = true;
		try
		{
			_auth.SignInDemo();
			await _demo.RunAsync();
		}
		finally
		{
			IsDemoRunning = false;
		}
	}

	/// <summary>Appelée à l'apparition de la page : lance la démo si l'app a été démarrée avec --ez demo true.</summary>
	[RelayCommand]
	private async Task AppearingAsync()
	{
		if (Preferences.Default.Get("demo_autostart", false))
		{
			Preferences.Default.Remove("demo_autostart");
			await StartDemoAsync();
		}
	}
}
