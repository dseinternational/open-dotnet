// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Represents a document processed by a Stanza pipeline.
/// </summary>
public class Document : StanzaObject
{
    internal Document(PyObject pyDocument, IStanzaService stanza) : base(pyDocument, stanza)
    {
        Text = pyDocument.GetAttr("text").As<string>();
        Sentences = [.. pyDocument.GetAttr("sentences").As<IReadOnlyList<PyObject>>().Select(s => new Sentence(s, stanza))];
    }

    /// <summary>
    /// Gets the text of the document.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the sentences contained in the document.
    /// </summary>
    public IReadOnlyList<Sentence> Sentences { get; }
}
