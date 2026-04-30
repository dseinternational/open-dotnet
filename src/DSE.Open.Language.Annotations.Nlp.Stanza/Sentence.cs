// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Represents a sentence within a Stanza-processed <see cref="Document"/>.
/// </summary>
public class Sentence : StanzaObject
{
    internal Sentence(PyObject pySentence, IStanzaService stanza) : base(pySentence, stanza)
    {
        Id = pySentence.GetAttr("sent_id").As<string?>();
        Text = pySentence.GetAttr("text").As<string>();
        Tokens = [.. pySentence.GetAttr("tokens").As<IReadOnlyList<PyObject>>().Select(t => new Token(t, stanza))];
        Comments = [.. pySentence.GetAttr("comments").As<IReadOnlyList<string>>()];
    }

    /// <summary>
    /// Gets the sentence identifier, if one was assigned.
    /// </summary>
    public string? Id { get; }

    /// <summary>
    /// Gets the text of the sentence.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the tokens contained in the sentence.
    /// </summary>
    public IReadOnlyList<Token> Tokens { get; }

    /// <summary>
    /// Gets the comments associated with the sentence.
    /// </summary>
    public IReadOnlyList<string> Comments { get; }
}
