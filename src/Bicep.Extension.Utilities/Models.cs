using System.Text.Json.Serialization;
using Azure.Bicep.Types.Concrete;
using Bicep.Local.Extension.Types.Attributes;

namespace Bicep.Extension.Utilities;

public class WaitResourceIdentifiers
{
}

[ResourceType("Wait")]
public class WaitResource : WaitResourceIdentifiers
{
    [TypeProperty("The wait duration in milliseconds", ObjectTypePropertyFlags.Required)]
    public required int DurationMs { get; set; }
}

public enum ScriptType
{
    Bash,
    PowerShell,
}

public class ScriptResourceIdentifiers
{
    [TypeProperty("The script type", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ScriptType? Type { get; set; }

    [TypeProperty("The script content", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Script { get; set; }

}

[ResourceType("Script")]
public class ScriptResource : ScriptResourceIdentifiers
{
    [TypeProperty("The exit code", ObjectTypePropertyFlags.ReadOnly)]
    public int ExitCode { get; set; }

    [TypeProperty("The stdout from calling the script", ObjectTypePropertyFlags.ReadOnly)]
    public string? StdOut { get; set; }

    [TypeProperty("The stderr from calling the script", ObjectTypePropertyFlags.ReadOnly)]
    public string? StdErr { get; set; }
}

public class CommandResourceIdentifiers
{
    [TypeProperty("The command", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Command { get; set; }
}

[ResourceType("Command")]
public class CommandResource : CommandResourceIdentifiers
{
    [TypeProperty("The exit code", ObjectTypePropertyFlags.ReadOnly)]
    public int ExitCode { get; set; }

    [TypeProperty("The stdout from calling the command", ObjectTypePropertyFlags.ReadOnly)]
    public string? StdOut { get; set; }

    [TypeProperty("The stderr from calling the command", ObjectTypePropertyFlags.ReadOnly)]
    public string? StdErr { get; set; }
}

public class AssertResourceIdentifiers
{
    [TypeProperty("The assertion name", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }
}

[ResourceType("Assert")]
public class AssertResource : AssertResourceIdentifiers
{
    [TypeProperty("The assertion condition", ObjectTypePropertyFlags.Required)]
    public required bool Condition { get; set; }
}