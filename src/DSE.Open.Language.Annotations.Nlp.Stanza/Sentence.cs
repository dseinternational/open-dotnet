// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Sentence : StanzaObject
{
    [RequiresDynamicCode("Calls DSE.Open.Interop.Python.PyObjectExtensions.AsNullable<T>()")]
    internal Sentence(PyObject pySentence, IStanzaService stanza) : base(pySentence, stanza)
    {
        Id = pySentence.GetAttr("sent_id").As<string?>();
        Text = pySentence.GetAttr("text").As<string>();
        Tokens = [.. pySentence.GetAttr("tokens").As<IReadOnlyList<PyObject>>().Select(t => new Token(t, stanza))];
        Comments = [.. pySentence.GetAttr("comments").As<IReadOnlyList<string>>()];
    }

    public string? Id { get; }

    public string Text { get; }

    public IReadOnlyList<Token> Tokens { get; }

    public IReadOnlyList<string> Comments { get; }
}
