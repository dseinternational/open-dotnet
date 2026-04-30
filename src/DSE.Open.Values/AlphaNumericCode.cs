// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters or digits used to identify something.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<AlphaNumericCode, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct AlphaNumericCode
    : IComparableValue<AlphaNumericCode, AsciiString>,
      IUtf8SpanSerializable<AlphaNumericCode>,
      IRepeatableHash64
{
    /// <summary>
    /// Gets the maximum number of characters required when serializing an <see cref="AlphaNumericCode"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// Gets the maximum number of bytes required when serializing an <see cref="AlphaNumericCode"/> as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => MaxLength;

    /// <summary>
    /// The maximum number of ASCII letters or digits permitted in an <see cref="AlphaNumericCode"/>.
    /// </summary>
    public const int MaxLength = 32;

    /// <summary>
    /// Initialises a new <see cref="AlphaNumericCode"/> by parsing the supplied string as ASCII.
    /// </summary>
    public AlphaNumericCode(string code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initialises a new <see cref="AlphaNumericCode"/> by parsing the supplied character span as ASCII.
    /// </summary>
    public AlphaNumericCode(ReadOnlySpan<char> code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Initialises a new <see cref="AlphaNumericCode"/> from the supplied <see cref="AsciiString"/>, validating its contents.
    /// </summary>
    public AlphaNumericCode(AsciiString code) : this(code, false)
    {
    }

    /// <summary>
    /// Gets the number of ASCII letters or digits in the code.
    /// </summary>
    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s)
    {
        return CodeStringPool.Shared.GetOrAdd(s);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is non-empty, no longer than
    /// <see cref="MaxLength"/>, and contains only ASCII letters or digits.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnlyAsciiLettersOrDigits();
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied character span using an ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied string using an ordinal comparison.
    /// </summary>
    public bool Equals(string other)
    {
        return _value.Equals(other);
    }

    /// <summary>
    /// Compares this code with another, ignoring case.
    /// </summary>
    public int CompareToIgnoreCase(AlphaNumericCode other)
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
    /// Explicitly converts a string to an <see cref="AlphaNumericCode"/> by parsing it with the invariant culture.
    /// </summary>
    public static explicit operator AlphaNumericCode(string value)
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
