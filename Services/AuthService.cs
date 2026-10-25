using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using StageLink.Models;

namespace StageLink.Services;

public class StoredAccount
{
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Institution { get; set; } = string.Empty;
	public string Major { get; set; } = string.Empty;
}

/// <summary>
/// Authentification locale (projet pédagogique sans back-end) :
/// comptes enregistrés dans Preferences, mots de passe hachés (SHA-256).
/// </summary>
public class AuthService : IAuthService
{
	public const string DemoEmail = "idriss.kamfo7@univ-tech.edu";
	public const string DemoPassword = "StageLink2026";

	private const string AccountsKey = "stagelink_accounts";
	private const string RememberedKey = "stagelink_remembered_email";

	public UserProfile? CurrentUser { get; private set; }

	public string RememberedEmail => Preferences.Default.Get(RememberedKey, string.Empty);

	public Task<AuthResult> LoginAsync(string email, string password)
	{
		var mail = Normalize(email);

		if (mail == DemoEmail && password == DemoPassword)
		{
			CurrentUser = UserProfile.CreateDemo();
			return Task.FromResult(new AuthResult(true, string.Empty));
		}

		var account = LoadAccounts().FirstOrDefault(a => a.Email == mail);
		if (account is null || account.PasswordHash != Hash(mail, password))
		{
			return Task.FromResult(new AuthResult(false, "Identifiants non reconnus. Vérifiez votre saisie."));
		}

		CurrentUser = ToProfile(account);
		return Task.FromResult(new AuthResult(true, string.Empty));
	}

	public Task<AuthResult> RegisterAsync(UserProfile profile, string password)
	{
		var mail = Normalize(profile.Email);
		var accounts = LoadAccounts();

		if (mail == DemoEmail || accounts.Any(a => a.Email == mail))
		{
			return Task.FromResult(new AuthResult(false, "Un compte existe déjà avec cet email."));
		}

		var account = new StoredAccount
		{
			Email = mail,
			PasswordHash = Hash(mail, password),
			FullName = profile.FullName.Trim(),
			Institution = profile.Institution.Trim(),
			Major = profile.Major.Trim()
		};
		accounts.Add(account);
		Preferences.Default.Set(AccountsKey, JsonSerializer.Serialize(accounts));

		CurrentUser = ToProfile(account);
		return Task.FromResult(new AuthResult(true, string.Empty));
	}

	public void SignInDemo() => CurrentUser = UserProfile.CreateDemo();

	public void Logout() => CurrentUser = null;

	public void SaveRememberedEmail(string? email)
	{
		if (string.IsNullOrWhiteSpace(email))
			Preferences.Default.Remove(RememberedKey);
		else
			Preferences.Default.Set(RememberedKey, email.Trim());
	}

	private static string Normalize(string email) => email.Trim().ToLowerInvariant();

	private static string Hash(string email, string password)
		=> Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{email}:{password}")));

	private static UserProfile ToProfile(StoredAccount a) => new()
	{
		FullName = a.FullName,
		Institution = a.Institution,
		Major = a.Major,
		Email = a.Email,
		Domain = a.Major
	};

	private static List<StoredAccount> LoadAccounts()
	{
		var json = Preferences.Default.Get(AccountsKey, string.Empty);
		if (string.IsNullOrWhiteSpace(json)) return new List<StoredAccount>();

		try
		{
			return JsonSerializer.Deserialize<List<StoredAccount>>(json) ?? new List<StoredAccount>();
		}
		catch (JsonException)
		{
			return new List<StoredAccount>();
		}
	}
}
