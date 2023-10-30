// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A single distinct meaningful element of speech or writing, used with others
/// (or sometimes alone) to form a sentence and typically shown with a space on
/// either side when written or printed.
/// </summary>
/// <remarks>
/// This structure is simply the characters in the word. For tokens, see TokenizedWord.
/// </remarks>
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Word, CharSequence>))]
public readonly partial struct Word : IComparableValue<Word, CharSequence>
{
    public static int MaxSerializedCharLength => 64;

    public Word(string word) : this((CharSequence)word)
    {
    }

    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (char.IsLetter(c))
            {
                continue;
            }

            if (char.IsWhiteSpace(value[i])
                && (i > 0 || i < value.Length - 1))
            {
                continue;
            }

            if (char.IsPunctuation(value[i]))
            {
                if (i > 0)
                {
                    if (c == '\''
                        || c == '’'
                        || (c == '-' && i < value.Length - 1))
                    {
                        continue;
                    }
                }
            }

            return false;
        }

        return true;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Word(string word)
    {
        return new(word);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

}
