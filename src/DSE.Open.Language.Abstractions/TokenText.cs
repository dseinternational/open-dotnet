// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Text that represents a token - the literal surface form emitted by a
/// tokenizer, including whitespace and punctuation, which may or may not
/// correspond to a single <see cref="WordText">word</see>.
/// </summary>
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<TokenText, CharSequence>))]
public readonly partial struct TokenText
    : IComparableValue<TokenText, CharSequence>,
      ISpanSerializableValue<TokenText, CharSequence>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="TokenText"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// Initializes a new <see cref="TokenText"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="word">The characters of the token.</param>
    public TokenText(string word) : this((CharSequence)word)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="TokenText"/> from a
    /// <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
    /// </summary>
    /// <param name="word">The characters of the token.</param>
    public TokenText(ReadOnlyMemory<char> word) : this(new CharSequence(word))
    {
    }

    /// <summary>
    /// The number of characters in the token.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// The underlying <see cref="CharSequence"/> holding the characters of
    /// the token.
    /// </summary>
    public CharSequence Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid
    /// <see cref="TokenText"/> value (non-empty and within the maximum length).
    /// </summary>
    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Indicates whether this token equals another <see cref="TokenText"/>,
    /// using the specified <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(TokenText other, StringComparison stringComparison)
    {
        return Equals(other._value, stringComparison);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="WordText"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(WordText other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="WordText"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(WordText other, StringComparison stringComparison)
    {
        return Equals(other.Value, stringComparison);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="CharSequence"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(CharSequence other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="CharSequence"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(CharSequence other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="string"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(string other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this token equals the specified <see cref="string"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(string other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="TokenText"/>.
    /// </summary>
    public static explicit operator TokenText(string word)
    {
        return new(word);
    }

    /// <summary>
    /// Converts a <see cref="WordText"/> to a <see cref="TokenText"/>.
    /// </summary>
    public static implicit operator TokenText(WordText word)
    {
        return new((CharSequence)word);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="TokenText"/> is equal
    /// to the specified <see cref="CharSequence"/> using ordinal comparison.
    /// </summary>
    public static bool operator ==(TokenText left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="TokenText"/> is not
    /// equal to the specified <see cref="CharSequence"/> using ordinal comparison.
    /// </summary>
    public static bool operator !=(TokenText left, CharSequence right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="TokenText"/> is equal
    /// to the specified <see cref="string"/> using ordinal comparison.
    /// </summary>
    public static bool operator ==(TokenText left, string right)
    {
        return right is not null && left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="TokenText"/> is not
    /// equal to the specified <see cref="string"/> using ordinal comparison.
    /// </summary>
    public static bool operator !=(TokenText left, string right)
    {
        return !(right is not null && left.Equals(right, StringComparison.Ordinal));
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

}
