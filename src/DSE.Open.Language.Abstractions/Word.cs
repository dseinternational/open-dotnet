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
/// either side when written or printed. Includes compound words, which may be
/// hyphenated, space-separated or not separated.
/// <para>May also be template - for example, as a placeholder for a proper
/// noun. Templates are in the format <c>{{ template_id }}</c> where <c>template_id</c>
/// is from 4 to 26 lowercase ASCII characters or underscores ('_').</para>
/// </summary>
/// <remarks>
/// This structure is simply the characters in the word or template.
/// </remarks>
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Word, CharSequence>))]
public readonly partial struct Word
    : IComparableValue<Word, CharSequence>,
      ISpanSerializableValue<Word, CharSequence>
{
    public static int MaxSerializedCharLength => 32;

    public Word(string word) : this((CharSequence)word)
    {
    }

    public Word(ReadOnlyMemory<char> word) : this(new CharSequence(word))
    {
    }

    public CharSequence Value => _value;

    public bool IsTemplate => !_value.IsEmpty && _value[0] == '{';

    public static bool IsValidValue(CharSequence value)
    {
        if (value.IsEmpty || value.Length > MaxSerializedCharLength)
        {
            return false;
        }

        // template "{{ child_subject_given_name }}"
        // template id min 4 chars, max 26 chars
        // with spaces inside braces, max overall length = 32

        if (value[0] == '{')
        {
            if (value.Length < 8)
            {
                return false;
            }

            if (value[1] != '{'
                || value[^1] != '}'
                || value[^2] != '}')
            {
                return false;
            }

            for (var i = 2; i < value.Length - 2; i++)
            {
                var c = value[i];

                if (char.IsWhiteSpace(c))
                {
                    if (i == 2 || i == value.Length - 3)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (char.IsAsciiLetterLower(c) || c == '_')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (char.IsLetter(c))
            {
                continue;
            }

            if (char.IsWhiteSpace(value[i])
                && i > 0
                && i < value.Length - 1)
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

    public static explicit operator Word(TokenText word)
    {
        return new((CharSequence)word);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

}
