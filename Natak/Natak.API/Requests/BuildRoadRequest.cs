using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class BuildRoadRequest
{
    [JsonPropertyName("firstPoint")]
    public required PointRequest FirstPoint { get; init; }

    [JsonPropertyName("secondPoint")]
    public required PointRequest SecondPoint { get; init; }
}
