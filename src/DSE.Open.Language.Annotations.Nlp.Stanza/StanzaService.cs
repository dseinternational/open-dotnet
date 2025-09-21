// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Provides access to <see href="https://github.com/stanfordnlp/stanza">Stanza</see> NLP processing pipeline.
/// </summary>
public sealed class StanzaService : IDisposable
{
    private readonly IPythonEnvironment _pythonEnvironment;
    private readonly IStanzaService _stanza;

    public StanzaService(IPythonEnvironment pythonEnvironment) : this(pythonEnvironment, NullLogger<StanzaService>.Instance)
    {
    }

    public StanzaService(IPythonEnvironment pythonEnvironment, ILogger<StanzaService> logger)
    {
        ArgumentNullException.ThrowIfNull(pythonEnvironment);
        ArgumentNullException.ThrowIfNull(logger);

        _pythonEnvironment = pythonEnvironment;
        _stanza = _pythonEnvironment.StanzaService();
        Logger = logger;
    }

    private ILogger Logger { get; }

    public void Download(string lang = "en")
    {
        _ = _stanza.Download(lang);

        Logger.LogInformation("Downloaded Stanza model '{Language}'.", lang);
    }

    public Pipeline CreatePipeline(string lang = "en")
    {
        return new Pipeline(_stanza.CreatePipeline(lang), _stanza);
    }

    public void Dispose()
    {
        _stanza.Dispose();
    }
}
