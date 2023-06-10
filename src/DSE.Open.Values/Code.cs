// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) characters used to identify something.
/// </summary>
[JsonConverter(typeof(JsonStringCodeConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct Code
    : IComparable<Code>,
      ISpanFormattable,
      ISpanParsable<Code>,
      IEquatable<Code>
{
    public const int MaxLength = 32;

    public static readonly Code Empty;

    private readonly string? _code;

    public Code(string code)
    {
        Guard.IsNotNull(code);

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

    public ReadOnlySpan<char> AsSpan() => _code.AsSpan();

    public ReadOnlyMemory<char> AsMemory() => _code.AsMemory();

    public int CompareTo(Code other) => CompareTo(other, StringComparison.CurrentCulture);

    public int CompareTo(Code other, StringComparison comparison) => string.Compare(_code, other._code, comparison);

    public int CompareOrdinal(Code other) => string.CompareOrdinal(_code, other._code);

    public int CompareOrdinalIgnoreCase(Code other) => CompareTo(other, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => string.GetHashCode(_code, StringComparison.Ordinal);

    public bool Equals(Code other) => Equals(other._code.AsSpan());

    public bool Equals(Code other, StringComparison comparison) => Equals(other._code, comparison);

    public bool Equals(string? other) => other != null && Equals(other.AsSpan());

    public bool Equals(string? other, StringComparison comparison) => other != null
        && string.Equals(_code, other, comparison);

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _code.AsSpan().SequenceEqual(other);

    public static explicit operator Code(string code) => FromString(code);

    public static Code FromString(string code) => new(code);

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Code(short code) => FromNumber(code);

    public static explicit operator Code(int code) => FromNumber(code);

    public static explicit operator Code(long code) => FromNumber(code);

    public static explicit operator Code(ushort code) => FromNumber(code);

    public static explicit operator Code(uint code) => FromNumber(code);

    public static explicit operator Code(ulong code) => FromNumber(code);

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static Code FromNumber<T>(T code) where T : ISpanFormattable
    {
        Span<char> span = stackalloc char[MaxLength];

        return code.TryFormat(span, out var charsWritten, null, CultureInfo.InvariantCulture)
            ? new Code(span[..charsWritten], true)
            : ThrowHelper.ThrowFormatException<Code>(
                $"The maximum length of a {nameof(Code)} is {MaxLength} characters");
    }

    public static explicit operator string(Code code) => code.ToString();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyMemory<char>(Code code) => code._code.AsMemory();

    public static explicit operator ReadOnlySpan<char>(Code code) => code._code;

#pragma warning restore CA2225 // Operator overloads have named alternates

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _code is null
            ? string.Empty
            : CodeStringPool.Shared.GetOrAdd(_code);
    }

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

    public static Code Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Code result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static Code Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Code>($"Could not parse {nameof(Code)}");
    }

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
            if (s.Length > MaxLength)
            {
                result = default;
                return false;
            }
        }

        result = new Code(s, true);
        return true;
    }

    public static bool operator <(Code left, Code right) => left.CompareTo(right, StringComparison.CurrentCulture) < 0;

    public static bool operator <=(Code left, Code right) => left.CompareTo(right, StringComparison.CurrentCulture) <= 0;

    public static bool operator >(Code left, Code right) => left.CompareTo(right, StringComparison.CurrentCulture) > 0;

    public static bool operator >=(Code left, Code right) => left.CompareTo(right, StringComparison.CurrentCulture) >= 0;
}
