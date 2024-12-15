using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using Shared.DTOs.Order;
using Shared.Utils;

namespace Service.Utils;

public class TrackingDataExtractor
{
    public TrackingResponse ExtractLatestTrackingData(string jsonResponse)
{
    var jsonDocument = JsonDocument.Parse(jsonResponse);

    if (!jsonDocument.RootElement.TryGetProperty("shipments", out var shipments) || shipments.ValueKind != JsonValueKind.Array || shipments.GetArrayLength() == 0)
    {
        throw new KeyNotFoundException("Expected 'shipments' array is missing or empty in the JSON response.");
    }

    var shipment = shipments[0];

    if (!shipment.TryGetProperty("events", out var eventsElement) || eventsElement.ValueKind != JsonValueKind.Array || !eventsElement.EnumerateArray().Any())
    {
        throw new KeyNotFoundException("Expected 'events' array is missing or empty in the shipment.");
    }

    // Extract and order events
    var events = eventsElement.EnumerateArray().ToList();
    var latestEvent = events.OrderByDescending(e =>
        DateTime.Parse(e.GetProperty("timestamp").GetString() ?? throw new FormatException("Invalid timestamp format in event.")))
        .FirstOrDefault();

    if (latestEvent.ValueKind == JsonValueKind.Undefined)
    {
        throw new InvalidOperationException("No valid events found in the 'events' array.");
    }

    var statusDescription = latestEvent.TryGetProperty("description", out var description) ? description.GetString() : "Unknown status";
    var location = latestEvent.TryGetProperty("location", out var locationElement) &&
                   locationElement.TryGetProperty("address", out var addressElement) &&
                   addressElement.TryGetProperty("addressLocality", out var locality)
        ? locality.GetString()
        : "Unknown location";

    var timestamp = latestEvent.TryGetProperty("timestamp", out var timestampProperty) ? timestampProperty.GetString() : null;

    var estimatedDelivery = shipment.TryGetProperty("estimatedTimeOfDelivery", out var estimatedDeliveryProperty)
        ? estimatedDeliveryProperty.GetString()
        : null;

    return new TrackingResponse
    {
        StatusDescription = statusDescription ?? "Unknown status",
        Location = location ?? "Unknown location",
        Timestamp = timestamp != null ? DateUtils.FormatToLocalDate(timestamp) : "Unknown timestamp",
        EstimatedDelivery = estimatedDelivery != null ? DateUtils.FormatToLocalDate(estimatedDelivery) : "Unknown delivery date"
    };
}


}