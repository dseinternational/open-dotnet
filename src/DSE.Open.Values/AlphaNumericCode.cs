// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters or digits used to identify something.
/// </summary>
/// <remarks>
/// This value type is designed to minimise memory use, using interned or pooled
/// instances of codes - assuming that often the same codes will be used.
/// </remarks>
[JsonConverter(typeof(JsonStringAlphaNumericCodeConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct AlphaNumericCode : IComparable<AlphaNumericCode>, ISpanParsable<AlphaNumericCode>,
    ISpanFormattable, IEquatable<string>, IEquatable<ReadOnlyMemory<char>> // ISpanProvider<char>, 
{
    public const int MaxLength = 32;

    /// <remarks>
    /// <c>null</c> if the code is empty.
    /// </remarks>
    private readonly string? _code;

    /// <summary>
    /// Initialises an <see cref="AlphaNumericCode"/> from a string.
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="code"/> is longer than
    /// <see cref="MaxLength"/> or if <paramref name="code"/> contains characters that are not ASCII
    /// letters or digits.</exception>
    /// <remarks>
    /// If the same set of characters is available as an interned string, then the referenced string (if
    /// not the same instance) is discarded and the interned instance referenced. Otherwise, we check to
    /// see if a string with the same set of characters is pooled and use the pooled instance if available,
    /// otherwise pooling this instance.
    /// </remarks>
    public AlphaNumericCode(string code) : this(code, false)
    {
    }

    /// <summary>
    /// Initialises an <see cref="AlphaNumericCode"/> from a span of characters.
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="code"/> is longer than
    /// <see cref="MaxLength"/> or if <paramref name="code"/> contains characters that are not ASCII
    /// letters or digits.</exception>
    /// <remarks>
    /// If the same span of characters is already pooled, then no new copy is made. Otherwise a copy of
    /// the characters is made and pooled.
    /// </remarks>
    public AlphaNumericCode(ReadOnlySpan<char> code) : this(code, false)
    {
    }

    private AlphaNumericCode(string code, bool skipChecks)
    {
        Guard.IsNotNull(code);

        code = code.Trim();

        if (!skipChecks)
        {
            EnsureIsValid(code);
        }

        _code = string.IsInterned(code) ?? CodeStringPool.Shared.GetOrAdd(code);
    }

    private AlphaNumericCode(ReadOnlySpan<char> code, bool skipChecks)
    {
        code = code.Trim();

        if (!skipChecks)
        {
            EnsureIsValid(code);
        }

        _code = CodeStringPool.Shared.GetOrAdd(code);
    }

    public static bool IsValid(ReadOnlySpan<char> code)
        => code is { IsEmpty: false, Length: <= MaxLength } && code.ContainsOnlyAsciiLettersOrDigits();

    private static void EnsureIsValid(ReadOnlySpan<char> code)
    {
        if (code.Length > MaxLength)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(code),
                $"The maximum length of an {nameof(AlphaNumericCode)} is {MaxLength} characters.");
        }

        if (!code.ContainsOnlyAsciiLettersOrDigits())
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(code),
                $"An {nameof(AlphaNumericCode)} may only include ASCII letters.");
        }
    }

    public int Length => _code?.Length ?? 0;

    public ReadOnlySpan<char> AsSpan() => _code.AsSpan();

    public int CompareTo(AlphaNumericCode other) => CompareTo(other, StringComparison.CurrentCulture);

    public int CompareTo(AlphaNumericCode other, StringComparison comparison)
        => string.Compare(_code, other._code, comparison);

    public bool Equals(AlphaNumericCode other) => Equals(other._code.AsSpan());

    public bool Equals(string? other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(string? other, StringComparison comparison) => other is not null
        && (_code is null && other.Length == 0 || string.Equals(_code, other, comparison));

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _code.AsSpan().SequenceEqual(other);

    public static bool operator ==(AlphaNumericCode left, string right) => left.Equals(right, StringComparison.Ordinal);

    public static bool operator !=(AlphaNumericCode left, string right) => !(left == right);

    public override int GetHashCode() => string.GetHashCode(_code.AsSpan(), StringComparison.Ordinal);

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider) => _code ?? string.Empty;

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
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

    public static AlphaNumericCode Parse(string s) => Parse(s, null);

    public static AlphaNumericCode Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static AlphaNumericCode Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static AlphaNumericCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<AlphaNumericCode>(
                $"'{s.ToString()}' is not a valid {nameof(AlphaNumericCode)}.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out AlphaNumericCode result)
        => TryParse(s, null, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AlphaNumericCode result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out AlphaNumericCode result) => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out AlphaNumericCode result)
    {
        if (s.Length <= MaxLength && s.AllAreAsciiLetterOrDigit())
        {
            result = new AlphaNumericCode(s, skipChecks: true);
            return true;
        }

        result = default;
        return false;
    }

    public static explicit operator string(AlphaNumericCode code) => code.ToString();

    private static AlphaNumericCode FromNumber<T>(T code) where T : ISpanFormattable
    {
        Span<char> buffer = stackalloc char[MaxLength];

        return code.TryFormat(buffer, out var charsWritten, default, null)
            ? Parse(buffer[..charsWritten])
            : ThrowHelper.ThrowFormatException<AlphaNumericCode>($"Could not format '{code}' to {nameof(AlphaNumericCode)}");
    }

    public static explicit operator AlphaNumericCode(string code) => FromString(code);

    public static AlphaNumericCode FromString(string code) => new(code);

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaNumericCode(byte code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(short code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(int code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(long code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(ushort code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(uint code) => FromNumber(code);

    public static explicit operator AlphaNumericCode(ulong code) => FromNumber(code);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(AlphaNumericCode left, AlphaNumericCode right) => left.CompareTo(right, StringComparison.CurrentCulture) < 0;

    public static bool operator <=(AlphaNumericCode left, AlphaNumericCode right) => left.CompareTo(right, StringComparison.CurrentCulture) <= 0;

    public static bool operator >(AlphaNumericCode left, AlphaNumericCode right) => left.CompareTo(right, StringComparison.CurrentCulture) > 0;

    public static bool operator >=(AlphaNumericCode left, AlphaNumericCode right) => left.CompareTo(right, StringComparison.CurrentCulture) >= 0;
}
