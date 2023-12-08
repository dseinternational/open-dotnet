// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

[ComparableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<TokenText, CharSequence>))]
public readonly partial struct TokenText
    : IComparableValue<TokenText, CharSequence>,
      ISpanSerializableValue<TokenText, CharSequence>
{
    public static int MaxSerializedCharLength => 32;

    public TokenText(string word) : this((CharSequence)word)
    {
    }

    public TokenText(ReadOnlyMemory<char> word) : this(new CharSequence(word))
    {
    }

    public int Length => _value.Length;

    public CharSequence Value => _value;

    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        return true;
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

#pragma warning restore CA2225 // Operator overloads have named alternates

}
