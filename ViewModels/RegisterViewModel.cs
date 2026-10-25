using System.Net.Mail;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using StageLink.Models;
using StageLink.Services;

namespace StageLink.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
	private static readonly Color EmptyBar = Color.FromArgb("#31353C");
	private static readonly Color WeakColor = Color.FromArgb("#FFB4AB");
	private static readonly Color MediumColor = Color.FromArgb("#7BD0FF");
	private static readonly Color StrongColor = Color.FromArgb("#4EDEA3");
	private static readonly Color NeutralColor = Color.FromArgb("#86948A");

	private readonly IAuthService _auth;

	[ObservableProperty] private string fullName = string.Empty;
	[ObservableProperty] private string institution = string.Empty;
	[ObservableProperty] private string major = string.Empty;
	[ObservableProperty] private string email = string.Empty;
	[ObservableProperty] private bool acceptTerms;
	[ObservableProperty] private bool isRegistering;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(PasswordToggleText))]
	private bool isPasswordHidden = true;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasError))]
	private string errorMessage = string.Empty;

	[ObservableProperty] private string password = string.Empty;
	[ObservableProperty] private string passwordStrengthLabel = "Vide";
	[ObservableProperty] private Color passwordStrengthColor = NeutralColor;
	[ObservableProperty] private Color bar1Color = EmptyBar;
	[ObservableProperty] private Color bar2Color = EmptyBar;
	[ObservableProperty] private Color bar3Color = EmptyBar;

	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
	public string PasswordToggleText => IsPasswordHidden ? "Afficher" : "Masquer";

	public RegisterViewModel(IAuthService auth)
	{
		_auth = auth;
	}

	partial void OnPasswordChanged(string value)
	{
		var length = value.Length;
		if (length == 0)
		{
			SetStrength("Vide", NeutralColor, EmptyBar, EmptyBar, EmptyBar);
		}
		else if (length < 6)
		{
			SetStrength("Faible", WeakColor, WeakColor, EmptyBar, EmptyBar);
		}
		else if (length < 10)
		{
			SetStrength("Moyen", MediumColor, MediumColor, MediumColor, EmptyBar);
		}
		else
		{
			SetStrength("Robuste", StrongColor, StrongColor, StrongColor, StrongColor);
		}
	}

	private void SetStrength(string label, Color labelColor, Color b1, Color b2, Color b3)
	{
		PasswordStrengthLabel = label;
		PasswordStrengthColor = labelColor;
		Bar1Color = b1;
		Bar2Color = b2;
		Bar3Color = b3;
	}

	[RelayCommand]
	private void TogglePassword() => IsPasswordHidden = !IsPasswordHidden;

	[RelayCommand]
	private async Task RegisterAsync()
	{
		if (IsRegistering) return;
		ErrorMessage = string.Empty;

		if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Institution) ||
		    string.IsNullOrWhiteSpace(Major) || string.IsNullOrWhiteSpace(Email))
		{
			ErrorMessage = "Veuillez renseigner tous les champs.";
			return;
		}

		if (!IsValidEmail(Email))
		{
			ErrorMessage = "L'adresse email n'est pas valide.";
			return;
		}

		if (Password.Length < 6)
		{
			ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.";
			return;
		}

		if (!AcceptTerms)
		{
			ErrorMessage = "Vous devez accepter les conditions générales d'utilisation.";
			return;
		}

		IsRegistering = true;
		try
		{
			var profile = new UserProfile
			{
				FullName = FullName,
				Institution = Institution,
				Major = Major,
				Email = Email,
				Domain = Major
			};

			var result = await _auth.RegisterAsync(profile, Password);
			if (!result.Success)
			{
				ErrorMessage = result.Message;
				return;
			}

			await Shell.Current.GoToAsync("//MainTabs/offres");
		}
		finally
		{
			IsRegistering = false;
		}
	}

	private static bool IsValidEmail(string value)
	{
		return MailAddress.TryCreate(value.Trim(), out var address) && address.Host.Contains('.');
	}
}
