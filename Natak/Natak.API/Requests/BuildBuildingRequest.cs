using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class BuildBuildingRequest
{
    [JsonPropertyName("point")]
    public required PointRequest Point { get; init; }
}
