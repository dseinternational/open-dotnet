// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Interop.Python;
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

        using (Py.GIL())
        {
            _stanza = Py.Import("stanza");
        }

        if (_stanza is null)
        {
            throw new InvalidOperationException("Failed to import stanza.");
        }
    }

    [MemberNotNull(nameof(_stanza))]
    private void EnsureNotDisposed()
    {
        if (_disposed || _stanza is null)
        {
            ThrowHelper.ThrowObjectDisposedException(nameof(StanzaContext));
        }
    }

    public void DownloadModel(string model)
    {
        EnsureNotDisposed();

        using (Py.GIL())
        {
            _stanza.download(model, logging_level: "WARN");
        }
    }

    public Pipeline CreatePipeline(string model)
    {
        EnsureNotDisposed();

        using (Py.GIL())
        {
            var nlp = _stanza.Pipeline(model, logging_level: "WARN");

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
                _stanza = null;
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
