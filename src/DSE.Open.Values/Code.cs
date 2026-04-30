// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) characters used to identify something.
/// </summary>
[JsonConverter(typeof(JsonStringCodeConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct Code
    : IComparable<Code>,
      ISpanFormattable,
      ISpanParsable<Code>,
      IEquatable<Code>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters permitted in a <see cref="Code"/>.
    /// </summary>
    public const int MaxLength = 32;

    /// <summary>
    /// An empty <see cref="Code"/>.
    /// </summary>
    public static readonly Code Empty;

    private readonly string? _code;

    /// <summary>
    /// Initialises a new <see cref="Code"/> from the supplied string, after trimming whitespace.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="code"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="code"/> exceeds <see cref="MaxLength"/> characters after trimming.</exception>
    public Code(string code)
    {
        ArgumentNullException.ThrowIfNull(code);

        code = code.Trim();

        if (code.Length > MaxLength)
        {
            ThrowHelper.ThrowFormatException(
                $"The maximum length of a {nameof(Code)} is {MaxLength} characters.");
            return; // Unreachable
        }

        _code = string.IsInterned(code) ?? CodeStringPool.Shared.GetOrAdd(code);
    }

    private Code(ReadOnlySpan<char> code, bool valid)
    {
        _code = CodeStringPool.Shared.GetOrAdd(code);
    }

    /// <summary>
    /// Returns a read-only span over the underlying characters.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _code.AsSpan();
    }

    /// <summary>
    /// Returns a read-only memory region over the underlying characters.
    /// </summary>
    public ReadOnlyMemory<char> AsMemory()
    {
        return _code.AsMemory();
    }

    /// <inheritdoc/>
    public int CompareTo(Code other)
    {
        return CompareTo(other, StringComparison.CurrentCulture);
    }

    /// <summary>
    /// Compares this code with another using the specified <see cref="StringComparison"/>.
    /// </summary>
    public int CompareTo(Code other, StringComparison comparison)
    {
        return string.Compare(_code, other._code, comparison);
    }

    /// <summary>
    /// Compares this code with another using ordinal comparison.
    /// </summary>
    public int CompareOrdinal(Code other)
    {
        return string.CompareOrdinal(_code, other._code);
    }

    /// <summary>
    /// Compares this code with another using ordinal, case-insensitive comparison.
    /// </summary>
    public int CompareOrdinalIgnoreCase(Code other)
    {
        return CompareTo(other, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(_code, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public bool Equals(Code other)
    {
        return Equals(other._code.AsSpan());
    }

    /// <summary>
    /// Determines whether this code is equal to another using the specified <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(Code other, StringComparison comparison)
    {
        return Equals(other._code, comparison);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied string using ordinal comparison of its characters.
    /// </summary>
    public bool Equals(string? other)
    {
        return other != null && Equals(other.AsSpan());
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied string using the specified <see cref="StringComparison"/>.
    /// </summary>
    public bool Equals(string? other, StringComparison comparison)
    {
        return other != null
               && string.Equals(_code, other, comparison);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied character memory region using ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    /// <summary>
    /// Determines whether this code is equal to the supplied character span using ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _code.AsSpan().SequenceEqual(other);
    }

    /// <summary>
    /// Explicitly converts a string to a <see cref="Code"/>.
    /// </summary>
    public static explicit operator Code(string code)
    {
        return FromString(code);
    }

    /// <summary>
    /// Creates a <see cref="Code"/> from the supplied string.
    /// </summary>
    public static Code FromString(string code)
    {
        return new(code);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="short"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(short code)
    {
        return FromNumber(code);
    }

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(int code)
    {
        return FromNumber(code);
    }

    /// <summary>
    /// Explicitly converts a <see cref="long"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(long code)
    {
        return FromNumber(code);
    }

    /// <summary>
    /// Explicitly converts a <see cref="ushort"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(ushort code)
    {
        return FromNumber(code);
    }

    /// <summary>
    /// Explicitly converts a <see cref="uint"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(uint code)
    {
        return FromNumber(code);
    }

    /// <summary>
    /// Explicitly converts a <see cref="ulong"/> to a <see cref="Code"/> using its invariant string representation.
    /// </summary>
    public static explicit operator Code(ulong code)
    {
        return FromNumber(code);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    [SkipLocalsInit]
    private static Code FromNumber<T>(T code) where T : ISpanFormattable
    {
        Span<char> span = stackalloc char[MaxLength];

        return code.TryFormat(span, out var charsWritten, null, CultureInfo.InvariantCulture)
            ? new(span[..charsWritten], true)
            : ThrowHelper.ThrowFormatException<Code>(
                $"The maximum length of a {nameof(Code)} is {MaxLength} characters");
    }

    /// <summary>
    /// Explicitly converts a <see cref="Code"/> to its string representation.
    /// </summary>
    public static explicit operator string(Code code)
    {
        return code.ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="Code"/> to a <see cref="ReadOnlyMemory{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlyMemory<char>(Code code)
    {
        return code._code.AsMemory();
    }

    /// <summary>
    /// Explicitly converts a <see cref="Code"/> to a <see cref="ReadOnlySpan{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlySpan<char>(Code code)
    {
        return code._code;
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _code is null
            ? string.Empty
            : CodeStringPool.Shared.GetOrAdd(_code);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var span = _code.AsSpan();
        if (span.TryCopyTo(destination))
        {
            charsWritten = span.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <inheritdoc/>
    public static Code Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Code result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public static Code Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Code>($"Could not parse {nameof(Code)}");
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Code result)
    {
        s = s.Trim();

        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        if (s.Length > MaxLength)
        {
            result = default;
            return false;
        }

        result = new(s, true);
        return true;
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_code.AsSpan());
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts before <paramref name="right"/> using current-culture comparison.
    /// </summary>
    public static bool operator <(Code left, Code right)
    {
        return left.CompareTo(right, StringComparison.CurrentCulture) < 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts before or equals <paramref name="right"/> using current-culture comparison.
    /// </summary>
    public static bool operator <=(Code left, Code right)
    {
        return left.CompareTo(right, StringComparison.CurrentCulture) <= 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts after <paramref name="right"/> using current-culture comparison.
    /// </summary>
    public static bool operator >(Code left, Code right)
    {
        return left.CompareTo(right, StringComparison.CurrentCulture) > 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts after or equals <paramref name="right"/> using current-culture comparison.
    /// </summary>
    public static bool operator >=(Code left, Code right)
    {
        return left.CompareTo(right, StringComparison.CurrentCulture) >= 0;
    }
}
