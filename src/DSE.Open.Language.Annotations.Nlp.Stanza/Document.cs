// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// A Document object holds the annotation of an entire document, and is automatically generated
/// when a string is annotated by the Pipeline. It contains a collection of Sentences and
/// entities (which are represented as Spans).
/// </summary>
public class Document
{
    private readonly dynamic _doc;

    private readonly Lazy<string?> _lang;
    private readonly Lazy<Collection<Sentence>> _sentences;
    private readonly Lazy<Collection<Span>> _entities;

    internal Document(dynamic doc)
    {
        Guard.IsNotNull(doc);

        _doc = doc;

        _lang = new Lazy<string?>(() => PyConverter.GetStringOrNull(doc.lang));

        _sentences = new Lazy<Collection<Sentence>>(() =>
        {
            return PyConverter.GetList<Sentence>(_doc.sentences, (PyWrapperFactory<Sentence>)del);

            Sentence del(dynamic s)
            {
                return new(this, s);
            }
        });

        _entities = new Lazy<Collection<Span>>(() =>
        {
            return PyConverter.GetList<Span>(_doc.entities, (PyWrapperFactory<Span>)del);

            Span del(dynamic s)
            {
                return new(this, s);
            }
        });
    }

    public string? Lang => _lang.Value;

    public IReadOnlyList<Sentence> Sentences => _sentences.Value;

    public IReadOnlyList<Span> Entities => _entities.Value;
}
