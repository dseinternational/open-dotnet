// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Language.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A sign is anything that communicates a meaning that is not the sign
/// itself to the interpreter of the sign.
/// </summary>
[JsonConverter(typeof(JsonStringSignConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct Sign : ISpanFormattable, ISpanParsable<Sign>
{
    public static readonly int MaxSerializedCharLength
        = SignModality.MaxSerializedCharLength + 1 + Word.MaxSerializedCharLength;

    public static readonly Sign Empty;

    public Sign(SignModality modality, Word word)
    {
        Modality = modality;
        Word = word;
    }

    /// <summary>
    /// The modality of the sign.
    /// </summary>
    public SignModality Modality { get; init; }

    /// <summary>
    /// A <see cref="Word"/> that identifies the meaning of the sign.
    /// </summary>
    public Word Word { get; init; }

    public static Sign Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static Sign Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            ThrowHelper.ThrowFormatException("Invalid sign format");
        }

        return result;
    }

    public static Sign Parse(string s) => Parse(s, null);

    public static Sign Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);

        if (!TryParse(s, provider, out var result))
        {
            ThrowHelper.ThrowFormatException("Invalid sign format");
        }

        return result;
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out Sign result)
        => TryParse(s, null, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Sign result)
    {
        var i = s.IndexOf(':');

        if (i > 0)
        {
            if (SignModality.TryParse(s[..i], provider, out var modality))
            {
                if (Word.TryParse(s[(i + 1)..], provider, out var word))
                {
                    result = new Sign(modality, word);
                    return true;
                }
            }
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        [MaybeNullWhen(false)] out Sign result)
        => TryParse(s, null, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Sign result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[SignModality.MaxSerializedCharLength + 2 + Word.MaxSerializedCharLength];

        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
        {
            return buffer[..charsWritten].ToString();
        }

        ThrowHelper.ThrowInvalidOperationException("Should be unreachable");
        return string.Empty; // unreachable
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
        => TryFormat(destination, out charsWritten, null, null);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (Modality.TryFormat(destination, out charsWritten, format, provider))
        {
            destination[charsWritten++] = ':';

            if (Word.TryFormat(destination[charsWritten..], out var wordCharsWritten, format, provider))
            {
                charsWritten += wordCharsWritten;
                return true;
            }
        }
        return false;
    }
}
