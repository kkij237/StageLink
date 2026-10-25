namespace StageLink.Models;

public class UserProfile
{
	public string FullName { get; set; } = string.Empty;
	public string Institution { get; set; } = string.Empty;
	public string Major { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Status { get; set; } = "À la recherche d'un stage";
	public string Domain { get; set; } = string.Empty;
	public string Location { get; set; } = "À renseigner";
	public string Availability { get; set; } = "À renseigner";
	public List<string> Skills { get; set; } = new();

	public string Subtitle => $"{Institution} • {Major}";
	public string CvLabel => $"CV {FullName} (PDF)";
	public string SkillsCountLabel => Skills.Count <= 1 ? $"{Skills.Count} clé" : $"{Skills.Count} clés";

	public string Initials =>
		string.Concat(FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			.Take(2).Select(w => char.ToUpperInvariant(w[0])));

	public static UserProfile CreateDemo() => new()
	{
		FullName = "Idriss Kamfo",
		Institution = "École Nationale Supérieure Polytechnique de Douala",
		Major = "Génie Logiciel",
		Email = "idriss.kamfo7@univ-tech.edu",
		Status = "À la recherche d'un stage PFE (6 mois)",
		Domain = "Génie Logiciel",
		Location = "Douala (Télétravail accepté)",
		Availability = "Février 2027",
		Skills = new() { "C# / .NET", "Python", "SQL", "Power BI", "Docker", "Git" }
	};
}
