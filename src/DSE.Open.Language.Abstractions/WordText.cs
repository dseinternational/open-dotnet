// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Text that represents a word - single distinct meaningful element of speech
/// or writing, used with others (or sometimes alone) to form a sentence and
/// typically shown with a space on either side when written or printed.
/// Includes compound words, which may be hyphenated, space-separated or not
/// separated.
/// <para>May also be template - for example, as a placeholder for a proper
/// noun. Templates are in the format <c>{{ template_id }}</c> where <c>template_id</c>
/// is from 4 to 26 lowercase ASCII characters or underscores ('_').</para>
/// </summary>
/// <remarks>
/// This structure is simply the characters in the word or template.
/// </remarks>
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<WordText, CharSequence>))]
public readonly partial struct WordText
    : IComparableValue<WordText, CharSequence>,
      ISpanSerializableValue<WordText, CharSequence>
{
    public static int MaxSerializedCharLength => 32;

    public WordText(string word) : this((CharSequence)word)
    {
    }

    public WordText(ReadOnlyMemory<char> word) : this(new CharSequence(word))
    {
    }

    public int Length => _value.Length;

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

    public bool Equals(WordText other, StringComparison stringComparison)
    {
        return Equals(other._value, stringComparison);
    }

    public bool Equals(TokenText other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(TokenText other, StringComparison stringComparison)
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

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator WordText(string word)
    {
        return new(word);
    }

    public static explicit operator WordText(TokenText word)
    {
        return new((CharSequence)word);
    }

    public static bool operator ==(WordText left, TokenText right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(WordText left, TokenText right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator ==(WordText left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(WordText left, CharSequence right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator ==(WordText left, string right)
    {
        return right is not null && left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(WordText left, string right)
    {
        return !(right is not null && left.Equals(right, StringComparison.Ordinal));
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public WordId GetId(WordMeaningId meaningId, LanguageTag language)
    {
        return WordId.FromWord(meaningId, this, language);
    }
}
