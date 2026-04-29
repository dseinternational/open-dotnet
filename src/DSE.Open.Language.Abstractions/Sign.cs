// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Language.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A sign is anything that communicates a meaning that is not the sign
/// itself to the interpreter of the sign.
/// </summary>
[JsonConverter(typeof(JsonStringSignConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct Sign
    : ISpanFormattable,
      ISpanParsable<Sign>
{
    /// <summary>
    /// The maximum number of characters used to serialize a <see cref="Sign"/>
    /// value (modality, separator, word).
    /// </summary>
    public static readonly int MaxSerializedCharLength
        = SignModality.MaxSerializedCharLength + 1 + WordText.MaxSerializedCharLength;

    /// <summary>
    /// An empty <see cref="Sign"/>.
    /// </summary>
    public static readonly Sign Empty;

    /// <summary>
    /// Initializes a new <see cref="Sign"/> with the specified modality and word.
    /// </summary>
    /// <param name="modality">The modality of the sign.</param>
    /// <param name="word">The word that identifies the sign.</param>
    public Sign(SignModality modality, WordText word)
    {
        Modality = modality;
        Word = word;
    }

    /// <summary>
    /// The modality of the sign.
    /// </summary>
    public SignModality Modality { get; init; }

    /// <summary>
    /// A <see cref="Word"/> that identifies the sign.
    /// </summary>
    public WordText Word { get; init; }

    /// <summary>
    /// Parses a <see cref="Sign"/> from the given character span.
    /// </summary>
    /// <param name="s">The characters to parse.</param>
    /// <returns>The parsed <see cref="Sign"/>.</returns>
    /// <exception cref="FormatException">The input is not in the expected format.</exception>
    public static Sign Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static Sign Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            ThrowHelper.ThrowFormatException($"Invalid sign format: '{s}'");
        }

        return result;
    }

    /// <summary>
    /// Parses a <see cref="Sign"/> from the given string.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The parsed <see cref="Sign"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException">The input is not in the expected format.</exception>
    public static Sign Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)"/>
    public static Sign Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (!TryParse(s, provider, out var result))
        {
            ThrowHelper.ThrowFormatException($"Invalid sign format: '{s}'");
        }

        return result;
    }

    /// <summary>
    /// Attempts to parse a <see cref="Sign"/> from the given character span.
    /// </summary>
    /// <param name="s">The characters to parse.</param>
    /// <param name="result">When this method returns, contains the parsed
    /// <see cref="Sign"/> if successful; otherwise the default value.</param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Sign result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)"/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Sign result)
    {
        var i = s.IndexOf(':');

        if (i > 0)
        {
            if (SignModality.TryParse(s[..i], provider, out var modality))
            {
                if (WordText.TryParse(s[(i + 1)..], provider, out var word))
                {
                    result = new(modality, word);
                    return true;
                }
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse a <see cref="Sign"/> from the given string.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">When this method returns, contains the parsed
    /// <see cref="Sign"/> if successful; otherwise the default value.</param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out Sign result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Sign result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[SignModality.MaxSerializedCharLength + 2 + WordText.MaxSerializedCharLength];

        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
        {
            return buffer[..charsWritten].ToString();
        }

        Expect.Unreachable();
        return string.Empty; // unreachable
    }

    /// <summary>
    /// Attempts to format the value of this <see cref="Sign"/> into the
    /// provided character span.
    /// </summary>
    /// <param name="destination">The span to write the formatted characters to.</param>
    /// <param name="charsWritten">When this method returns, contains the number
    /// of characters written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the formatting was successful; otherwise <see langword="false"/>.</returns>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, null, null);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (Modality.TryFormat(destination, out charsWritten, format, provider))
        {
            if (charsWritten >= destination.Length)
            {
                charsWritten = 0;
                return false;
            }

            destination[charsWritten++] = ':';

            if (Word.TryFormat(destination[charsWritten..], out var wordCharsWritten, format, provider))
            {
                charsWritten += wordCharsWritten;
                return true;
            }
        }

        charsWritten = 0;
        return false;
    }
}
