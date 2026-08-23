using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Bicep.Local.Extension.Host.Handlers;

namespace Bicep.Extension.Utilities.Handlers;

public class CommandHandler : TypedResourceHandler<CommandResource, CommandResourceIdentifiers>
{
    private record RunResponse(
        int ExitCode,
        string StdOut,
        string StdErr);

    protected override async Task<ResourceResponse> Preview(ResourceRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return GetResponse(request);
    }

    protected override async Task<ResourceResponse> CreateOrUpdate(ResourceRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var output = RunCommand(request.Properties.Command);

        request.Properties.StdOut = output.StdOut;
        request.Properties.StdErr = output.StdErr;
        request.Properties.ExitCode = output.ExitCode;

        return GetResponse(request);
    }

    private static RunResponse RunCommand(string command)
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command.Split(' ')[0],
                Arguments = string.Join(' ', command.Split(' ').Skip(1)),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                CreateNoWindow = true,
            }
        };

        proc.Start();
        var stdout = proc.StandardOutput.ReadToEnd();
        var stderr = proc.StandardError.ReadToEnd();
        proc.WaitForExit();

        return new(proc.ExitCode, stdout, stderr);
    }

    protected override CommandResourceIdentifiers GetIdentifiers(CommandResource properties)
        => new()
        {
            Command = properties.Command
        };
}
