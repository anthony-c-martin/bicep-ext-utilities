using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Bicep.Local.Extension.Host.Handlers;

namespace Bicep.Extension.Utilities.Handlers;

public class ScriptHandler : TypedResourceHandler<ScriptResource, ScriptResourceIdentifiers>
{
    private record RunScriptResponse(
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

        var scriptOutput = request.Properties.Type switch
        {
            ScriptType.Bash => RunBashScript(request.Properties.Script),
            ScriptType.PowerShell => RunPowerShellScript(request.Properties.Script),
            _ => throw new InvalidOperationException($"Unknown script type '{request.Properties.Type}'"),
        };

        request.Properties.StdOut = scriptOutput.StdOut;
        request.Properties.StdErr = scriptOutput.StdErr;
        request.Properties.ExitCode = scriptOutput.ExitCode;

        return GetResponse(request);
    }

    private static RunScriptResponse RunBashScript(string script)
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{script.Replace("\"", "\\\"")}\"",
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

    private static RunScriptResponse RunPowerShellScript(string script)
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pwsh",
                Arguments = $"-c \"{script.Replace("\"", "\\\"")}\"",
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

    protected override ScriptResourceIdentifiers GetIdentifiers(ScriptResource properties)
        => new()
        {
            Type = properties.Type,
            Script = properties.Script
        };
}
