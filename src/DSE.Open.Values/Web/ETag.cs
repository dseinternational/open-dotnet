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
    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    public const int MaxLength = 1024 * 4;

    public Etag(string code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public Etag(ReadOnlySpan<char> code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public Etag(AsciiString code)
        : this(code, false)
    {
    }

    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s)
    {
        return CodeStringPool.Shared.GetOrAdd(s);
    }

    public static bool IsValidValue(AsciiString value)
    {
        return value is { IsEmpty: false, Length: <= MaxLength };
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    public bool Equals(string other)
    {
        return _value.Equals(other);
    }

    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.AsSpan();
    }

    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Etag(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
