// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Document : StanzaObject
{
    internal Document(PyObject pyDocument, IStanzaService stanza) : base(pyDocument, stanza)
    {
        Text = pyDocument.GetAttr("text").As<string>();
        Sentences = [.. pyDocument.GetAttr("sentences").As<IReadOnlyList<PyObject>>().Select(s => new Sentence(s, stanza))];
    }

    public string Text { get; }

    public IReadOnlyList<Sentence> Sentences { get; }
}
