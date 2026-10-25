namespace StageLink.Services;

public class DialogService : IDialogService
{
	public async Task ShowAlertAsync(string title, string message, string accept = "OK")
	{
		if (Shell.Current is { } shell)
		{
			await shell.DisplayAlert(title, message, accept);
		}
	}
}
