// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Text;

/// <summary>
/// Represents a pattern that can be used to specify a string search. Can be translated to a SQL CONTAINS clause.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<ContainsPattern, CharSequence>))]
public readonly partial struct ContainsPattern : IEquatableValue<ContainsPattern, CharSequence>
{
    private static readonly SearchValues<char> s_validSymbols =
        SearchValues.Create(['*', '(', ')', '"', '&', '!', '|']);

    public static int MaxSerializedCharLength => 128;

    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        foreach (var c in value.AsSpan())
        {
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || s_validSymbols.Contains(c))
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}
