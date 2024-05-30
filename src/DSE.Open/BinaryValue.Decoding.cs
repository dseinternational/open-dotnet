// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open;

public readonly partial record struct BinaryValue
{
    public static BinaryValue FromBase62EncodedString(string value)
    {
        return FromEncodedString(value, BinaryStringEncoding.Base62);
    }

    public static BinaryValue FromBase64EncodedString(string value)
    {
        return FromEncodedString(value, BinaryStringEncoding.Base64);
    }

    public static BinaryValue FromEncodedString(string value, BinaryStringEncoding encoding)
    {
        return encoding switch
        {
            BinaryStringEncoding.Base64 => new(Convert.FromBase64String(value), true),
            BinaryStringEncoding.HexLower or BinaryStringEncoding.HexUpper => new(Convert.FromHexString(value), true),
            BinaryStringEncoding.Base62 => new(Base62Converter.FromBase62(value), true),
            _ => ThrowHelper.ThrowFormatException<BinaryValue>("Invalid encoding provided."),
        };
    }

    /// <summary>
    /// Decodes a string into a <see cref="BinaryValue"/> using the provided <paramref name="encoding"/>.
    /// </summary>
    /// <param name="value">The string to decode.</param>
    /// <param name="encoding">The encoding to use. Defaults to <see cref="Encoding.UTF8"/> if not provided.</param>
    /// <returns></returns>
    public static BinaryValue FromEncodedString(string value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return new(encoding.GetBytes(value), true);
    }

    public static bool TryFromBase62EncodedString(string value, out BinaryValue binaryValue)
    {
        Guard.IsNotNull(value);
        return TryFromEncodedString(value, BinaryStringEncoding.Base62, out binaryValue);
    }

    public static bool TryFromBase64EncodedString(string value, out BinaryValue binaryValue)
    {
        Guard.IsNotNull(value);
        return TryFromEncodedString(value, BinaryStringEncoding.Base64, out binaryValue);
    }

    public static bool TryFromEncodedString(string value, BinaryStringEncoding encoding, out BinaryValue binaryValue)
    {
        Guard.IsNotNull(value);
        return TryFromEncodedChars(value.AsSpan(), encoding, out binaryValue);
    }

    public static bool TryFromEncodedChars(ReadOnlySpan<char> value, BinaryStringEncoding encoding, out BinaryValue binaryValue)
    {
        return encoding switch
        {
            BinaryStringEncoding.Base62 => TryDecodeFromBase62(value, out binaryValue),
            BinaryStringEncoding.HexLower => TryDecodeFromLowerHex(value, out binaryValue),
            BinaryStringEncoding.HexUpper => TryDecodeFromUpperHex(value, out binaryValue),
            _ => TryDecodeFromBase64(value, out binaryValue)
        };
    }

    public static bool TryFromEncodedBytes(ReadOnlySpan<byte> value, BinaryStringEncoding encoding, out BinaryValue binaryValue)
    {
        return encoding switch
        {
            BinaryStringEncoding.Base62 => TryDecodeFromBase62(Encoding.UTF8.GetString(value), out binaryValue),
            BinaryStringEncoding.HexLower => TryDecodeFromHex(value, out binaryValue),
            BinaryStringEncoding.HexUpper => TryDecodeFromHex(value, out binaryValue),
            _ => TryDecodeFromBase64(value, out binaryValue)
        };
    }

    private static bool TryDecodeFromBase64(ReadOnlySpan<char> base64, out BinaryValue binaryValue)
    {
        var size = GetRequiredBufferSize(base64.Length, BinaryStringEncoding.Base64);

        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(size)
            ? stackalloc byte[size]
            : (rented = SpanOwner<byte>.Allocate(size)).Span;

        using (rented)
        {
            if (Convert.TryFromBase64Chars(base64, buffer, out var bytesWritten))
            {
                binaryValue = new(buffer[..bytesWritten].ToArray(), noCopy: true);
                return true;
            }
        }

        binaryValue = default;
        return false;
    }

    private static bool TryDecodeFromBase64(ReadOnlySpan<byte> base64, out BinaryValue binaryValue)
    {
        var size = GetRequiredBufferSize(base64.Length, BinaryStringEncoding.Base64);

        var rented = SpanOwner<byte>.Empty;

        Span<byte> bytes = MemoryThresholds.CanStackalloc<byte>(size)
            ? stackalloc byte[size]
            : (rented = SpanOwner<byte>.Allocate(size)).Span;

        using (rented)
        {
            var status = Base64.DecodeFromUtf8(base64, bytes, out _, out var bytesWritten, isFinalBlock: true);

            if (status == OperationStatus.Done)
            {
                binaryValue = new(bytes[..bytesWritten].ToArray(), noCopy: true);
                return true;
            }

            Debug.Assert(status == OperationStatus.InvalidData,
                "We should be sizing the buffer so that this is the only possible status.");

            binaryValue = default;
            return false;
        }
    }

    private static bool TryDecodeFromUpperHex(ReadOnlySpan<char> hex, out BinaryValue binaryValue)
    {
        if (HexConverter.IsValidUpperHex(hex))
        {
            binaryValue = new(Convert.FromHexString(hex), noCopy: true);
            return true;
        }

        binaryValue = default;
        return false;
    }

    private static bool TryDecodeFromLowerHex(ReadOnlySpan<char> hex, out BinaryValue binaryValue)
    {
        if (HexConverter.IsValidLowerHex(hex))
        {
            binaryValue = new(Convert.FromHexString(hex), noCopy: true);
            return true;
        }

        binaryValue = default;
        return false;
    }

    private static bool TryDecodeFromHex(ReadOnlySpan<byte> hex, out BinaryValue binaryValue)
    {
        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(hex.Length)
            ? stackalloc byte[hex.Length]
            : (rented = SpanOwner<byte>.Allocate(hex.Length)).Span;

        using (rented)
        {
            if (HexConverter.TryConvertFromUtf8(hex, buffer, out var bytesWritten))
            {
                binaryValue = new(buffer[..bytesWritten].ToArray(), noCopy: true);
                return true;
            }

            binaryValue = default;
            return false;
        }
    }

    private static bool TryDecodeFromBase62(ReadOnlySpan<char> base62, out BinaryValue binaryValue)
    {
        if (Base62Converter.TryFromBase62Chars(base62, out var bytes))
        {
            binaryValue = new(bytes, noCopy: true);
            return true;
        }

        binaryValue = default;
        return false;
    }
}
