// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json;

namespace DSE.Open.Diagnostics;

[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringDiagnosticCodeConverter))]
public readonly record struct DiagnosticCode
    : IComparable<DiagnosticCode>,
      ISpanParsable<DiagnosticCode>,
      ISpanFormattable
{
    public const int MaxPrefixLength = 4;
    public const int MinPrefixLength = 3;

    public const int MaxDigitsLength = 8;
    public const int MinDigitsLength = 6;

    public const int MaxLength = MaxPrefixLength + MaxDigitsLength;    // ABCDE12345678
    public const int MinLength = MinPrefixLength + MinDigitsLength;    // ABC123456

    public static readonly DiagnosticCode Empty;

    /// <remarks>
    /// <c>null</c> if <see cref="Empty"/>.
    /// </remarks>
    private readonly string? _code;

    /// <summary>
    /// Initialises an <see cref="DiagnosticCode"/> from a string.
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="code"/> is longer than
    /// <see cref="MaxLength"/> or shorter that <see cref="MinLength"/> or if <paramref name="code"/>
    /// does not start with 3 or 4 upper case ASCII characters and then 6-8 ASCII digits.</exception>
    /// <remarks>
    /// If the same set of characters is available as an interned string, then the referenced string (if
    /// not the same instance) is discarded and the interned instance referenced. Otherwise, we check to
    /// see if a string with the same set of characters is pooled and use the pooled instance if available,
    /// otherwise pooling this instance.
    /// </remarks>
    public DiagnosticCode(string code) : this(code, false)
    {
    }

    /// <summary>
    /// Initialises an <see cref="DiagnosticCode"/> from a span of characters.
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="code"/> is longer than
    /// <see cref="MaxLength"/> or shorter that <see cref="MinLength"/> or if <paramref name="code"/>
    /// does not start with 3 or 4 upper case ASCII characters and then 6-8 ASCII digits.</exception>
    /// <remarks>
    /// If the same span of characters is already pooled, then no new copy is made. Otherwise a copy of
    /// the characters is made and pooled.
    /// </remarks>
    public DiagnosticCode(ReadOnlySpan<char> code) : this(code, false)
    {
    }

    private DiagnosticCode(string code, bool skipChecks)
    {
        Guard.IsNotNull(code);

        if (!skipChecks)
        {
            EnsureValidCode(code);
        }

        _code = string.IsInterned(code) ?? CodeStringPool.Shared.GetOrAdd(code);
    }

    private DiagnosticCode(ReadOnlySpan<char> code, bool skipChecks)
    {
        if (!skipChecks)
        {
            EnsureValidCode(code);
        }

        _code = CodeStringPool.Shared.GetOrAdd(code);
    }

    public static bool IsValid(ReadOnlySpan<char> code)
    {
        return !(code.IsEmpty || code.Length > MaxLength || code.Length < MinLength)
            && char.IsAsciiLetterUpper(code[0])
            && char.IsAsciiLetterUpper(code[1])
            && char.IsAsciiLetterUpper(code[2])
            && (char.IsAsciiLetterUpper(code[3]) && code.Length >= MaxPrefixLength + MinDigitsLength || char.IsAsciiDigit(code[3]))
            && code[4..].ContainsOnlyAsciiDigits();
    }

    private static void EnsureValidCode(ReadOnlySpan<char> code)
    {
        if (!IsValid(code))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(code), $"Invalid {nameof(DiagnosticCode)}: {code}");
        }
    }

    public int Length => _code?.Length ?? 0;

    public ReadOnlySpan<char> AsSpan() => _code.AsSpan();

    public int CompareTo(DiagnosticCode other) => StringComparer.Ordinal.Compare(_code, other._code);

    public static bool Equals(DiagnosticCode a, DiagnosticCode b) => Equals(a, b._code.AsSpan());

    public static bool Equals(DiagnosticCode a, string b) => Equals(a, b.AsSpan());

    public static bool Equals(DiagnosticCode a, ReadOnlySpan<char> b) => a._code.AsSpan().SequenceEqual(b);

    public bool Equals(DiagnosticCode other) => Equals(this, other);

    public bool Equals(string other) => Equals(this, other);

    public bool Equals(ReadOnlySpan<char> other) => Equals(this, other);

    public static bool operator ==(DiagnosticCode left, string right) => left.Equals(right);

    public static bool operator !=(DiagnosticCode left, string right) => !(left == right);

    public override int GetHashCode() => string.GetHashCode(_code.AsSpan(), StringComparison.Ordinal);

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider) => _code ?? string.Empty;

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (_code is null)
        {
            charsWritten = 0;
            return true;
        }

        if (_code.TryCopyTo(destination))
        {
            charsWritten = _code.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static DiagnosticCode Parse(string s) => Parse(s, null);

    public static DiagnosticCode Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static DiagnosticCode Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static DiagnosticCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<DiagnosticCode>($"'{s}' is not a valid {nameof(DiagnosticCode)}.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out DiagnosticCode result)
        => TryParse(s, null, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DiagnosticCode result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out DiagnosticCode result) => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DiagnosticCode result)
    {
        if (IsValid(s))
        {
            result = new DiagnosticCode(s, skipChecks: true);
            return true;
        }

        result = Empty;
        return false;
    }

    public static implicit operator string(DiagnosticCode code) => code.ToString();

    public static implicit operator DiagnosticCode(string code) => FromString(code);

    public static DiagnosticCode FromString(string code) => new(code);

    public static bool operator <(DiagnosticCode left, DiagnosticCode right) => left.CompareTo(right) < 0;

    public static bool operator <=(DiagnosticCode left, DiagnosticCode right) => left.CompareTo(right) <= 0;

    public static bool operator >(DiagnosticCode left, DiagnosticCode right) => left.CompareTo(right) > 0;

    public static bool operator >=(DiagnosticCode left, DiagnosticCode right) => left.CompareTo(right) >= 0;

}
