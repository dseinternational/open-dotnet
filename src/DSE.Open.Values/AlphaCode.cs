// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters used to identify something.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<AlphaCode, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct AlphaCode
    : IComparableValue<AlphaCode, AsciiString>,
      IUtf8SpanSerializable<AlphaCode>,
      IRepeatableHash64
{
    /// <summary>
    /// Gets the maximum number of characters required when serializing an <see cref="AlphaCode"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// Gets the maximum number of bytes required when serializing an <see cref="AlphaCode"/> as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => MaxLength;

    /// <summary>
    /// The maximum number of ASCII letters permitted in an <see cref="AlphaCode"/>.
    /// </summary>
    public const int MaxLength = 32;

    /// <summary>
    /// Initialises a new <see cref="AlphaCode"/> by parsing the supplied string as ASCII.
    /// </summary>
    public AlphaCode(string code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initialises a new <see cref="AlphaCode"/> by parsing the supplied character span as ASCII.
    /// </summary>
    public AlphaCode(ReadOnlySpan<char> code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initialises a new <see cref="AlphaCode"/> from the supplied <see cref="AsciiString"/>, validating its contents.
    /// </summary>
    public AlphaCode(AsciiString code)
        : this(code, false)
    {
    }

    /// <summary>
    /// Gets the number of ASCII letters in the code.
    /// </summary>
    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s)
    {
        return CodeStringPool.Shared.GetOrAdd(s);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is non-empty, no longer than
    /// <see cref="MaxLength"/>, and contains only ASCII letters.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnlyAsciiLetters();
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied character span using an ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied character span using a case-insensitive ordinal comparison.
    /// </summary>
    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied string using an ordinal comparison.
    /// </summary>
    public bool Equals(string other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied string using a case-insensitive ordinal comparison.
    /// </summary>
    public bool EqualsIgnoreCase(string other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Compares this code with another, ignoring case.
    /// </summary>
    public int CompareToIgnoreCase(AlphaCode other)
    {
        return _value.CompareToIgnoreCase(other._value);
    }

    /// <summary>
    /// Returns a read-only span over the underlying ASCII characters.
    /// </summary>
    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.AsSpan();
    }

    /// <summary>
    /// Copies the code's characters to a new <see cref="char"/> array.
    /// </summary>
    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a string to an <see cref="AlphaCode"/> by parsing it with the invariant culture.
    /// </summary>
    public static explicit operator AlphaCode(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Returns the code as a lower-case string.
    /// </summary>
    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    /// <summary>
    /// Returns the code as an upper-case string.
    /// </summary>
    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
