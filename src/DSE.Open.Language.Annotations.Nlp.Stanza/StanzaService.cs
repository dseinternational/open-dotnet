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

    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaService"/> class with no logging.
    /// </summary>
    /// <param name="pythonEnvironment">The Python environment used to host Stanza.</param>
    public StanzaService(IPythonEnvironment pythonEnvironment) : this(pythonEnvironment, NullLogger<StanzaService>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaService"/> class.
    /// </summary>
    /// <param name="pythonEnvironment">The Python environment used to host Stanza.</param>
    /// <param name="logger">The logger used to log diagnostic information.</param>
    public StanzaService(IPythonEnvironment pythonEnvironment, ILogger<StanzaService> logger)
    {
        ArgumentNullException.ThrowIfNull(pythonEnvironment);
        ArgumentNullException.ThrowIfNull(logger);

        _pythonEnvironment = pythonEnvironment;
        _stanza = _pythonEnvironment.StanzaService();
        Logger = logger;
    }

    private ILogger Logger { get; }

    /// <summary>
    /// Downloads the Stanza model resources for the specified language.
    /// </summary>
    /// <param name="lang">The language code identifying the model to download.</param>
    public void Download(string lang = "en")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lang);

        _ = _stanza.Download(lang);

#pragma warning disable CA1873 // Avoid potentially expensive logging
        Logger.LogInformation("Downloaded Stanza model '{Language}'.", lang);
#pragma warning restore CA1873 // Avoid potentially expensive logging
    }

    /// <summary>
    /// Creates a new Stanza <see cref="Pipeline"/> for the specified language.
    /// </summary>
    /// <param name="lang">The language code identifying the pipeline to create.</param>
    public Pipeline CreatePipeline(string lang = "en")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lang);

        return new Pipeline(_stanza.CreatePipeline(lang), _stanza);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stanza.Dispose();
    }
}
