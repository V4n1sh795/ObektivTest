namespace RequestModels;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
public record Subscribe
(
    [property: JsonPropertyName("link")] string? link,
    [property: JsonPropertyName("email")] string? email
);

public record NewPrice(
    [property: JsonPropertyName("link")] string link,
    [property: JsonPropertyName("new_price")] string price
);