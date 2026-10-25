namespace StageLink.Services;

public interface IDialogService
{
	Task ShowAlertAsync(string title, string message, string accept = "OK");
}
