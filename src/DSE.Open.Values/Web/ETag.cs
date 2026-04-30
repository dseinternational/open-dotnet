// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Web;

/// <summary>
/// An immutable sequence of ASCII characters used to identify a specific version of a resource.
/// Limited to a maximum of 4096 characters.
/// </summary>
/// <remarks>
/// See <see href="https://datatracker.ietf.org/doc/html/rfc9110#section-8.8.3">RFC 9110, section 8.8.3.</see>
/// </remarks>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Etag, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Etag
    : IComparableValue<Etag, AsciiString>,
      IUtf8SpanSerializable<Etag>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters required to serialize an <see cref="Etag"/>.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// The maximum number of UTF-8 bytes required to serialize an <see cref="Etag"/>.
    /// </summary>
    public static int MaxSerializedByteLength => MaxLength;

    /// <summary>
    /// The maximum permitted length, in characters, of an <see cref="Etag"/> value.
    /// </summary>
    public const int MaxLength = 1024 * 4;

    /// <summary>
    /// Initializes a new <see cref="Etag"/> by parsing the specified string.
    /// </summary>
    public Etag(string code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Etag"/> by parsing the specified character span.
    /// </summary>
    public Etag(ReadOnlySpan<char> code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Etag"/> from the specified ASCII string.
    /// </summary>
    public Etag(AsciiString code)
        : this(code, false)
    {
    }

    /// <summary>
    /// Gets the length, in characters, of this <see cref="Etag"/>.
    /// </summary>
    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s)
    {
        return CodeStringPool.Shared.GetOrAdd(s);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the specified value is a valid <see cref="Etag"/>;
    /// that is, non-empty and no longer than <see cref="MaxLength"/> characters.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return value is { IsEmpty: false, Length: <= MaxLength };
    }

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="Etag"/> equals the specified character span.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="Etag"/> equals the specified character span,
    /// ignoring case.
    /// </summary>
    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="Etag"/> equals the specified string.
    /// </summary>
    public bool Equals(string other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Returns a read-only span over the underlying ASCII characters.
    /// </summary>
    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.AsSpan();
    }

    /// <summary>
    /// Returns a new <see cref="char"/> array containing the characters of this <see cref="Etag"/>.
    /// </summary>
    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="string"/> to an <see cref="Etag"/> by parsing it
    /// using the invariant culture.
    /// </summary>
    public static explicit operator Etag(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
