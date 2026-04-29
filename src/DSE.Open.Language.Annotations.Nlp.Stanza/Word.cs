// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Represents a syntactic word within a Stanza-processed <see cref="Token"/>.
/// </summary>
public class Word : StanzaObject
{
    internal Word(PyObject pyWord, IStanzaService stanza) : base(pyWord, stanza)
    {
        Index = pyWord.GetAttr("id").AsNumber<int>();
        Head = pyWord.GetAttr("head").AsNullableNumber<int>();
        Text = pyWord.GetAttr("text").As<string>();
        Lemma = pyWord.GetAttr("lemma").AsNullable<string>();
        Pos = pyWord.GetAttr("upos").As<string>();
        AltPos = pyWord.GetAttr("xpos").AsNullable<string>();
        Features = pyWord.GetAttr("feats").AsNullable<string>();
        Relation = pyWord.GetAttr("deprel").As<string>();
        Attributes = pyWord.GetAttr("misc").AsNullable<string>();
    }

    /// <summary>
    /// The index of this word in the sentence, 1-based (index 0 is reserved for an
    /// artificial symbol that represents the root of the syntactic tree).
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the index of the syntactic head of this word, or <see langword="null"/> if not set.
    /// </summary>
    public int? Head { get; }

    /// <summary>
    /// The text of this word. Example: ‘The’
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// The lemma of this word.
    /// </summary>
    public string? Lemma { get; }

    /// <summary>
    /// Gets the universal part-of-speech tag (UPOS) of this word.
    /// </summary>
    public string Pos { get; }

    /// <summary>
    /// Gets the language-specific part-of-speech tag (XPOS) of this word, if any.
    /// </summary>
    public string? AltPos { get; }

    /// <summary>
    /// Gets the morphological features of this word, if any.
    /// </summary>
    public string? Features { get; }

    /// <summary>
    /// Gets the universal dependency relation of this word to its head.
    /// </summary>
    public string Relation { get; }

    /// <summary>
    /// Gets miscellaneous attributes associated with this word, if any.
    /// </summary>
    public string? Attributes { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Index} {Pos} {AltPos} {Text}";
    }
}
