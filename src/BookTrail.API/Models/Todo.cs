using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace BookTrail.API;

public record Todo(int Id, string Title, DateOnly? DueBy = null, bool IsComplete = false);


[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}