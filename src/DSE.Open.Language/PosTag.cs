// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Language;

[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringPosTagConverter))]
public readonly record struct PosTag
    : IComparable<PosTag>,
      ISpanFormattable,
      ISpanParsable<PosTag>,
      IEquatable<string>,
      IEquatable<ReadOnlyMemory<char>>
{
    public const int MaxLength = 32;

    public static readonly PosTag Empty;

    private readonly string? _code;

    public PosTag(string code)
    {
        Guard.IsNotNull(code);

        if (code.Length > MaxLength)
        {
            ThrowHelper.ThrowFormatException($"The maximum length of a {nameof(PosTag)} is {MaxLength} characters.");
            return; // Unreachable
        }

        _code = string.IsInterned(code) ?? PosTagStringPool.Shared.GetOrAdd(code);
    }

    private PosTag(string code, bool valid)
    {
        _code = string.IsInterned(code) ?? PosTagStringPool.Shared.GetOrAdd(code);
    }

    private PosTag(ReadOnlySpan<char> code, bool valid)
    {
        _code = PosTagStringPool.Shared.GetOrAdd(code);
    }

    public ReadOnlySpan<char> AsSpan() => _code.AsSpan();

    public int CompareTo(PosTag other) => string.CompareOrdinal(_code, other._code);

    public int CompareTo(PosTag other, StringComparison comparison) => string.Compare(_code, other._code, comparison);

    public int CompareOrdinal(PosTag other) => string.CompareOrdinal(_code, other._code);

    public int CompareOrdinalIgnoreCase(PosTag other) => CompareTo(other, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => string.GetHashCode(_code, StringComparison.Ordinal);

    public bool Equals(PosTag other) => Equals(other._code.AsSpan());

    public bool Equals(PosTag other, StringComparison comparison) => Equals(other._code, comparison);

    public bool Equals(string? other) => other != null && Equals(other.AsSpan());

    public bool Equals(string? other, StringComparison comparison) => other != null
        && string.Equals(_code, other, comparison);

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _code.AsSpan().SequenceEqual(other);

    public static explicit operator PosTag(string code) => new(code);

    public static explicit operator string(PosTag code) => code.ToString();

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _code is null
            ? string.Empty
            : _code;
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

    public static PosTag Parse(string s) => Parse(s, null);

    public static PosTag Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out PosTag result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static PosTag Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<PosTag>($"Could not parse {nameof(PosTag)}");
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out PosTag result)
    {
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

        result = new PosTag(s, true);
        return true;
    }

    public static PosTag ToPosTag(PosTag left, PosTag right) => throw new NotImplementedException();

    public static bool operator <(PosTag left, PosTag right) => left.CompareTo(right, StringComparison.Ordinal) < 0;

    public static bool operator <=(PosTag left, PosTag right) => left.CompareTo(right, StringComparison.Ordinal) <= 0;

    public static bool operator >(PosTag left, PosTag right) => left.CompareTo(right, StringComparison.Ordinal) > 0;

    public static bool operator >=(PosTag left, PosTag right) => left.CompareTo(right, StringComparison.Ordinal) >= 0;

}
