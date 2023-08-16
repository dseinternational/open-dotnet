// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
///     A tag consisting only of ASCII letters or digits or dashes, forward slashes,
///     or colons ('-', '/', ':', with only letters or digits at beginning or end).
///     Tags are designed to be useablein URIs without escaping. A related, but
///     more permissive, type is <see cref="Label" />.
/// </summary>
/// <remarks>
///     See: <see href="https://www.ietf.org/rfc/rfc3986.html">RFC 3986: Uniform Resource
///     Identifier (URI): Generic Syntax</see>
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringTagConverter))]
public readonly record struct Tag
    : IComparable<Tag>,
      ISpanFormattable,
      ISpanParsable<Tag>,
      IEquatable<Tag>
{
    public const int MinLength = 2;
    public const int MaxLength = 120;

    public static readonly Tag Empty;

    /// <remarks>
    /// <c>null</c> if <see cref="Empty"/>.
    /// </remarks>
    private readonly string? _tag;

    public Tag(string tag) : this(tag, false)
    {
    }

    public Tag(ReadOnlySpan<char> tag) : this(tag, false)
    {
    }

    private Tag(string tag, bool skipValidation)
    {
        tag = tag.Trim();

        if (!skipValidation)
        {
            EnsureValidTag(tag.AsSpan());
        }

        _tag = string.IsInterned(tag) ?? TagStringPool.Shared.GetOrAdd(tag);
    }

    private Tag(ReadOnlySpan<char> tag, bool skipValidation)
    {
        tag = tag.Trim();

        if (!skipValidation)
        {
            EnsureValidTag(tag);
        }

        _tag = TagStringPool.Shared.GetOrAdd(tag);
    }

    public static void EnsureValidTag(ReadOnlySpan<char> tag)
    {
        if (!IsValidTag(tag))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tag), $"Invalid {nameof(Tag)}: {tag}");
        }
    }

    public ReadOnlySpan<char> AsSpan() => _tag.AsSpan();

    public bool Equals(Tag other) => string.Equals(_tag, other._tag, StringComparison.Ordinal);

    public override int GetHashCode() => string.GetHashCode(_tag.AsSpan(), StringComparison.Ordinal);

    public static bool IsValidTag(ReadOnlySpan<char> tag)
    {
        return tag.Length is >= MinLength and <= MaxLength
            && char.IsAsciiLetterOrDigit(tag[0])
            && char.IsAsciiLetterOrDigit(tag[^1])
            && tag.All(IsValidInnerChar);
    }

    private static bool IsValidInnerChar(char c)
        => char.IsAsciiLetterOrDigit(c) || c == '-' || c == '/' || c == ':';

    public int CompareTo(Tag other) => string.CompareOrdinal(_tag, other._tag);

    public bool Equals(string? other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(string? other, StringComparison comparison) => other is not null
        && (_tag is null && other.Length == 0 || string.Equals(_tag, other, comparison));

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _tag.AsSpan().SequenceEqual(other);

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, default, null);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_tag is null || _tag.Length == 0)
        {
            charsWritten = 0;
            return true;
        }

        if (destination.Length < _tag.Length)
        {
            charsWritten = 0;
            return false;
        }

        _tag.CopyTo(destination);
        charsWritten = _tag.Length;

        return true;
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider) => _tag ?? string.Empty;

    public static Tag Parse(string s) => Parse(s, null);

    public static Tag Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);

        return TryParse(s, provider, out var tag)
            ? tag
            : ThrowHelper.ThrowFormatException<Tag>($"Could not parse {nameof(Tag)} with value: {s}");
    }

    public static Tag Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static Tag Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var tag)
            ? tag
            : ThrowHelper.ThrowFormatException<Tag>($"Could not parse {nameof(Tag)} with value: {s}");
    }

    public static bool TryParse(string? s, out Tag result) => TryParse(s, null, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Tag result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out Tag result) => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Tag result)
    {
        s = s.Trim();

        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (!IsValidTag(s))
        {
            result = default;
            return false;
        }

        result = new Tag(s, skipValidation: true);
        return true;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Tag(string tag) => new(tag);

    public static explicit operator string(Tag tag) => tag.ToString();

    public static explicit operator ReadOnlySpan<char>(Tag tag) => tag._tag;

    public static explicit operator ReadOnlyMemory<char>(Tag tag) => tag._tag.AsMemory();

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static class TagStringPool
    {
        public static readonly StringPool Shared = new();
    }

    public static bool operator <(Tag left, Tag right) => left.CompareTo(right) < 0;

    public static bool operator <=(Tag left, Tag right) => left.CompareTo(right) <= 0;

    public static bool operator >(Tag left, Tag right) => left.CompareTo(right) > 0;

    public static bool operator >=(Tag left, Tag right) => left.CompareTo(right) >= 0;
}
