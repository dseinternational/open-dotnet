// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Diagnostics;

/// <summary>
/// A short alphanumeric identifier for a diagnostic, consisting of 3 or 4 upper case ASCII
/// letters followed by 6 to 8 ASCII digits (for example, <c>ABC123456</c>).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringDiagnosticCodeConverter))]
public readonly struct DiagnosticCode
    : IComparable<DiagnosticCode>,
      IEquatable<DiagnosticCode>,
      ISpanParsable<DiagnosticCode>,
      ISpanFormattable
{
    /// <summary>
    /// The maximum number of characters in the alphabetic prefix portion of a code.
    /// </summary>
    public const int MaxPrefixLength = 4;

    /// <summary>
    /// The minimum number of characters in the alphabetic prefix portion of a code.
    /// </summary>
    public const int MinPrefixLength = 3;

    /// <summary>
    /// The maximum number of digits in the numeric portion of a code.
    /// </summary>
    public const int MaxDigitsLength = 8;

    /// <summary>
    /// The minimum number of digits in the numeric portion of a code.
    /// </summary>
    public const int MinDigitsLength = 6;

    /// <summary>
    /// The maximum length of a <see cref="DiagnosticCode"/>.
    /// </summary>
    public const int MaxLength = MaxPrefixLength + MaxDigitsLength;    // ABCDE12345678

    /// <summary>
    /// The minimum length of a <see cref="DiagnosticCode"/>.
    /// </summary>
    public const int MinLength = MinPrefixLength + MinDigitsLength;    // ABC123456

    /// <summary>
    /// An empty (default) <see cref="DiagnosticCode"/>.
    /// </summary>
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
        ArgumentNullException.ThrowIfNull(code);

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

    /// <summary>
    /// Determines whether the specified span of characters is a valid <see cref="DiagnosticCode"/>.
    /// </summary>
    public static bool IsValid(ReadOnlySpan<char> code)
    {
        return !(code.IsEmpty || code.Length > MaxLength || code.Length < MinLength)
            && char.IsAsciiLetterUpper(code[0])
            && char.IsAsciiLetterUpper(code[1])
            && char.IsAsciiLetterUpper(code[2])
            && ((char.IsAsciiLetterUpper(code[3]) && code.Length >= MaxPrefixLength + MinDigitsLength) || char.IsAsciiDigit(code[3]))
            && code[4..].ContainsOnlyAsciiDigits();
    }

    private static void EnsureValidCode(ReadOnlySpan<char> code)
    {
        if (!IsValid(code))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(code), $"Invalid {nameof(DiagnosticCode)}: {code}");
        }
    }

    /// <summary>
    /// Gets the number of characters in the code.
    /// </summary>
    public int Length => _code?.Length ?? 0;

    /// <summary>
    /// Returns a read-only span over the underlying characters of the code.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _code.AsSpan();
    }

    /// <inheritdoc/>
    public int CompareTo(DiagnosticCode other)
    {
        return StringComparer.Ordinal.Compare(_code, other._code);
    }

    /// <summary>
    /// Determines whether two <see cref="DiagnosticCode"/> values represent the same code.
    /// </summary>
    public static bool Equals(DiagnosticCode a, DiagnosticCode b)
    {
        return Equals(a, b._code.AsSpan());
    }

    /// <summary>
    /// Determines whether the specified <see cref="DiagnosticCode"/> equals the specified string.
    /// </summary>
    public static bool Equals(DiagnosticCode a, string b)
    {
        return Equals(a, b.AsSpan());
    }

    /// <summary>
    /// Determines whether the specified <see cref="DiagnosticCode"/> equals the specified span of characters.
    /// </summary>
    public static bool Equals(DiagnosticCode a, ReadOnlySpan<char> b)
    {
        return a._code.AsSpan().SequenceEqual(b);
    }

    /// <inheritdoc/>
    public bool Equals(DiagnosticCode other)
    {
        return Equals(this, other);
    }

    /// <summary>
    /// Determines whether this code equals the specified string.
    /// </summary>
    public bool Equals(string other)
    {
        return Equals(this, other);
    }

    /// <summary>
    /// Determines whether this code equals the specified span of characters.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return Equals(this, other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is DiagnosticCode other && Equals(other);
    }

    /// <summary>
    /// Determines whether two <see cref="DiagnosticCode"/> values are equal.
    /// </summary>
    public static bool operator ==(DiagnosticCode left, DiagnosticCode right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two <see cref="DiagnosticCode"/> values are not equal.
    /// </summary>
    public static bool operator !=(DiagnosticCode left, DiagnosticCode right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the specified <see cref="DiagnosticCode"/> is equal to the specified string.
    /// </summary>
    public static bool operator ==(DiagnosticCode left, string right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether the specified <see cref="DiagnosticCode"/> is not equal to the specified string.
    /// </summary>
    public static bool operator !=(DiagnosticCode left, string right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(_code.AsSpan(), StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _code ?? string.Empty;
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Parses a string into a <see cref="DiagnosticCode"/>.
    /// </summary>
    public static DiagnosticCode Parse(string s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a string into a <see cref="DiagnosticCode"/> using <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static DiagnosticCode ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static DiagnosticCode Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="DiagnosticCode"/>.
    /// </summary>
    public static DiagnosticCode Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="DiagnosticCode"/> using
    /// <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static DiagnosticCode ParseInvariant(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static DiagnosticCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<DiagnosticCode>($"'{s}' is not a valid {nameof(DiagnosticCode)}.");
    }

    /// <summary>
    /// Attempts to parse the specified string into a <see cref="DiagnosticCode"/>.
    /// </summary>
    public static bool TryParse([NotNullWhen(true)] string? s, out DiagnosticCode result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DiagnosticCode result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Attempts to parse the specified span of characters into a <see cref="DiagnosticCode"/>.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> s, out DiagnosticCode result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DiagnosticCode result)
    {
        if (IsValid(s))
        {
            result = new(s, skipChecks: true);
            return true;
        }

        result = Empty;
        return false;
    }

    /// <summary>
    /// Implicitly converts the specified <see cref="DiagnosticCode"/> to its string representation.
    /// </summary>
    public static implicit operator string(DiagnosticCode code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Implicitly converts the specified string to a <see cref="DiagnosticCode"/>.
    /// </summary>
    public static implicit operator DiagnosticCode(string code)
    {
        return FromString(code);
    }

    /// <summary>
    /// Creates a <see cref="DiagnosticCode"/> from the specified string.
    /// </summary>
    public static DiagnosticCode FromString(string code)
    {
        return new(code);
    }

    /// <summary>
    /// Determines whether one <see cref="DiagnosticCode"/> orders before another.
    /// </summary>
    public static bool operator <(DiagnosticCode left, DiagnosticCode right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines whether one <see cref="DiagnosticCode"/> orders before or is equal to another.
    /// </summary>
    public static bool operator <=(DiagnosticCode left, DiagnosticCode right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Determines whether one <see cref="DiagnosticCode"/> orders after another.
    /// </summary>
    public static bool operator >(DiagnosticCode left, DiagnosticCode right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines whether one <see cref="DiagnosticCode"/> orders after or is equal to another.
    /// </summary>
    public static bool operator >=(DiagnosticCode left, DiagnosticCode right)
    {
        return left.CompareTo(right) >= 0;
    }
}
