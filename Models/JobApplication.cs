namespace StageLink.Models;

public enum ApplicationStatus
{
	Sent,
	InReview,
	Interview,
	Accepted
}

public class JobApplication
{
	public int Id { get; init; }
	public int OfferId { get; init; }
	public string Company { get; init; } = string.Empty;
	public string Position { get; init; } = string.Empty;
	public ApplicationStatus Status { get; init; }
	public string DateLabel { get; init; } = string.Empty;

	public string Initials =>
		string.Concat(Company.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			.Take(2).Select(w => char.ToUpperInvariant(w[0])));

	public string StatusLabel => Status switch
	{
		ApplicationStatus.Interview => "Entretien",
		ApplicationStatus.InReview => "En attente",
		ApplicationStatus.Accepted => "Acceptée",
		_ => "Envoyée"
	};

	public string ActionLabel => Status switch
	{
		ApplicationStatus.Interview => "Rejoindre 🎥",
		ApplicationStatus.Accepted => "Convention 📄",
		ApplicationStatus.InReview => "Détails ›",
		_ => "Suivi ›"
	};

	public string FilterKey => Status switch
	{
		ApplicationStatus.Interview => "interview",
		ApplicationStatus.InReview => "review",
		ApplicationStatus.Accepted => "final",
		_ => "sent"
	};

	public bool IsPrimaryAction => Status is ApplicationStatus.Interview or ApplicationStatus.Accepted;
	public bool IsSecondaryAction => !IsPrimaryAction;
}
