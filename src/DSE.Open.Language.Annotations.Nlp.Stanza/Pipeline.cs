// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Python.Runtime;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Pipeline : IDisposable
{
    private readonly dynamic _nlp;
    private bool _disposed;

    internal Pipeline(dynamic nlp)
    {
        Guard.IsNotNull(nlp);

        _nlp = nlp;
    }

    public Task<Document> ProcessTextAsync(
        IEnumerable<string> sentences,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(sentences);

        var text = string.Join("\n\n", sentences);
        return ProcessTextAsync(text, cancellationToken);
    }

    public Task<Document> ProcessTextAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        return Task.Run(() =>
        {
            try
            {
                using (Py.GIL())
                {
                    var doc = _nlp(text);

                    if (doc is null)
                    {
                        throw new InvalidOperationException("Failed to create document.");
                    }

                    return new Document(doc);
                }
            }
            catch (Exception e)
            {
                throw new StanzaException("Failed to process text.", e);
            }
        }
        , cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                using (Py.GIL())
                {
                    if (_nlp is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
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
