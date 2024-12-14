using System.ComponentModel.DataAnnotations;
using Service.Interfaces;
using Service.Utils;
using Shared.DTOs.Order;

namespace Service.Services.Order;

public class TrackingService(HttpClient httpClient) : ITrackingService
{
    private readonly string _dhlApiKey = Environment.GetEnvironmentVariable("DHL_API_KEY")
                                         ?? throw new ValidationException("DhlApiKey is not set.");

    public async Task<TrackingResponse> GetTrackingStatusAsync(string trackingNumber, string transporter)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new ValidationException("Tracking number cannot be null or empty.");

        if (transporter.ToLower() != "dhl")
            throw new NotSupportedException($"Transporter '{transporter}' is not supported.");

        var url = $"https://api-eu.dhl.com/track/shipments?trackingNumber={trackingNumber}&service=express";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("DHL-API-Key", _dhlApiKey);

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Error fetching tracking status: {response.StatusCode}. Details: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();

        var extractor = new TrackingDataExtractor();
        return extractor.ExtractLatestTrackingData(content);
    }
}
