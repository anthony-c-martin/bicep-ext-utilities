using Bicep.Extension.Utilities.Handlers;

namespace Bicep.Extension.Utilities.Tests;

[TestClass]
public sealed class UtilitiesHandlerTests
{
    [TestMethod]
    public async Task Preview_serializes_resource_properties_as_camel_case()
    {
        var response = await HandlerHarness.PreviewAsync(
            new AssertHandler(),
            "Assert",
            new { name = "healthy", condition = true });

        var properties = response.ResourceProperties();
        Assert.AreEqual("healthy", properties.GetProperty("name").GetString());
        Assert.IsTrue(properties.GetProperty("condition").GetBoolean());
        Assert.IsFalse(properties.TryGetProperty("Condition", out _));
    }

    [TestMethod]
    public async Task Assert_create_returns_properties_when_condition_is_true()
    {
        var response = await HandlerHarness.CreateOrUpdateAsync(
            new AssertHandler(),
            "Assert",
            new { name = "healthy", condition = true });

        Assert.AreEqual("healthy", response.ResourceProperties().GetProperty("name").GetString());
    }

    [TestMethod]
    public async Task Assert_create_returns_error_when_condition_is_false()
    {
        var response = await HandlerHarness.CreateOrUpdateAsync(
            new AssertHandler(),
            "Assert",
            new { name = "broken", condition = false });

        Assert.IsNotNull(response.ErrorData);
        StringAssert.Contains(response.ErrorData.Error.Message, "Assertion 'broken' failed!");
    }

    [TestMethod]
    public async Task Command_create_captures_standard_output_and_exit_code()
    {
        var response = await HandlerHarness.CreateOrUpdateAsync(
            new CommandHandler(),
            "Command",
            new { command = "echo hello" });

        var properties = response.ResourceProperties();
        Assert.AreEqual(0, properties.GetProperty("exitCode").GetInt32());
        Assert.AreEqual("hello", properties.GetProperty("stdOut").GetString()?.Trim());
        Assert.AreEqual(string.Empty, properties.GetProperty("stdErr").GetString());
    }

    [TestMethod]
    public async Task Bash_script_create_captures_output_and_exit_code()
    {
        var response = await HandlerHarness.CreateOrUpdateAsync(
            new ScriptHandler(),
            "Script",
            new { type = "Bash", script = "printf 'hello'" });

        var properties = response.ResourceProperties();
        Assert.AreEqual(0, properties.GetProperty("exitCode").GetInt32());
        Assert.AreEqual("hello", properties.GetProperty("stdOut").GetString());
    }

    [TestMethod]
    public async Task Wait_create_completes_after_requested_duration()
    {
        var response = await HandlerHarness.CreateOrUpdateAsync(
            new WaitHandler(),
            "Wait",
            new { durationMs = 1 });

        Assert.IsNotNull(response.Resource);
        Assert.IsNull(response.ErrorData);
    }
}
