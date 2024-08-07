// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Text;

/// <summary>
/// Specifies a pattern that can be used to match a string. Can be translated
/// to a SQL LIKE pattern.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>
/// <term>?</term>
/// <description>Matches any single character.</description>
/// </item>
/// <item>
/// <term>*</term>
/// <description>Matches any string of zero or more characters.</description>
/// </item>
/// <item>
/// <term>[ ]</term>
/// <description>Any single character within the specified range
/// <c>[a-f]</c> or set <c>[abcdef]</c>. Brackets inside a set are treated as
/// literals.</description>
/// </item>
/// <item>
/// <term>[^ ]</term>
/// <description>Any single character not within the specified range
/// <c>[^a-f]</c> or set <c>[^abcdef]</c>.</description>
/// </item>
/// </list>
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringLikePatternConverter))]
public readonly record struct LikePattern : IEquatable<string>, ISpanParsable<LikePattern>, ISpanFormattable
{
    public const int MaxLength = MemoryThresholds.StackallocCharThreshold;

    public static readonly LikePattern Empty;

    /// <remarks>
    /// <c>null</c> if <cref cref="Empty"/>.
    /// </remarks>
    private readonly string? _pattern;

    public LikePattern(string pattern) : this(pattern, true)
    {
    }

    private LikePattern(string pattern, bool validate)
    {
        Debug.Assert(pattern is not null);

        if (validate)
        {
            EnsureIsValid(pattern);
        }

        _pattern = pattern;
    }

    public LikePattern(ReadOnlySpan<char> pattern) : this(pattern, true)
    {
    }

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
        return other is not null &&
            ((_pattern is null && other.Length == 0) || string.Equals(_pattern, other, comparison));
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

        result = new(s, validate: false);
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

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
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
    /// <returns><see langword="true"/> if the specified value matches the pattern; otherwise, <see langword="false"/>.</returns>
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
    /// <returns><see langword="true"/> if the specified value matches the pattern; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public bool IsMatch(string value, StringComparison comparison)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparer = StringComparer.FromComparison(comparison);

        var patternToMatch = _pattern ?? string.Empty;

        var patternIndex = 0;
        var valueIndex = 0;

        var escapeNext = false;

        while (true)
        {
            var escapeThis = escapeNext;
            escapeNext = false;

            if (patternIndex >= patternToMatch.Length)
            {
                // If we've reached the end of the pattern without returning, then the pattern has matched the value
                return valueIndex >= value.Length;
            }

            var p = patternToMatch[patternIndex];

            if (valueIndex >= value.Length)
            {
                if (p != '*')
                {
                    return false;
                }

                if (patternIndex == patternToMatch.Length - 1)
                {
                    return true;
                }

                return patternToMatch[patternIndex + 1] != '?';
            }

            var v = value[valueIndex];

            if (!escapeThis)
            {
                switch (p)
                {
                    case '*':
                        if (TryMatchAsterisk(
                                ref patternIndex,
                                ref valueIndex,
                                patternToMatch,
                                value,
                                out var matched))
                        {
                            return matched;
                        }

                        continue;

                    case '?':
                        patternIndex++;
                        valueIndex++;
                        continue;

                    case '[':
                        if (TryMatchSet(
                                ref patternIndex,
                                ref valueIndex,
                                patternToMatch,
                                value,
                                comparison,
                                out matched))
                        {
                            return matched;
                        }
                        continue;

                    case '\\':
                        escapeNext = true;
                        patternIndex++;
                        continue;
                }
            }

            if (!comparer.Equals(p, v))
            {
                return false;
            }

            patternIndex++;
            valueIndex++;
        }
    }

    private static bool TryMatchAsterisk(
        ref int patternIndex,
        ref int valueIndex,
        string patternToMatch,
        string value,
        out bool matched)
    {
        patternIndex++;

        if (patternIndex >= patternToMatch.Length)
        {
            // `*` at end of pattern matches everything
            matched = true;
            return true;
        }

        if (patternToMatch[patternIndex] == '?')
        {
            // `*?` matches any string of _one or more_ characters. As such, we can't match 0 characters here, so make
            // sure that another character is present in the value by passing off to `?` matching.
            matched = false;
            return false;
        }

        // skip until next match
        while (true)
        {
            if (valueIndex >= value.Length)
            {
                // reached the end of the value without finding a match
                matched = false;
                return true;
            }

            var valueChar = value[valueIndex];
            var patternChar = patternToMatch[patternIndex];

            if (valueChar.Equals(patternChar))
            {
                // matched the `*` pattern. Break to continue matching the rest of the pattern.
                matched = default;
                return false;
            }

            valueIndex++;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> it can be determined that the pattern matches or does not match the value; in
    /// this case, the value of <paramref name="matched"/> represents whether the range pattern matches. Otherwise,
    /// <see langword="false"/> is returned and <paramref name="matched"/> is meaningless and always
    /// <see langword="default"/>
    /// </summary>
    /// <exception cref="FormatException">Thrown if there is a range bracket with no unescaped counterpart.</exception>
    private static bool TryMatchSet(
        ref int patternIndex,
        ref int valueIndex,
        string patternToMatch,
        string value,
        StringComparison comparison,
        out bool matched)
    {
        Debug.Assert(patternToMatch[patternIndex] == '[');

        patternIndex++;

        if (patternIndex >= patternToMatch.Length)
        {
            // [ at end of patternToMatch matches nothing
            matched = false;
            return true;
        }

        // We want the first range from `Split`. If there are _more than 1_ ranges, then the range for the _remainder_
        // of the pattern will be in the second range.
        Span<Range> rangeBuffer = stackalloc Range[2];
        var written = patternToMatch.AsSpan(patternIndex).Split(rangeBuffer, ']');

        Debug.Assert(written == 2);

        var currentRange = rangeBuffer[0];

        var indexOfOpeningBracket = patternIndex - 1;
        var indexOfClosingBracket = patternIndex + currentRange.End.Value;

        Debug.Assert(indexOfOpeningBracket < indexOfClosingBracket);
        Debug.Assert(patternToMatch[indexOfOpeningBracket] == '[');
        Debug.Assert(patternToMatch[indexOfClosingBracket] == ']');

        var start = rangeBuffer[0].Start.Value + patternIndex;
        var end = rangeBuffer[0].End.Value + patternIndex;
        var set = patternToMatch.AsSpan(start, end - start);

        switch (set.Length)
        {
            case 0:
                throw new FormatException("Range contains no characters");
            case 3 when set[1] == '-':
                return TryMatchRange(
                    ref patternIndex,
                    ref valueIndex,
                    value,
                    set,
                    out matched);
            case 4 when set[0] == '^' && set[2] == '-':
                return TryMatchNegatedRange(
                    ref patternIndex,
                    ref valueIndex,
                    value,
                    set,
                    out matched);
        }

        var v = value[valueIndex];

        if (patternToMatch[indexOfOpeningBracket + 1] == '^')
        {
            // Negated set
            set = set[1..];

            if (set.Contains([v], comparison))
            {
                // no match
                matched = false;
                return true;
            }

            patternIndex += set.Length + 2; // 1 for the `^` and 1 for the `]`
            valueIndex++;
        }
        else
        {
            if (!set.Contains([v], comparison))
            {
                // no match
                matched = false;
                return true;
            }

            patternIndex += set.Length + 1;
            valueIndex++;
        }

        matched = default;
        return false;
    }

    private static bool TryMatchRange(
        ref int patternIndex,
        ref int valueIndex,
        string value,
        ReadOnlySpan<char> range,
        out bool matched)
    {
        Debug.Assert(range[1] == '-');

        var rangeStart = range[0];
        var rangeEnd = range[2];

        if (rangeStart > rangeEnd)
        {
            throw new FormatException($"The specified range ('[{rangeStart}-{rangeEnd}']) is invalid.");
        }

        if (!(char.IsAscii(rangeStart) && char.IsAscii(rangeEnd)))
        {
            throw new FormatException($"Only ASCII ranges are supported by {nameof(LikePattern)}");
        }

        if (value[valueIndex] > rangeEnd || value[valueIndex] < rangeStart)
        {
            matched = false;
            return true;
        }

        patternIndex += range.Length + 1;
        valueIndex += 1;

        matched = default;
        return false;
    }


    private static bool TryMatchNegatedRange(
        ref int patternIndex,
        ref int valueIndex,
        string value,
        ReadOnlySpan<char> range,
        out bool matched)
    {
        Debug.Assert(range.Length == 4);
        Debug.Assert(range[0] == '^');
        Debug.Assert(range[2] == '-');

        var rangeStart = range[1];
        var rangeEnd = range[3];

        if (rangeStart > rangeEnd)
        {
            throw new FormatException($"The specified range ('[{rangeStart}-{rangeEnd}']) is invalid.");
        }

        if (!(char.IsAscii(rangeStart) && char.IsAscii(rangeEnd)))
        {
            throw new FormatException($"Only ASCII ranges are supported by {nameof(LikePattern)}");
        }

        if (value[valueIndex] >= rangeStart && value[valueIndex] <= rangeEnd)
        {
            matched = false;
            return true;
        }

        patternIndex += range.Length + 1;
        valueIndex += 1;

        matched = default;
        return false;
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
            var current = _pattern[i];
            var hasPreviousChar = i > 0;

            if (hasPreviousChar && _pattern[i - 1] == '\\')
            {
                if (IsWildcard(current))
                {
                    _ = builder.Append(CultureInfo.InvariantCulture, $"[{current}]");
                    continue;
                }

                // E.g., `\*` should be outputted as `*` not `%`
                _ = builder.Append(current);
                continue;
            }

            _ = current switch
            {
                '*' => builder.Append('%'),
                '?' => builder.Append('_'),
                '%' or '_' => builder.Append('[').Append(current).Append(']'),
                '\\' => builder, // Don't write escape characters
                _ => builder.Append(current)
            };
        }

        return builder.ToString();
    }

    private static bool IsWildcard(char c)
    {
        // Note: `]` does not need to be escaped, so it is not included here
        return c is '%' or '_' or '[';
    }

    public static explicit operator string(LikePattern pattern)
    {
        return pattern.ToString();
    }
}
