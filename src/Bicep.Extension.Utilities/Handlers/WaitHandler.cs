using System.Text.Json;
using System.Text.Json.Nodes;
using Bicep.Local.Extension.Host.Handlers;

namespace Bicep.Extension.Utilities.Handlers;

public class WaitHandler : TypedResourceHandler<WaitResource, WaitResourceIdentifiers>
{
    protected override async Task<ResourceResponse> Preview(ResourceRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return GetResponse(request);
    }

    protected override async Task<ResourceResponse> CreateOrUpdate(ResourceRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(request.Properties.DurationMs, cancellationToken);

        return GetResponse(request);
    }

    protected override WaitResourceIdentifiers GetIdentifiers(WaitResource properties)
        => new();
}
