using System.Text.Json.Serialization;

namespace Tgs.GameInterface.Cards365.Core.Models;

public record BalanceInfo
{
    [JsonPropertyName("user")]
    public string User { get; init; } = default!;

    [JsonPropertyName("currency")]
    public string Currency { get; init; } = default!;

    [JsonPropertyName("extension1")]
    public string Extension1 { get; init; } = default!;

    [JsonPropertyName("balance")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Balance { get; init; } = default!;
}
