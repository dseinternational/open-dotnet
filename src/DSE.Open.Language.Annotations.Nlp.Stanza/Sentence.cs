// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Sentence : StanzaObject
{
    internal Sentence(PyObject pySentence, IStanzaService stanza) : base(pySentence, stanza)
    {
        Text = pySentence.GetAttr("text").As<string>();
        Id = pySentence.GetAttr("sent_id").As<string?>();
        Tokens = [.. pySentence.GetAttr("tokens").As<IReadOnlyList<PyObject>>().Select(t => new Token(t, stanza))];
    }

    public string Text { get; }

    public string? Id { get; }

    public IReadOnlyList<Token> Tokens { get; }
}
