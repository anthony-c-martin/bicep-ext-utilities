using System.Text.Json;
using Bicep.Local.Extension.Host.Handlers;
using Bicep.Local.Rpc;

namespace Bicep.Extension.Utilities.Tests;

public static class HandlerHarness
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static Task<LocalExtensibilityOperationResponse> PreviewAsync(
        IResourceHandler handler,
        string type,
        object properties,
        CancellationToken cancellationToken = default)
    {
        var specification = CreateSpecification(type, properties);
        return handler.Preview(specification, cancellationToken);
    }

    public static Task<LocalExtensibilityOperationResponse> CreateOrUpdateAsync(
        IResourceHandler handler,
        string type,
        object properties,
        CancellationToken cancellationToken = default)
    {
        var specification = CreateSpecification(type, properties);
        return handler.CreateOrUpdate(specification, cancellationToken);
    }

    public static JsonElement ResourceProperties(this LocalExtensibilityOperationResponse response)
    {
        Assert.IsNull(response.ErrorData);
        Assert.IsNotNull(response.Resource);
        return JsonSerializer.Deserialize<JsonElement>(response.Resource.Properties);
    }

    private static ResourceSpecification CreateSpecification(string type, object properties) => new()
    {
        Type = type,
        Config = JsonSerializer.Serialize(new { accessToken = "test-token" }, SerializerOptions),
        Properties = JsonSerializer.Serialize(properties, SerializerOptions),
    };
}
