// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Span
{
    private readonly dynamic _span;
    private readonly Lazy<string> _text;
    private readonly Lazy<string?> _type;
    private readonly Lazy<int> _start;
    private readonly Lazy<int> _end;

    private readonly Lazy<Collection<Token>> _tokens;
    private readonly Lazy<List<Word>> _words;

    internal Span(Document doc, dynamic span)
    {
        Document = doc;

        _span = span;

        _text = new Lazy<string>(() => PyConverter.GetString(_span.text));
        _type = new Lazy<string?>(() => PyConverter.GetStringOrNull(_span.misc));
        _start = new Lazy<int>(() => PyConverter.GetInt32(_span.start_char));
        _end = new Lazy<int>(() => PyConverter.GetInt32(_span.end_char));

        _tokens = new Lazy<Collection<Token>>(() => PyConverter.GetList<Token>(_span.tokens));
        _words = new Lazy<List<Word>>(() => Tokens.SelectMany(t => t.Words).ToList());
    }

    public Document Document { get; }

    public string Text => _text.Value;

    public string? Type => _type.Value;

    public int Start => _start.Value;

    public int End => _end.Value;

    public IReadOnlyList<Token> Tokens => _tokens.Value;

    public IReadOnlyList<Word> Words => _words.Value;
}
