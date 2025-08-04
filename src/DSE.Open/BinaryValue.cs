// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable binary data value.
/// </summary>
[JsonConverter(typeof(JsonStringBinaryValueBase64Converter))]
public readonly partial record struct BinaryValue : ISpanFormattable, IUtf8SpanFormattable, IRepeatableHash64
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

    public ReadOnlyMemory<byte> AsMemory()
    {
        return _value;
    }

    public ReadOnlySpan<byte> AsSpan()
    {
        return _value.Span;
    }

    /// <summary>
    /// Returns a copy of the value as an array.
    /// </summary>
    public byte[] ToArray()
    {
        return _value.ToArray();
    }

    public bool Equals(BinaryValue other)
    {
        return _value.Span.SequenceEqual(other._value.Span);
    }

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
        return new(buffer, true);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyMemory<byte>(BinaryValue value)
    {
        return value.AsMemory();
    }

    public static explicit operator ReadOnlySpan<byte>(BinaryValue value)
    {
        return value.AsSpan();
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    internal static BinaryValue CreateUnsafe(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        if (bytes.Length == 0)
        {
            return Empty;
        }

        return new(bytes, noCopy: true);
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(this);
    }

    public string ToHexString()
    {
        return Convert.ToHexString(_value.Span);
    }

    public string ToHexStringLower()
    {
        return Convert.ToHexStringLower(_value.Span);
    }

    public string ToBase64String()
    {
        return Convert.ToBase64String(_value.Span);
    }
}
