// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Text;

/// <summary>
/// Specifies a pattern that can be used to match a string. Can be translated to a SQL LIKE pattern.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>
/// <term>*</term>
/// <description>Matches any single character.</description>
/// </item>
/// <item>
/// <term>?</term>
/// <description>Matches any string of zero or more characters.</description>
/// </item>
/// <item>
/// <term>[ ]</term>
/// <description>Any single character within the specified range <c>[a-f]</c> or set <c>[abcdef]</c>.</description>
/// </item>
/// <item>
/// <term>[^ ]</term>
/// <description>Any single character not within the specified range <c>[^a-f]</c> or set <c>[^abcdef]</c>.</description>
/// </item>
/// </list>
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringLikePatternConverter))]
public readonly record struct LikePattern : IEquatable<string>, ISpanParsable<LikePattern>, ISpanFormattable
{
    public const int MaxLength = StackallocThresholds.MaxCharLength;

    public static readonly LikePattern Empty;

    /// <remarks>
    /// <c>null</c> if <cref cref="Empty"/>.
    /// </remarks>
    private readonly string? _pattern;

    public LikePattern(string pattern) : this(pattern, true) { }

    private LikePattern(string pattern, bool validate)
    {
        Debug.Assert(pattern is not null);

        if (validate)
        {
            EnsureIsValid(pattern);
        }

        _pattern = pattern;
    }

    public LikePattern(ReadOnlySpan<char> pattern) : this(pattern, true) { }

    private LikePattern(ReadOnlySpan<char> pattern, bool validate)
    {
        Debug.Assert(pattern.Length > 0);

        if (validate)
        {
            EnsureIsValid(pattern);
        }

        _pattern = pattern.ToString();
    }

    public static void EnsureIsValid(ReadOnlySpan<char> pattern)
    {
        if (!IsValid(pattern))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(pattern), $"Invalid {nameof(LikePattern)}: {pattern}");
        }
    }

    public static bool IsValid(ReadOnlySpan<char> pattern)
    {
        return pattern.Length <= MaxLength;
    }

    public bool Equals(string? other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(string? other, StringComparison comparison)
    {
        return other is not null
               && ((_pattern is null && other.Length == 0) || string.Equals(_pattern, other, comparison));
    }

    public static bool TryParse(ReadOnlySpan<char> s, out LikePattern result)
    {
        return TryParse(s, null, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out LikePattern result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (!IsValid(s))
        {
            result = default;
            return false;
        }

        result = new LikePattern(s, validate: false);
        return true;
    }

    public static LikePattern Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static LikePattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<LikePattern>($"Could not parse {nameof(LikePattern)} with value: {s}");
    }

    public static bool TryParse(string? s, out LikePattern result)
    {
        return TryParse(s, null, out result);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out LikePattern result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static LikePattern Parse(string s)
    {
        return Parse(s, null);
    }

    public static LikePattern Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var span = _pattern.AsSpan();

        if (destination.Length < span.Length)
        {
            charsWritten = 0;
            return false;
        }

        span.CopyTo(destination);
        charsWritten = span.Length;

        return true;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _pattern ?? string.Empty;
    }

    /// <summary>
    /// Indicates if the specified value matches the pattern.
    /// </summary>
    /// <param name="value"></param>
    /// <returns><see langword="true"/> if the specified value matches the patterm; otherwise, <see langword="false"/>.</returns>
    /// <remarks>This method performs an ordinal comparison.</remarks>
    public bool IsMatch(string value)
    {
        return IsMatch(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Indicates if the specified value matches the pattern.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparison"></param>
    /// <returns><see langword="true"/> if the specified value matches the patterm; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public bool IsMatch(string value, StringComparison comparison)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparer = StringComparer.FromComparison(comparison);

        var patternToMatch = _pattern ?? string.Empty;

        var patternIndex = 0;
        var valueIndex = 0;

        while (true)
        {
            if (patternIndex >= patternToMatch.Length)
            {
                return valueIndex >= value.Length;
            }

            var p = patternToMatch[patternIndex];

            if (valueIndex >= value.Length)
            {
                // end of value
                return p == '*';
            }

            var v = value[valueIndex];

            switch (p)
            {
                case '*':
                    // skip any number of characters

                    patternIndex++;

                    if (patternIndex >= patternToMatch.Length)
                    {
                        // * at end of pattern matches everything
                        return true;
                    }

                    // skip until next match

                    while (true)
                    {
                        if (valueIndex >= value.Length)
                        {
                            // end of value
                            return false;
                        }

                        var c = value[valueIndex];

                        if (comparer.Equals(c, patternToMatch[patternIndex]))
                        {
                            // found next match
                            break;
                        }

                        valueIndex++;
                    }

                    break;

                case '?':
                    // skip a single character

                    patternIndex++;
                    valueIndex++;

                    break;

                case '[':
                    // match any character in set

                    patternIndex++;

                    if (patternIndex >= patternToMatch.Length)
                    {
                        // [ at end of patternToMatch matches nothing
                        return false;
                    }

                    // TODO: escaped ']' - e.g. "\]"

                    var set = patternToMatch[patternIndex..].Split(']')[0];

                    if (set.Length == 0)
                    {
                        // [] matches nothing
                        return false;
                    }

                    // TODO: support range - e.g. [a-z]

                    if (!set.Contains(v, comparison))
                    {
                        // no match
                        return false;
                    }

                    patternIndex += set.Length + 1;
                    valueIndex++;
                    break;

                case '\\':

                    // escape

                    // TODO: support [] escaping
                    // https://learn.microsoft.com/en-us/sql/t-sql/language-elements/like-transact-sql?view=sql-server-ver16#pattern-match-with-the-escape-clause

                    patternIndex++;

                    if (patternIndex >= patternToMatch.Length)
                    {
                        // \ at end of patternToMatch matches nothing
                        return false;
                    }

                    p = patternToMatch[patternIndex];
                    goto default;

                default:

                    if (!comparer.Equals(p, v))
                    {
                        // no match
                        return false;
                    }

                    patternIndex++;
                    valueIndex++;
                    break;
            }
        }
    }

    public string ToSqlLikePattern()
    {
        if (_pattern is null)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(_pattern.Length + 2);

        for (var i = 0; i < _pattern.Length; i++)
        {
            var c = _pattern[i];

            // TODO: support escaping - e.g. "\*" or "\?"

            _ = c switch
            {
                '*' => builder.Append('%'),
                '?' => builder.Append('_'),
                '%' or '_' => builder.Append('[').Append(c).Append(']'),
                _ => builder.Append(c),
            };
        }

        return builder.ToString();
    }

    public static explicit operator string(LikePattern pattern)
    {
        return pattern.ToString();
    }
}
