using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.Utils;

public static class DateUtils
{
    public static string FormatToLocalDate(string? dateTimeString, bool includeTime = false)
    {
        var dateTimeUtc = DateTime.Parse(dateTimeString ?? throw new ValidationException("Unable to format date"),
            null, DateTimeStyles.AdjustToUniversal);

        var localDateTime = dateTimeUtc.ToLocalTime();

        return localDateTime.ToString(
            includeTime ? "dd-MM-yyyy HH:mm:ss" : "dd-MM-yyyy",
            CultureInfo.InvariantCulture);
    }

    public static string FormatToLocalDate(DateTime? dateTime, bool includeTime = false)
    {
        if (dateTime == null)
            return string.Empty;

        var localDateTime = dateTime.Value.ToLocalTime();

        return localDateTime.ToString(
            includeTime ? "dd-MM-yyyy HH:mm:ss" : "dd-MM-yyyy",
            CultureInfo.InvariantCulture);
    }

    public static string FormatToReverseLocalDate(DateTime? dateTime, bool includeTime = false)
    {
        if (dateTime == null)
            return string.Empty;

        var localDateTime = dateTime.Value.ToLocalTime();

        return localDateTime.ToString(
            includeTime ? "yyyy-MM-dd HH:mm:ss" : "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
    }
}