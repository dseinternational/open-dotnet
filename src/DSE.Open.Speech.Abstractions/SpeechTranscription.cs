// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Speech.Serialization;

namespace DSE.Open.Speech;

/// <summary>
/// A unit of speech sound transcription using the International Phonetic Alphabet,
/// together with notation type.
/// </summary>
[JsonConverter(typeof(JsonStringSpeechTranscriptionConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct SpeechTranscription
    : IEquatable<SpeechTranscription>,
      ISpanFormattable,
      ISpanParsable<SpeechTranscription>,
      ISpanFormatableCharCountProvider
{
    private readonly SpeechSymbolSequence _transcription;
    private readonly TranscriptionNotation _notation;

    public static readonly SpeechTranscription Empty;

    public const char LeftSquareBracket = '[';
    public const char RightSquareBracket = ']';

    public const char Slash = '/';

    public const char LeftBrace = '{';
    public const char RightBrace = '}';

    public const char LeftParenthesis = '(';
    public const char RightParenthesis = ')';

    public const char LeftDoubleParenthesis = '⸨';
    public const char RightDoubleParenthesis = '⸩';

    private readonly record struct DelimiterDefinition(
        TranscriptionNotation Notation,
        char LeftDelimiter,
        char RightDelimiter);

    private static readonly DelimiterDefinition s_phoneticDelimiter =
        new(TranscriptionNotation.Phonetic, LeftSquareBracket, RightSquareBracket);

    private static readonly DelimiterDefinition s_phonemicDelimiter =
        new(TranscriptionNotation.Phonemic, Slash, Slash);

    private static readonly DelimiterDefinition s_prosodicDelimiter =
        new(TranscriptionNotation.Prosodic, LeftBrace, RightBrace);

    private static readonly DelimiterDefinition s_indistinguishableDelimiter =
        new(TranscriptionNotation.IndistinguishableOrSlientArticulation, LeftParenthesis, RightParenthesis);

    private static readonly DelimiterDefinition s_obscuredDelimiter =
        new(TranscriptionNotation.ObscuredSpeechOrObscuringNoise, LeftDoubleParenthesis, RightDoubleParenthesis);

    private static readonly DelimiterDefinition[] s_delimiters =
    [
        s_phoneticDelimiter,
        s_phonemicDelimiter,
        s_prosodicDelimiter,
        s_indistinguishableDelimiter,
        s_obscuredDelimiter,
    ];

    private static readonly FrozenDictionary<TranscriptionNotation, DelimiterDefinition> s_delimitersLookup =
        s_delimiters.ToFrozenDictionary(d => d.Notation, d => d);

    public SpeechTranscription(SpeechSymbol transcription, TranscriptionNotation notation)
    {
        _transcription = [transcription];
        _notation = notation;
    }

    public SpeechTranscription(SpeechSymbolSequence transcription, TranscriptionNotation notation)
    {
        _transcription = transcription;
        _notation = notation;
    }

    public TranscriptionNotation Notation => _notation;

    public SpeechSymbolSequence Contents => _transcription;

    public override string ToString()
    {
        return ToString(null, null);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var transcription = MemoryMarshal.Cast<SpeechSymbol, char>(_transcription.AsSpan());

        if (_notation == TranscriptionNotation.Undefined)
        {
            if (transcription.Length <= destination.Length)
            {
                transcription.CopyTo(destination);
                charsWritten = transcription.Length;
                return true;
            }
            else
            {
                charsWritten = 0;
                return false;
            }
        }

        var delimiters = s_delimitersLookup[_notation];
        var outputLength = transcription.Length + 2;

        if (outputLength <= destination.Length)
        {
            destination[0] = delimiters.LeftDelimiter;
            transcription.CopyTo(destination[1..^1]);
            destination[^1] = delimiters.RightDelimiter;
            charsWritten = outputLength;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var outputLength = _notation == TranscriptionNotation.Undefined
            ? _transcription.Length
            : _transcription.Length + 2;

        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(outputLength)
            ? stackalloc char[outputLength]
            : (rented = new char[outputLength]);

        try
        {
            if (TryFormat(buffer, out var charsWritten, format.AsSpan(), formatProvider))
            {
                return buffer[..charsWritten].ToString();
            }

            ThrowHelper.ThrowFormatException(
                $"Could not format {nameof(SpeechTranscription)} value: '{_transcription}'");
            return null!; // unreachable
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    public static SpeechTranscription Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<SpeechTranscription>($"Could not parse {nameof(SpeechTranscription)}");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SpeechTranscription result)
    {
        s = s.Trim();

        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        if (s.Length < 2)
        {
            if (!SpeechSymbol.IsStrictIpaSymbol(s[0]))
            {
                result = default;
                return false;
            }
        }

        var first = s[0];
        var last = s[^1];

        foreach (var d in s_delimiters)
        {
            if (first == d.LeftDelimiter && last == d.RightDelimiter)
            {
                var contents = s[1..^1];

                if (SpeechSymbolSequence.TryParse(contents, CultureInfo.InvariantCulture, out var transcription))
                {
                    result = new(transcription, d.Notation);
                    return true;
                }
            }
        }

        // not delimited

        if (SpeechSymbolSequence.TryParse(s, CultureInfo.InvariantCulture, out var transcription2))
        {
            result = new(transcription2, TranscriptionNotation.Undefined);
            return true;
        }

        result = default;
        return false;
    }

    public static SpeechTranscription Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out SpeechTranscription result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryGetNotation(ReadOnlySpan<char> value, out TranscriptionNotation notation)
    {
        if (value.IsEmpty)
        {
            notation = TranscriptionNotation.Undefined;
            return false;
        }

        if (value.Length < 2)
        {
            notation = TranscriptionNotation.Undefined;
            return false;
        }

        var first = value[0];
        var last = value[^1];

        foreach (var d in s_delimiters)
        {
            if (first == d.LeftDelimiter && last == d.RightDelimiter)
            {
                notation = d.Notation;
                return true;
            }
        }

        notation = TranscriptionNotation.Undefined;
        return false;
    }

    int ISpanFormatableCharCountProvider.GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _notation == TranscriptionNotation.Undefined
            ? _transcription.Length
            : _transcription.Length + 2;
    }

    int IFormatableCharCountProvider.GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _notation == TranscriptionNotation.Undefined
            ? _transcription.Length
            : _transcription.Length + 2;
    }
}
