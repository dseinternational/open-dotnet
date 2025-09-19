// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public sealed class StanzaServiceFixture : IDisposable
{
    private readonly IHost _host;
    private readonly IPythonEnvironment _pythonEnvironment;
    private readonly StanzaService _stanzaEnvironment;

    public StanzaServiceFixture()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                _ = services.AddLogging(config => config.AddDebug().AddConsole());

                var venv = new DirectoryInfo(Path.Join(Environment.CurrentDirectory,
                    "..", "..", "..", "..", "..", ".venv"));

                if (!venv.Exists)
                {
                    throw new InvalidOperationException($"Virtual environment not found at {venv}");
                }

                _ = services
                    .WithPython()
                    .WithVirtualEnvironment(venv.FullName)
                    .FromEnvironmentVariable("PYTHON3_HOME", "3.13");
            });

        _host = builder.Build();

        _pythonEnvironment = _host.Services.GetRequiredService<IPythonEnvironment>();

        _stanzaEnvironment = new StanzaService(_pythonEnvironment);

        PipelineEnglish = _stanzaEnvironment.CreatePipeline("en");
    }

    public ILoggerFactory LoggerFactory => _host.Services.GetRequiredService<ILoggerFactory>();

    public StanzaService Stanza => _stanzaEnvironment;

    public Pipeline PipelineEnglish { get; }

    public void Dispose()
    {
        _stanzaEnvironment?.Dispose();
        _pythonEnvironment?.Dispose();
        _host?.Dispose();
    }
}
