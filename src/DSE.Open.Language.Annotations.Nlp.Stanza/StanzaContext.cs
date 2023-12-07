// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Python.Runtime;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class StanzaContext : IDisposable
{
    private readonly PythonContext _pythonContext;
    private dynamic? _stanza;
    private bool _disposed;

    public StanzaContext(PythonContext pythonContext)
    {
        ArgumentNullException.ThrowIfNull(pythonContext);
        _pythonContext = pythonContext;
    }

    [MemberNotNullWhen(true, nameof(_stanza))]
    public bool IsInitialized => _stanza is not null;

    [MemberNotNull(nameof(_stanza))]
    private void EnsureInitialized()
    {
        if (!IsInitialized)
        {
            throw new InvalidOperationException("Must be initialized.");
        }
    }

    public void Initialize()
    {
        using (Py.GIL())
        {
            _stanza = Py.Import("stanza");
        }
    }

    public void DownloadModel(string model)
    {
        EnsureInitialized();

        using (Py.GIL())
        {
            _stanza.download("en", logging_level: "WARN");
        }
    }

    public Pipeline CreatePipeline(string model)
    {
        EnsureInitialized();

        using (Py.GIL())
        {
            var nlp = _stanza.Pipeline("en", logging_level: "WARN");

            if (nlp is null)
            {
                throw new InvalidOperationException("Failed to create pipeline.");
            }

            return new Pipeline(nlp);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _pythonContext.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
