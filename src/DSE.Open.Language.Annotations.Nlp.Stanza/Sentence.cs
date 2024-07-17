// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class Sentence
{
    private readonly dynamic _sentence;

    private readonly Lazy<int> _index;
    private readonly Lazy<string> _id;
    private readonly Lazy<string?> _doc_id;
    private readonly Lazy<string> _text;
    private readonly Lazy<IReadOnlyList<string>> _comments;
    private readonly Lazy<IReadOnlyList<Token>> _tokens;
    private List<Word>? _words;

    internal Sentence(Document document, dynamic sentence)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(sentence);

        Document = document;

        _sentence = sentence;

        _index = new(() => PyConverter.GetInt32(_sentence.index));
        _id = new(() => PyConverter.GetString(_sentence.sent_id));
        // v1.7.0 and above
        _doc_id = new(() => PyConverter.GetStringOrNull(_sentence.doc_id));
        _text = new(() => PyConverter.GetString(_sentence.text));

        _comments = new(() =>
        {
            return PyConverter.GetList<string>(_sentence.comments, (PyWrapperFactory<string>)del);

            static string del(dynamic s)
            {
                return s.As<string>();
            }
        });

        _tokens = new(() => PyConverter.GetList<Token>(_sentence.tokens));
    }

    public Document Document { get; }

    public int Index => _index.Value;

    public string Id => _id.Value;

    public string? DocId => _doc_id.Value;

    public string Text => _text.Value;

    public IReadOnlyList<string> Comments => _comments.Value;

    public IReadOnlyList<Token> Tokens => _tokens.Value;

    public IReadOnlyList<Word> Words => _words ??= Tokens.SelectMany(t => t.Words).ToList();
}
