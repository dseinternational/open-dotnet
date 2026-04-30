// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;
using DSE.Open.Hashing;
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
      ISpanSerializableValue<WordText, CharSequence>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="WordText"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// Initializes a new <see cref="WordText"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="word">The characters of the word.</param>
    public WordText(string word) : this((CharSequence)word)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="WordText"/> from a
    /// <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
    /// </summary>
    /// <param name="word">The characters of the word.</param>
    public WordText(ReadOnlyMemory<char> word) : this(new CharSequence(word))
    {
    }

    /// <summary>
    /// The number of characters in the word.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// The underlying <see cref="CharSequence"/> holding the characters of the word.
    /// </summary>
    public CharSequence Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="WordText"/> is a
    /// template (a value enclosed in <c>{{</c> and <c>}}</c>).
    /// </summary>
    public bool IsTemplate => !_value.IsEmpty && _value[0] == '{';

    /// <summary>
    /// A <see cref="ReadOnlySpan{T}"/> over the characters of the word.
    /// </summary>
    public ReadOnlySpan<char> Span => _value.Span;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid
    /// <see cref="WordText"/> value (a word or a template, see remarks on the type).
    /// </summary>
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
            // Minimum template is "{{abcd}}" — 2 braces + 4-char id + 2 braces.
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

            var interior = value.Span[2..^2];

            // Spacing must be symmetric: either both edges are whitespace, or neither.
            var leadingSpace = char.IsWhiteSpace(interior[0]);
            var trailingSpace = char.IsWhiteSpace(interior[^1]);

            if (leadingSpace != trailingSpace)
            {
                return false;
            }

            var id = leadingSpace ? interior[1..^1] : interior;

            if (id.Length is < 4 or > 26)
            {
                return false;
            }

            foreach (var c in id)
            {
                if (!char.IsAsciiLetterLower(c) && c != '_')
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

    /// <summary>
    /// Indicates whether this word equals another <see cref="WordText"/>,
    /// using the specified <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(WordText other, StringComparison stringComparison)
    {
        return Equals(other._value, stringComparison);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="TokenText"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(TokenText other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="TokenText"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(TokenText other, StringComparison stringComparison)
    {
        return Equals(other.Value, stringComparison);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="CharSequence"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(CharSequence other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="CharSequence"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(CharSequence other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="string"/>
    /// using ordinal comparison.
    /// </summary>
    public bool Equals(string other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates whether this word equals the specified <see cref="string"/>
    /// using the given <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(string other, StringComparison stringComparison)
    {
        return _value.Equals(other, stringComparison);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="WordText"/>.
    /// </summary>
    public static explicit operator WordText(string word)
    {
        return new(word);
    }

    /// <summary>
    /// Converts a <see cref="TokenText"/> to a <see cref="WordText"/>.
    /// </summary>
    public static explicit operator WordText(TokenText word)
    {
        return new((CharSequence)word);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is equal
    /// to the specified <see cref="TokenText"/> using ordinal comparison.
    /// </summary>
    public static bool operator ==(WordText left, TokenText right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is not
    /// equal to the specified <see cref="TokenText"/> using ordinal comparison.
    /// </summary>
    public static bool operator !=(WordText left, TokenText right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is equal
    /// to the specified <see cref="CharSequence"/> using ordinal comparison.
    /// </summary>
    public static bool operator ==(WordText left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is not
    /// equal to the specified <see cref="CharSequence"/> using ordinal comparison.
    /// </summary>
    public static bool operator !=(WordText left, CharSequence right)
    {
        return !left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is equal
    /// to the specified <see cref="string"/> using ordinal comparison.
    /// </summary>
    public static bool operator ==(WordText left, string right)
    {
        return right is not null && left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <see cref="WordText"/> is not
    /// equal to the specified <see cref="string"/> using ordinal comparison.
    /// </summary>
    public static bool operator !=(WordText left, string right)
    {
        return !(right is not null && left.Equals(right, StringComparison.Ordinal));
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Returns a <see cref="WordId"/> derived from the specified
    /// <see cref="WordMeaningId"/>, this <see cref="WordText"/> and the
    /// specified <see cref="LanguageTag"/>.
    /// </summary>
    public WordId GetId(WordMeaningId meaningId, LanguageTag language)
    {
        return WordId.FromWord(meaningId, this, language);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
