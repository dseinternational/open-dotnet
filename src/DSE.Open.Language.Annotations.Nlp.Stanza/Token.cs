// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Interop.Python;
using Python.Runtime;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Token : IPyObjectWrapper<Token>
{
    private readonly dynamic _token;

    private Tuple<int, int?>? _id;
    private readonly Lazy<string> _text;
    private readonly Lazy<string?> _misc;
    private readonly Lazy<int> _start;
    private readonly Lazy<int> _end;
    private readonly Lazy<string?> _ner;
    private readonly Lazy<Collection<Word>> _words;

    internal Token(dynamic token)
    {
        _token = token;

        _text = new(() => PyConverter.GetString(_token.text));
        _misc = new(() => PyConverter.GetStringOrNull(_token.misc));
        _start = new(() => PyConverter.GetInt32(_token.start_char));
        _end = new(() => PyConverter.GetInt32(_token.end_char));
        _ner = new(() => PyConverter.GetStringOrNull(_token.ner));

        _words = new(() =>
        {
            return PyConverter.GetList<Word>(_token.words, (PyWrapperFactory<Word>)del);

            Word del(dynamic w)
            {
                return new(this, w);
            }
        });
    }

    public Tuple<int, int?> Id
    {
        get
        {
            if (_id is null)
            {
                using (Py.GIL())
                {
                    var pyObj = (PyObject)_token.id;

                    var type = pyObj.GetPythonType();

                    using var intPy = new PyTuple(_token.id);

                    var length = intPy.Length();

                    if (length > 0)
                    {
                        using var i0 = new PyInt(intPy[0]);

                        if (length > 1)
                        {
                            using var i1 = new PyInt(intPy[1]);

                            _id = Tuple.Create<int, int?>(i0.ToInt32(), i1.ToInt32());
                        }
                        else
                        {
                            _id = Tuple.Create<int, int?>(i0.ToInt32(), default);
                        }
                    }
                    else
                    {
                        _id = Tuple.Create<int, int?>(-1, default);
                    }
                }
            }

            return _id;
        }
    }

    public string Text => _text.Value;

    public string? Attributes => _misc.Value;

    public int Start => _start.Value;

    public int End => _end.Value;

    public string? Ner => _ner.Value;

    public IReadOnlyList<Word> Words => _words.Value;

    public static Token FromPyObject(PyObject pyObj)
    {
        return new(pyObj);
    }
}
