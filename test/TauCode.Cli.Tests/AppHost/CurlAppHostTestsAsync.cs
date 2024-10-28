using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using TauCode.Cli.Tests.Common;
using TauCode.Cli.Tests.Common.Apps.Curl;
using TauCode.IO;

#pragma warning disable NUnit1032

namespace TauCode.Cli.Tests.AppHost;

[TestFixture]
public class CurlAppHostTestsAsync
{
    private StringWriterWithEncoding _output = null!;

    [SetUp]
    public void SetUp()
    {
        _output = new StringWriterWithEncoding(Encoding.UTF8);
    }

    [Test]
    public async Task Run_ValidInput_ProducesExpectedResult()
    {
        // Arrange
        using var appHost = new Cli.AppHost(new CurlApp())
        {
            Output = _output,
        };

        var args = "    https://google.com";

        // Act
        await appHost.RunAsync(args.AsMemory());

        // Assert
        var output = _output.ToString();
        var commandDto = JsonConvert.DeserializeObject<TestCommandDto>(output);

        Assert.That(commandDto!.ExecutorName, Is.Null);
        Assert.That(commandDto.Input, Is.EqualTo("https://google.com"));

        Assert.That(commandDto.Arguments, Has.Count.EqualTo(1));
        Assert.That(commandDto.Arguments, Does.ContainKey("url"));
        Assert.That(commandDto.Arguments["url"].Single(), Is.EqualTo("https://google.com/"));

        Assert.That(commandDto.KeyValues, Is.Empty);
        Assert.That(commandDto.Switches, Is.Empty);
    }
}