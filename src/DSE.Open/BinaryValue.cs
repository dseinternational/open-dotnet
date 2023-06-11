// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable binary data value.
/// </summary>
[JsonConverter(typeof(JsonStringBinaryValueBase64Converter))]
public readonly record struct BinaryValue
{
    private readonly ReadOnlyMemory<byte> _value;

    public static readonly BinaryValue Empty;

    /// <summary>
    /// Initialises a new <see cref="BinaryValue"/> with a copy of an array of bytes.
    /// </summary>
    /// <param name="value"></param>
    public BinaryValue(byte[] value) : this(value.AsSpan())
    {
    }

    /// <summary>
    /// Initialises a new <see cref="BinaryValue"/> with a copy of a span of bytes.
    /// </summary>
    /// <param name="value"></param>
    public BinaryValue(ReadOnlySpan<byte> value)
    {
        _value = value.ToArray();
    }

    private BinaryValue(byte[] value, bool noCopy)
    {
        Debug.Assert(noCopy);
        _value = value;
    }

    /// <summary>
    /// Gets the number of bytes in the value.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Gets the number of bits in the value.
    /// </summary>
    public int BitLength => Length * 8;

    public ReadOnlyMemory<byte> AsMemory() => _value;

    public ReadOnlySpan<byte> AsSpan() => _value.Span;

    public static BinaryValue FromBase62EncodedString(string value)
        => FromEncodedString(value, BinaryStringEncoding.Base62);

    public static BinaryValue FromBase64EncodedString(string value)
        => FromEncodedString(value, BinaryStringEncoding.Base64);

    public static BinaryValue FromEncodedString(string value, BinaryStringEncoding encoding)
    {
        return encoding == BinaryStringEncoding.Base62
            ? new BinaryValue(Base62Converter.FromBase62(value), true)
            : encoding is BinaryStringEncoding.HexLower or BinaryStringEncoding.HexUpper
            ? new BinaryValue(Convert.FromHexString(value), true)
            : new BinaryValue(Convert.FromBase64String(value), true);
    }

    public static BinaryValue FromString(string value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;

        return new BinaryValue(encoding.GetBytes(value), true);
    }

    public static bool TryFromBase62EncodedString(string value, out BinaryValue binaryValue)
        => TryFromEncodedString(value, BinaryStringEncoding.Base62, out binaryValue);

    public static bool TryFromBase64EncodedString(string value, out BinaryValue binaryValue)
        => TryFromEncodedString(value, BinaryStringEncoding.Base64, out binaryValue);

    public static bool TryFromEncodedString(string value, BinaryStringEncoding encoding, out BinaryValue binaryValue)
    {
        switch (encoding)
        {
            case BinaryStringEncoding.Base62 when Base62Converter.TryFromBase62(value, out var data):
                binaryValue = new BinaryValue(data, true);
                return true;
            case BinaryStringEncoding.Base62:
                binaryValue = default;
                return false;
            case BinaryStringEncoding.HexLower or BinaryStringEncoding.HexUpper:
                try
                {
                    binaryValue = new BinaryValue(Convert.FromHexString(value), true);
                    return true;
                }
                catch (FormatException)
                {
                }
                catch (ArgumentNullException)
                {
                }

                break;
        }

        try
        {
            binaryValue = new BinaryValue(Convert.FromBase64String(value), true);
            return true;
        }
        catch (FormatException)
        {
        }
        catch (ArgumentNullException)
        {
        }

        binaryValue = default;
        return false;
    }

    /// <summary>
    /// Returns a copy of the value as an array.
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray() => _value.ToArray();

    public bool Equals(BinaryValue other) =>  _value.Span.SequenceEqual(other._value.Span);

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, null, null);

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

        var hex = ToString();

        if (destination.Length >= hex.Length)
        {
            hex.AsSpan().CopyTo(destination);
            charsWritten = hex.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public override string ToString() => ToBase64EncodedString();

    public string ToString(BinaryStringEncoding format)
    {
        return _value.Length == 0
            ? string.Empty
            : format switch
            {
                BinaryStringEncoding.Base62 => ToBase62EncodedString(),
                BinaryStringEncoding.HexUpper => Convert.ToHexString(_value.Span),
                _ => format == BinaryStringEncoding.HexLower
                    ? Convert.ToHexString(_value.Span).ToLowerInvariant()
                    : ToBase64EncodedString()
            };
    }

    public string ToBase62EncodedString()
        => _value.Length == 0 ? string.Empty : Base62Converter.ToBase62String(_value.Span);

    public string ToBase64EncodedString()
        => _value.Length == 0 ? string.Empty : Convert.ToBase64String(_value.Span);

    public override int GetHashCode()
    {
        var span = _value.Span;

        unchecked
        {
            const int p = 16777619;
            var hash = (int)2166136261;

            if (!span.IsEmpty)
            {
                foreach (var @byte in span)
                {
                    hash = (hash ^ @byte) * p;
                }
            }

            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;

            return hash;
        }
    }

    /// <summary>
    /// Creates a random binary value with the specified number of bytes.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static BinaryValue GetRandomValue(int length = 64)
    {
        var buffer = new byte[length];
        RandomNumberGenerator.Fill(buffer);
        return new BinaryValue(buffer, true);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyMemory<byte>(BinaryValue value) => value._value;

    public static explicit operator ReadOnlySpan<byte>(BinaryValue value) => value._value.Span;

#pragma warning restore CA2225 // Operator overloads have named alternates

}

