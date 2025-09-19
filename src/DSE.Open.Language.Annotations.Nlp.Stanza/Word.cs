// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Word : StanzaObject
{
    [RequiresDynamicCode("Calls DSE.Open.Interop.Python.PyObjectExtensions.AsNullable<T>()")]
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

    public int? Head { get; }

    /// <summary>
    /// The text of this word. Example: ‘The’
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// The lemma of this word.
    /// </summary>
    public string? Lemma { get; }

    public string Pos { get; }

    public string? AltPos { get; }

    public string? Features { get; }

    public string Relation { get; }

    public string? Attributes { get; }

    public override string ToString()
    {
        return $"{Index} {Pos} {AltPos} {Text}";
    }
}
