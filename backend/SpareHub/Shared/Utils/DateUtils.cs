using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Service.Utils;

public class DateUtils
{
    public static string FormatToLocalDate(string? dateTimeString)
    {
        var dateTimeUtc = DateTime.Parse(dateTimeString ?? throw new ValidationException("Unable to format date"), null,
            DateTimeStyles.AdjustToUniversal);

        var localDateTime = dateTimeUtc.ToLocalTime();

        return localDateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}