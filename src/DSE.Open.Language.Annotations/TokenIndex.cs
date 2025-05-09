// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Language.Annotations.Serialization;

namespace DSE.Open.Language.Annotations;

// Builds on: https://github.com/ArthurDevNL/CoNLL-U/blob/main/Conllu/Conllu/TokenIdentifier.cs

[JsonConverter(typeof(JsonStringTokenIdentifierConverter))]
public readonly struct TokenIndex
    : IComparable<TokenIndex>,
      IEquatable<TokenIndex>,
      ISpanFormattable,
      ISpanParsable<TokenIndex>,
      IRepeatableHash64
{
    public const int MaxSerializedCharLength = 8;

    private const char RangeSeparator = '-';
    private const char SpanSeparator = '.';

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

    public bool IsMultiwordIndex => End > Start;

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

    public override bool Equals(object? obj)
    {
        return obj is TokenIndex ti && Equals(ti);
    }

    public bool Equals(TokenIndex other)
    {
        return CompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End, EmptyId);
    }

    public static bool operator >(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) > 0;
    }

    public static bool operator <(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) < 0;
    }

    public static bool operator >=(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) >= 0;
    }

    public static bool operator <=(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) <= 0;
    }

    public static bool operator ==(TokenIndex li, TokenIndex ri)
    {
        return li.CompareTo(ri) == 0;
    }

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

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

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
                    }
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

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

    public static TokenIndex Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static TokenIndex Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        return ThrowHelper.ThrowFormatException<TokenIndex>(
            $"Cannot parse '{s}' as {nameof(TokenIndex)}.");
    }

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

    public static TokenIndex Parse(string s)
    {
        return Parse(s, default);
    }

    public static TokenIndex ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

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

    public static TokenIndex FromInt32(int index)
    {
        return new(index);
    }

    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Start);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(End);
        var h2 = RepeatableHash64Provider.Default.GetRepeatableHashCode(EmptyId ?? -1);
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }

    public static explicit operator TokenIndex(int index)
    {
        return FromInt32(index);
    }
}
