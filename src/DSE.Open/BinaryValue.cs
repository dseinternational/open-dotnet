// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable binary data value.
/// </summary>
[JsonConverter(typeof(JsonStringBinaryValueBase64Converter))]
public readonly partial record struct BinaryValue : ISpanFormattable, IUtf8SpanFormattable
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

    /// <summary>
    /// Returns a copy of the value as an array.
    /// </summary>
    public byte[] ToArray() => _value.ToArray();

    public bool Equals(BinaryValue other) => _value.Span.SequenceEqual(other._value.Span);

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

    public static explicit operator ReadOnlyMemory<byte>(BinaryValue value) => value.AsMemory();

    public static explicit operator ReadOnlySpan<byte>(BinaryValue value) => value.AsSpan();

#pragma warning restore CA2225 // Operator overloads have named alternates

    internal static BinaryValue CreateUnsafe(byte[] bytes)
    {
        Guard.IsNotNull(bytes);

        if (bytes.Length == 0)
        {
            return Empty;
        }

        return new BinaryValue(bytes, noCopy: true);
    }

}
