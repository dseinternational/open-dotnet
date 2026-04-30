// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Language.Annotations.Serialization;

namespace DSE.Open.Language.Annotations;

// Builds on: https://github.com/ArthurDevNL/CoNLL-U/blob/main/Conllu/Conllu/TokenIdentifier.cs

/// <summary>
/// Identifies the position of a token in a sentence. Supports single tokens, multi-word
/// token ranges (e.g. <c>1-2</c>) and empty nodes (e.g. <c>1.2</c>).
/// </summary>
[JsonConverter(typeof(JsonStringTokenIdentifierConverter))]
public readonly struct TokenIndex
    : IComparable<TokenIndex>,
      IEquatable<TokenIndex>,
      ISpanFormattable,
      ISpanParsable<TokenIndex>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters required to format a value as a string.
    /// </summary>
    public const int MaxSerializedCharLength = 21;

    private const char RangeSeparator = '-';
    private const char SpanSeparator = '.';

    /// <summary>
    /// Initializes a new <see cref="TokenIndex"/>.
    /// </summary>
    /// <param name="start">The index, starting at 1, of the token (or the start of a multi-word range).</param>
    /// <param name="end">The end of a multi-word range; equal to <paramref name="start"/> for single tokens.</param>
    /// <param name="emptyId">For empty nodes, the second number in the ID.</param>
    public TokenIndex(int start, int? end = null, int? emptyId = null)
    {
        Guard.IsGreaterThan(start, 0);

        if (end is not null)
        {
            Guard.IsGreaterThanOrEqualTo(end.Value, start);
        }

        if (end is not null && emptyId is not null && end.Value != start)
        {
            ThrowHelper.ThrowArgumentException(
                "Cannot have both a range and an emptyId", nameof(end));
        }

        if (emptyId is not null)
        {
            Guard.IsGreaterThan(emptyId.Value, 0);
        }

        Start = start;
        End = end ?? start;
        EmptyId = emptyId;
    }

    /// <summary>
    /// Index, starting at 1, of the token in the sentence, or the beginning of a range, or 0 if empty node.
    /// </summary>
    public int Start { get; init; }

    /// <summary>
    /// In the case of a multiword token (e.g. 1-2 or 3-5), End identifies
    /// the end of the range. For a single token, End is the same as Start.
    /// </summary>
    public int End { get; init; }

    /// <summary>
    /// In the case of an empty node ID (e.g. 1.2 or 3.5), the EmptyId is
    /// the second number in the ID.
    /// </summary>
    public int? EmptyId { get; init; }

    /// <summary>
    /// <see langword="true"/> if this is a multi-word token index (i.e. <see cref="End"/> is greater than <see cref="Start"/>).
    /// </summary>
    public bool IsMultiwordIndex => End > Start;

    /// <summary>
    /// <see langword="true"/> if this index identifies an empty node.
    /// </summary>
    public bool IsEmptyNode => EmptyId is not null;

    /// <summary>
    /// Tokens are sorted by their ID. Between tokens with the same ID, the order is: multi-word
    /// (by span id), ID and them empty nodes bu sub ID.
    /// </summary>
    public int CompareTo(TokenIndex other)
    {
        if (Start != other.Start)
        {
            return Start.CompareTo(other.Start);
        }

        if (End != other.End)
        {
            return End.CompareTo(other.End);
        }

        if (EmptyId != other.EmptyId)
        {
            return EmptyId.GetValueOrDefault().CompareTo(other.EmptyId.GetValueOrDefault());
        }

        return 0;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is TokenIndex ti && Equals(ti);
    }

    /// <inheritdoc/>
    public bool Equals(TokenIndex other)
    {
        return CompareTo(other) == 0;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End, EmptyId);
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="li"/> is greater than <paramref name="ri"/>.</summary>
    public static bool operator >(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) > 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="li"/> is less than <paramref name="ri"/>.</summary>
    public static bool operator <(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) < 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="li"/> is greater than or equal to <paramref name="ri"/>.</summary>
    public static bool operator >=(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) >= 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="li"/> is less than or equal to <paramref name="ri"/>.</summary>
    public static bool operator <=(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) <= 0;
    }

    /// <summary>Returns <see langword="true"/> if the two values are equal.</summary>
    public static bool operator ==(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) == 0;
    }

    /// <summary>Returns <see langword="true"/> if the two values are not equal.</summary>
    public static bool operator !=(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) != 0;
    }

    /// <summary>
    /// Returns whether the index is the same or in the range of the span in case of a multi word index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsInRange(int index)
    {
        if (!IsMultiwordIndex)
        {
            return index == Start;
        }

        return index >= Start && index <= End;
    }

    /// <summary>
    /// Tries to format the value into the provided destination buffer.
    /// </summary>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (Start.TryFormat(destination, out charsWritten, default, CultureInfo.InvariantCulture))
        {
            if (IsMultiwordIndex || IsEmptyNode)
            {
                if (IsMultiwordIndex)
                {
                    if (destination.Length > charsWritten)
                    {
                        destination[charsWritten++] = RangeSeparator;

                        if (End.TryFormat(destination[charsWritten..],
                            out var cwSpanId, default, CultureInfo.InvariantCulture))
                        {
                            charsWritten += cwSpanId;
                            return true;
                        }

                        charsWritten = 0;
                        return false;
                    }
                }
                else if (IsEmptyNode)
                {
                    if (destination.Length > charsWritten)
                    {
                        destination[charsWritten++] = SpanSeparator;

                        if (EmptyId!.Value.TryFormat(destination[charsWritten..],
                            out var cwSubId, default, CultureInfo.InvariantCulture))
                        {
                            charsWritten += cwSubId;
                            return true;
                        }

                        charsWritten = 0;
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        charsWritten = 0;
        return false;
    }

    internal int GetCharCount()
    {
        var count = GetInvariantCharCount(Start);

        if (IsMultiwordIndex)
        {
            count += 1 + GetInvariantCharCount(End);
        }
        else if (IsEmptyNode)
        {
            count += 1 + GetInvariantCharCount(EmptyId!.Value);
        }

        return count;
    }

    private static int GetInvariantCharCount(int value)
    {
        Span<char> buffer = stackalloc char[11];
        _ = value.TryFormat(buffer, out var charsWritten, default, CultureInfo.InvariantCulture);
        return charsWritten;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(default, default);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxSerializedCharLength];

        if (TryFormat(buffer, out var charsWritten, default, default))
        {
            return buffer[..charsWritten].ToString();
        }

        return ThrowHelper.ThrowFormatException<string>(
            "Could not format token identifier to string.");
    }

    /// <summary>
    /// Parses a <see cref="TokenIndex"/> from the specified character span.
    /// </summary>
    public static TokenIndex Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static TokenIndex Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TokenIndex>(
            $"Cannot parse '{s}' as {nameof(TokenIndex)}.");
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out TokenIndex result)
    {
        if (s.Contains(SpanSeparator))
        {
            Span<Range> ranges = stackalloc Range[2];

            var r = s.Split(ranges, SpanSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (r < 2)
            {
                goto Fail;
            }

            if (int.TryParse(s[ranges[0]], CultureInfo.InvariantCulture, out var id)
                && int.TryParse(s[ranges[1]], CultureInfo.InvariantCulture, out var emptyId))
            {
                result = new(id, emptyId: emptyId);
                return true;
            }
        }
        else if (s.Contains(RangeSeparator))
        {
            Span<Range> ranges = stackalloc Range[2];

            var r = s.Split(ranges, RangeSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (r < 2)
            {
                goto Fail;
            }

            if (int.TryParse(s[ranges[0]], CultureInfo.InvariantCulture, out var id)
                && int.TryParse(s[ranges[1]], CultureInfo.InvariantCulture, out var spanId))
            {
                result = new(id, end: spanId);
                return true;
            }
        }
        else
        {
            if (int.TryParse(s, CultureInfo.InvariantCulture, out var id))
            {
                result = new(id);
                return true;
            }
        }

    Fail:
        result = default;
        return false;
    }

    /// <summary>
    /// Parses a <see cref="TokenIndex"/> from the specified string.
    /// </summary>
    public static TokenIndex Parse(string s)
    {
        return Parse(s, default);
    }

    /// <summary>
    /// Parses a <see cref="TokenIndex"/> from the specified string using the invariant culture.
    /// </summary>
    public static TokenIndex ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static TokenIndex Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s.AsSpan(), provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TokenIndex>(
            $"Cannot parse '{s}' as {nameof(TokenIndex)}.");
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out TokenIndex result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Creates a <see cref="TokenIndex"/> for a single token at the specified position.
    /// </summary>
    public static TokenIndex FromInt32(int index)
    {
        return new(index);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Start);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(End);
        var h2 = RepeatableHash64Provider.Default.GetRepeatableHashCode(EmptyId ?? -1);
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to a <see cref="TokenIndex"/>.
    /// </summary>
    public static explicit operator TokenIndex(int index)
    {
        return FromInt32(index);
    }
}
