// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Speech.Serialization;

namespace DSE.Open.Speech;

/// <summary>
/// A unit of phonetic transcription using the International Phonetic Alphabet.
/// </summary>
[JsonConverter(typeof(JsonStringTranscriptionConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly struct Transcription
    : IEquatable<Transcription>,
      ISpanFormattable,
      ISpanParsable<Transcription>
{
    private readonly string _value;
    private readonly bool _initialized;

    public static int MaxLength => 64; // TODO: confirm

    public static readonly Transcription Empty;

    public const char LeftSquareBracket = '[';
    public const char RightSquareBracket = ']';

    public const char Slash = '/';

    public const char LeftBrace = '{';
    public const char RightBrace = '}';

    public Transcription(char value)
        : this(value.ToString())
    {
    }

    public Transcription(string value)
        : this(value, false)
    {
    }

    public Transcription(ReadOnlySpan<char> value)
        : this(value, false)
    {
    }

    private Transcription(ReadOnlySpan<char> value, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureValidValue(value);
        }

        _value = TranscriptionStringPool.Shared.GetOrAdd(value);
        _initialized = true;
    }

    // TODO: not sure about this - we need to consider use-cases further

    private static class TranscriptionStringPool
    {
        public static readonly StringPool Shared = new(512);
    }

    private Transcription(string value, bool skipValidation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!skipValidation)
        {
            EnsureValidValue(value);
        }

        _value = string.IsInterned(value) ?? TranscriptionStringPool.Shared.GetOrAdd(value);
        _initialized = true;
    }

    public static Transcription Phonetic(ReadOnlySpan<char> value)
    {
        return new($"{LeftSquareBracket}{value}{RightSquareBracket}");
    }

    public static Transcription Phonemic(ReadOnlySpan<char> value)
    {
        return new($"{Slash}{value}{Slash}");
    }

    public static Transcription Prosodic(ReadOnlySpan<char> value)
    {
        return new($"{LeftBrace}{value}{RightBrace}");
    }

    public bool IsPhoneticNotation => _value[0] == LeftSquareBracket;

    public bool IsPhonemicNotation => _value[0] == Slash;

    public bool IsProsodicNotation => _value[0] == LeftBrace;

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        if (value.Length <= 2 || value.Length > MaxLength)
        {
            return false;
        }

        var contents = value[1..^1];
        var first = value[0];
        var last = value[^1];

        var valid = ((first == LeftSquareBracket && last == RightSquareBracket)
            || (first == Slash && last == Slash)
            || (first == LeftBrace && last == RightBrace))
            && SpeechSound.AreAllIpaChars(contents);

        return valid;
    }

    private static void EnsureValidValue(ReadOnlySpan<char> value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
            return; // Unreachable
        }
    }

    public int Length => _value.Length;

    public override bool Equals(object? obj)
    {
        return obj is Transcription ph && Equals(ph);
    }

    public bool Equals(Transcription other)
    {
        return string.Equals(_value, other._value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(_value, StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return _value;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var span = _value.AsSpan();

        if (span.TryCopyTo(destination))
        {
            charsWritten = span.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value is null
            ? string.Empty
            : _value;
    }

    public static Transcription Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Transcription>($"Could not parse {nameof(Transcription)}");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Transcription result)
    {
        s = s.Trim();

        if (s.Length < 3
            || s.Length > MaxLength
            || !((s[0] == LeftSquareBracket && s[^1] == RightSquareBracket)
                || (s[0] == Slash && s[^1] == Slash)
                || (s[0] == LeftBrace && s[^1] == RightBrace)))
        {
            result = default;
            return false;
        }

        if (SpeechSound.IsValidValue(s[1..^1]))
        {
            result = new Transcription(s, true);
            return true;
        }

        result = default;
        return false;
    }

    public static Transcription Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Transcription result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool operator ==(Transcription left, Transcription right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Transcription left, Transcription right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static Transcription operator +(Transcription left, Transcription right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new Transcription(left._value + right._value);
    }
}
