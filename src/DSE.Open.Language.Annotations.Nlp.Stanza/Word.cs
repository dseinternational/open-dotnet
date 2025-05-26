// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Word : StanzaObject
{
    internal Word(PyObject pyWord, IStanzaService stanza) : base(pyWord, stanza)
    {
        Id = pyWord.GetAttr("id").As<int>();
        Text = pyWord.GetAttr("text").As<string>();
        Lemma = pyWord.GetAttr("lemma").As<string?>();
    }

    /// <summary>
    /// The index of this word in the sentence, 1-based (index 0 is reserved for an
    /// artificial symbol that represents the root of the syntactic tree).
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// The text of this word. Example: ‘The’
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// The lemma of this word.
    /// </summary>
    public string? Lemma { get; }
}
