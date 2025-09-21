// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Token : StanzaObject
{
    internal Token(PyObject pyToken, IStanzaService stanza) : base(pyToken, stanza)
    {
        Text = pyToken.GetAttr("text").As<string>();
        StartChar = pyToken.GetAttr("start_char").As<int>();
        EndChar = pyToken.GetAttr("end_char").As<int>();
        Words = [.. pyToken.GetAttr("words").As<IReadOnlyList<PyObject>>().Select(w => new Word(w, stanza))];
    }

    public string Text { get; }

    public int StartChar { get; }

    public int EndChar { get; }

    public IReadOnlyList<Word> Words { get; }

    public override string ToString()
    {
        return Text;
    }
}
