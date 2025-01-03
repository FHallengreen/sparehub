
using System.Text.Json.Serialization;

namespace Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending,
    Ready,
    Inbound,
    Stock,
    Cancelled,
    Delivered
}