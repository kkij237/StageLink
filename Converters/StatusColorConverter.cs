using System.Globalization;
using Microsoft.Maui.Graphics;
using StageLink.Models;

namespace StageLink.Converters;

/// <summary>ConverterParameter = "bg" (fond de la pastille) ou "fg" (texte).</summary>
public class StatusColorConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		var status = value is ApplicationStatus s ? s : ApplicationStatus.Sent;
		var foreground = string.Equals(parameter as string, "fg", StringComparison.OrdinalIgnoreCase);

		return status switch
		{
			ApplicationStatus.Interview => Color.FromArgb(foreground ? "#7BD0FF" : "#267BD0FF"),
			ApplicationStatus.Accepted => Color.FromArgb(foreground ? "#4EDEA3" : "#264EDEA3"),
			ApplicationStatus.InReview => Color.FromArgb(foreground ? "#68DBA9" : "#262A31"),
			_ => Color.FromArgb(foreground ? "#BBCABF" : "#262A31")
		};
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}
