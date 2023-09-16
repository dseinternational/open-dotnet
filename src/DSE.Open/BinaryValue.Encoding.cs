// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text;

namespace DSE.Open;

public readonly partial record struct BinaryValue
{
    /// <summary>
    /// Formats the value into the provided buffer as Base64.
    /// </summary>
    /// <param name="destination">The buffer to write the value into.</param>
    /// <param name="charsWritten">The number of characters written to the buffer.</param>
    /// <returns><c>true</c> if the value was successfully written to the buffer; otherwise, <c>false</c>.</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, null, null);

    /// <inheritdoc />
    /// <remarks>
    /// <para>
    /// The format string is a single character that indicates how to format the value.
    /// <list>
    ///     <item><c>null</c> or <c>Empty</c> - Base64</item>
    ///     <item><c>'B'</c> - Base64</item>
    ///     <item><c>'b'</c> - Base62</item>
    ///     <item><c>'x'</c> - Hexadecimal (lowercase)</item>
    ///     <item><c>'X'</c> - Hexadecimal (uppercase)</item>
    ///     <item>Any other character will throw a <see cref="FormatException"/>.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.Length == 0)
        {
            charsWritten = 0;
            return true;
        }

        if (format.Length > 1)
        {
            ThrowHelper.ThrowFormatException($"Invalid format string: '{format}'.");
            charsWritten = 0; // unreachable
            return false;
        }

        if (format.Length == 0 || format[0] == 'B')
        {
            return Convert.TryToBase64Chars(_value.Span, destination, out charsWritten);
        }

        switch (format[0])
        {
            case 'b':
                return Base62Converter.TryEncodeToBase62(_value.Span, destination, out charsWritten);
            case 'x':
                return HexConverter.TryEncodeToHexLower(_value.Span, destination, out charsWritten);
            case 'X':
                return HexConverter.TryEncodeToHexUpper(_value.Span, destination, out charsWritten);
            default:
                charsWritten = 0;
                return ThrowHelper.ThrowFormatException<bool>($"Invalid format string: '{format}'.");
        }
    }

    internal static int GetRequiredBufferSize(int inputLength, BinaryStringEncoding encoding) => encoding switch
    {
        BinaryStringEncoding.Base64 or BinaryStringEncoding.Base62 => (int)((uint)inputLength + 2) / 3 * 4,
        BinaryStringEncoding.HexLower or BinaryStringEncoding.HexUpper => (int)(uint)inputLength * 2,
        _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null)
    };

    /// <summary>
    /// Returns a Base64 string representation of the value.
    /// </summary>
    public override string ToString() => ToString(BinaryStringEncoding.Base64);

    /// <inheritdoc />
    /// <remarks>
    /// <para>
    /// The format string is a single character that indicates how to format the value.
    /// <list>
    ///     <item><c>null</c> or <c>Empty</c> - Base64</item>
    ///     <item><c>'B'</c> - Base64</item>
    ///     <item><c>'b'</c> - Base62</item>
    ///     <item><c>'x'</c> - Hexadecimal (lowercase)</item>
    ///     <item><c>'X'</c> - Hexadecimal (uppercase)</item>
    ///     <item>Any other character will throw a <see cref="FormatException"/>.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.Length == 0)
        {
            bytesWritten = 0;
            return true;
        }

        if (format.Length > 1)
        {
            ThrowHelper.ThrowFormatException($"Invalid format string: '{format}'.");
            bytesWritten = 0; // unreachable
            return false;
        }

        if (format.Length == 0 || format[0] == 'B')
        {
            return Base64.EncodeToUtf8(_value.Span, utf8Destination, out _, out bytesWritten) == OperationStatus.Done;
        }

        switch (format[0])
        {
            case 'b':
                return Ascii.FromUtf16(Base62Converter.ToBase62String(_value.Span).AsSpan(), utf8Destination, out bytesWritten) == OperationStatus.Done;
            case 'x':
                return HexConverter.TryEncodeToHexLower(_value.Span, utf8Destination, out bytesWritten);
            case 'X':
                return HexConverter.TryEncodeToHexUpper(_value.Span, utf8Destination, out bytesWritten);
            default:
                bytesWritten = 0;
                return ThrowHelper.ThrowFormatException<bool>($"Invalid format string: '{format}'.");
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// <para>
    /// The format string is a single character that indicates how to format the value.
    /// <list>
    ///     <item><c>null</c> or <c>Empty</c> - Base64</item>
    ///     <item><c>'B'</c> - Base64</item>
    ///     <item><c>'b'</c> - Base62</item>
    ///     <item><c>'x'</c> - Hexadecimal (lowercase)</item>
    ///     <item><c>'X'</c> - Hexadecimal (uppercase)</item>
    ///     <item>Any other character will throw a <see cref="FormatException"/>.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "B";

        if (format.Length > 1)
        {
            ThrowHelper.ThrowFormatException($"Invalid format string: '{format}'.");
        }

        if (_value.Length == 0)
        {
            return string.Empty;
        }

        return format[0] switch
        {
            'B' => ToString(BinaryStringEncoding.Base64),
            'b' => ToString(BinaryStringEncoding.Base62),
            'x' => ToString(BinaryStringEncoding.HexLower),
            'X' => ToString(BinaryStringEncoding.HexUpper),
            _ => ThrowHelper.ThrowFormatException<string>($"Invalid format string: '{format}'."),
        };
    }

    public string ToString(BinaryStringEncoding format)
    {
        if (_value.Length == 0)
        {
            return string.Empty;
        }

        return format switch
        {
            BinaryStringEncoding.Base62 => Base62Converter.ToBase62String(_value.Span),
            BinaryStringEncoding.HexUpper => string.Create(_value.Length * 2, this, (span, value) =>
            {
                value.TryFormat(span, out var charsWritten, format: "X", provider: null);
                Debug.Assert(charsWritten == span.Length);
            }),
            BinaryStringEncoding.HexLower => string.Create(_value.Length * 2, this, (span, value) =>
            {
                value.TryFormat(span, out var charsWritten, format: "x", provider: null);
                Debug.Assert(charsWritten == span.Length);
            }),
            _ => Convert.ToBase64String(_value.Span)
        };
    }

    public string ToBase62EncodedString() => ToString(BinaryStringEncoding.Base62);

    public string ToBase64EncodedString() => ToString(BinaryStringEncoding.Base64);
}
