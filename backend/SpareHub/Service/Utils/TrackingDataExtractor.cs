using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using Shared.DTOs.Order;

namespace Service.Utils;

public class TrackingDataExtractor
{
    public TrackingResponse ExtractLatestTrackingData(string jsonResponse)
    {
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var shipment = jsonDocument.RootElement.GetProperty("shipments")[0];
        var events = shipment.GetProperty("events").EnumerateArray().ToList();

        var latestEvent = events.OrderByDescending(e =>
            DateTime.Parse(e.GetProperty("timestamp").GetString()!)).First();

        var statusDescription = latestEvent.GetProperty("description").GetString();
        var location = latestEvent.GetProperty("location").GetProperty("address")
            .GetProperty("addressLocality").GetString();
        var timestamp = latestEvent.GetProperty("timestamp").GetString();

        var estimatedDelivery = shipment.GetProperty("estimatedTimeOfDelivery").ToString();

        return new TrackingResponse
        {
            StatusDescription = statusDescription!,
            Location = location!,
            Timestamp = FormatToLocalDate(timestamp),
            EstimatedDelivery = FormatToLocalDate(estimatedDelivery)
        };
    }

    private string FormatToLocalDate(string? dateTimeString)
    {

        var dateTimeUtc = DateTime.Parse(dateTimeString ?? throw new ValidationException("Unable to format date"), null, DateTimeStyles.AdjustToUniversal);

        var localDateTime = dateTimeUtc.ToLocalTime();

        return localDateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}