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

    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        return true;
    }

    public bool Equals(TokenText other, StringComparison stringComparison)
    {
        return Equals(other._value, stringComparison);
    }

    public bool Equals(WordText other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(WordText other, StringComparison stringComparison)
    {
        return Equals(other.Value, stringComparison);
    }

    public bool Equals(CharSequence other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(CharSequence other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

    public bool Equals(string other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(string other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator TokenText(string word)
    {
        return new(word);
    }

    public static implicit operator TokenText(WordText word)
    {
        return new((CharSequence)word);
    }

    public static bool operator ==(TokenText left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(TokenText left, CharSequence right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator ==(TokenText left, string right)
    {
        return right is not null && left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(TokenText left, string right)
    {
        return !(right is not null && left.Equals(right, StringComparison.Ordinal));
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

}
