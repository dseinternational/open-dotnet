// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Represents a token within a Stanza-processed <see cref="Sentence"/>.
/// </summary>
public class Token : StanzaObject
{
    internal Token(PyObject pyToken, IStanzaService stanza) : base(pyToken, stanza)
    {
        Text = pyToken.GetAttr("text").As<string>();
        StartChar = pyToken.GetAttr("start_char").As<int>();
        EndChar = pyToken.GetAttr("end_char").As<int>();
        Words = [.. pyToken.GetAttr("words").As<IReadOnlyList<PyObject>>().Select(w => new Word(w, stanza))];
    }

    /// <summary>
    /// Gets the text of the token.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the character offset at which the token begins in the source text.
    /// </summary>
    public int StartChar { get; }

    /// <summary>
    /// Gets the character offset at which the token ends in the source text.
    /// </summary>
    public int EndChar { get; }

    /// <summary>
    /// Gets the words contained in the token.
    /// </summary>
    public IReadOnlyList<Word> Words { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Text;
    }
}
