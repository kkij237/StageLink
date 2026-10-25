using StageLink.Models;

namespace StageLink.Services;

public record AuthResult(bool Success, string Message);

public interface IAuthService
{
	UserProfile? CurrentUser { get; }
	string RememberedEmail { get; }
	Task<AuthResult> LoginAsync(string email, string password);
	Task<AuthResult> RegisterAsync(UserProfile profile, string password);
	void SignInDemo();
	void Logout();
	void SaveRememberedEmail(string? email);
}
