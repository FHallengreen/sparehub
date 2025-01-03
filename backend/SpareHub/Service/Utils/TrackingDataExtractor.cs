using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Utils;

namespace Service.Utils;

public static class TrackingDataExtractor
{
    public static TrackingResponse ExtractLatestTrackingData(string jsonResponse)
    {
        var jsonDocument = JsonDocument.Parse(jsonResponse);

        Console.WriteLine(jsonDocument.RootElement.ToString());

        if (!jsonDocument.RootElement.TryGetProperty("shipments", out var shipments) ||
            shipments.ValueKind != JsonValueKind.Array || shipments.GetArrayLength() == 0)
        {
            throw new NotFoundException("Expected 'shipments' array is missing or empty in the JSON response.");
        }

        var shipment = shipments[0];

        if (!shipment.TryGetProperty("events", out var eventsElement) ||
            eventsElement.ValueKind != JsonValueKind.Array || !eventsElement.EnumerateArray().Any())
        {
            throw new NotFoundException("Expected 'events' array is missing or empty in the shipment.");
        }

        var events = eventsElement.EnumerateArray().ToList();
        var latestEvent = events.OrderByDescending(e =>
                DateTime.Parse(e.GetProperty("timestamp").GetString() ??
                               throw new FormatException("Invalid timestamp format in event.")))
            .FirstOrDefault();

        if (latestEvent.ValueKind == JsonValueKind.Undefined)
        {
            throw new NotFoundException("No valid events found in the 'events' array.");
        }

        var statusDescription = latestEvent.TryGetProperty("description", out var description)
            ? description.GetString()
            : "Unknown status";
        var location = latestEvent.TryGetProperty("location", out var locationElement) &&
                       locationElement.TryGetProperty("address", out var addressElement) &&
                       addressElement.TryGetProperty("addressLocality", out var locality)
            ? locality.GetString()
            : "Unknown location";

        var timestamp = latestEvent.TryGetProperty("timestamp", out var timestampProperty)
            ? timestampProperty.GetString()
            : null;

        var estimatedDelivery = shipment.TryGetProperty("estimatedTimeOfDelivery", out var estimatedDeliveryProperty)
            ? estimatedDeliveryProperty.GetString()
            : null;

        return new TrackingResponse
        {
            StatusDescription = statusDescription ?? "Unknown status",
            Location = location ?? "Unknown location",
            Timestamp = timestamp != null
                ? DateTimeOffset.Parse(timestamp).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                : "Unknown timestamp",
            EstimatedDelivery = estimatedDelivery != null
                ? DateTimeOffset.Parse(estimatedDelivery).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                : "Unknown delivery date"
        };
    }
}