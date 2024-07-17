// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Interop.Python;
using Python.Runtime;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// A Word object holds a syntactic word and all of its word-level annotations. In the
/// event of multi-word tokens (MWT), words are generated as a result of applying the
/// MWTProcessor, and are used in all downstream syntactic analyses such as tagging,
/// lemmatization, and parsing. If a Word is the result from an MWT expansion, its text
/// will usually not be found in the input raw text. Aside from multi-word tokens, Words
/// should be similar to the familiar “tokens” one would see elsewhere.
/// </summary>
public class Word
{
    private readonly dynamic _word;

    private readonly Lazy<int> _index;
    private readonly Lazy<string> _text;
    private readonly Lazy<string?> _lemma;
    private readonly Lazy<string> _upos;
    private readonly Lazy<string?> _xpos;
    private readonly Lazy<string?> _feats;
    private readonly Lazy<int> _head;
    private readonly Lazy<string> _deprel;
    private readonly Lazy<string?> _deps;
    private readonly Lazy<string?> _misc;
    private readonly Lazy<string> _prettyPrint;

    public Word(Token token, dynamic word)
    {
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(word);

        Token = token;

        _word = word;

        _index = new(() => PyConverter.GetInt32(_word.id));
        _text = new(() => PyConverter.GetString(_word.text));
        _lemma = new(() => PyConverter.GetStringOrNull(_word.lemma));
        _upos = new(() => PyConverter.GetString(_word.upos));
        _xpos = new(() => PyConverter.GetStringOrNull(_word.xpos));
        _feats = new(() => PyConverter.GetStringOrNull(_word.feats));
        _head = new(() => PyConverter.GetInt32(_word.head));
        _deprel = new(() => PyConverter.GetString(_word.deprel));
        _deps = new(() => PyConverter.GetStringOrNull(_word.deps));
        _misc = new(() => PyConverter.GetStringOrNull(_word.misc));

        _prettyPrint = new(() =>
        {
            using (Py.GIL())
            {
                return _word.pretty_print();
            }
        });
    }

    public Token Token { get; }

    /// <summary>
    /// The index of this word in the sentence, 1-based (index 0 is reserved for an
    /// artificial symbol that represents the root of the syntactic tree).
    /// </summary>
    public int Index => _index.Value;

    /// <summary>
    /// The text of this word. Example: ‘The’
    /// </summary>
    public string Text => _text.Value;

    /// <summary>
    /// The lemma of this word.
    /// </summary>
    public string? Lemma => _lemma.Value;

    /// <summary>
    /// The universal part-of-speech of this word. Example: ‘NOUN’.
    /// </summary>
    public string Pos => _upos.Value;

    public string? AltPos => _xpos.Value;

    public string? Features => _feats.Value;

    public int Head => _head.Value;

    public string Relation => _deprel.Value;

    public string? Dependencies => _deps.Value;

    public string? Attributes => _misc.Value;

    public override string ToString()
    {
        return _prettyPrint.Value;
    }
}
