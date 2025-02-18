using LofiBeats.Cli;
using LofiBeats.Cli.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests;

[Collection("AI Generated Tests")]
public class StartupTests : IDisposable
{
    private static readonly Action<ILogger, Exception?> LogTestMessage =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(LogTestMessage)),
            "Test log message");

    private readonly string _originalConfigPath;
    private readonly string _testConfigPath;

    public StartupTests()
    {
        // Setup test configuration
        _originalConfigPath = Path.Combine(AppContext.BaseDirectory, "cli.appsettings.json");
        _testConfigPath = Path.Combine(AppContext.BaseDirectory, "test.cli.appsettings.json");

        // Create a symbolic link to our test config with the expected name
        if (File.Exists(_testConfigPath))
        {
            File.Copy(_testConfigPath, _originalConfigPath, true);
        }
    }

    public void Dispose()
    {
        // Cleanup test configuration
        if (File.Exists(_originalConfigPath))
        {
            File.Delete(_originalConfigPath);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateHostBuilder_ConfiguresExpectedServices()
    {
        // Arrange
        var args = new[] { "--test-arg", "value" };
        
        // Act
        var hostBuilder = Startup.CreateHostBuilder(args);
        var host = hostBuilder.Build();
        
        // Assert
        var services = host.Services;
        
        // Verify required services are registered
        Assert.NotNull(services.GetService<CommandLineInterface>());
        Assert.NotNull(services.GetService<IConfiguration>());
        Assert.NotNull(services.GetService<ILoggerFactory>());
        
        // Verify configuration sources
        var config = services.GetRequiredService<IConfiguration>();
        var configRoot = config as IConfigurationRoot;
        Assert.NotNull(configRoot);
        
        // Verify expected configuration providers are present
        var providers = configRoot.Providers.ToList();
        Assert.Contains(providers, p => p.GetType().Name.Contains("JsonConfigurationProvider"));
        Assert.Contains(providers, p => p.GetType().Name.Contains("EnvironmentVariablesConfigurationProvider"));
        Assert.Contains(providers, p => p.GetType().Name.Contains("CommandLineConfigurationProvider"));
    }
    
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateHostBuilder_ConfiguresLogging()
    {
        // Arrange
        var args = Array.Empty<string>();
        
        // Act
        var hostBuilder = Startup.CreateHostBuilder(args);
        var host = hostBuilder.Build();
        
        // Assert
        var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
        Assert.NotNull(loggerFactory);
        
        // Create a logger and verify it works
        var logger = loggerFactory.CreateLogger("TestLogger");
        Assert.NotNull(logger);
        
        // Verify we can log without exceptions
        LogTestMessage(logger, null);
    }
    
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateHostBuilder_HandlesCustomLogLevel()
    {
        // Arrange
        var args = new[] { "--Logging:LogLevel:Default", "Debug" };
        
        // Act
        var hostBuilder = Startup.CreateHostBuilder(args);
        var host = hostBuilder.Build();
        
        // Assert
        var config = host.Services.GetRequiredService<IConfiguration>();
        var logLevel = config["Logging:LogLevel:Default"];
        Assert.Equal("Debug", logLevel);
    }
} 