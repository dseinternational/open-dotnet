// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// An integer value that may be 'not a value', missing, or not available, such as <see langword="null"/> or
/// <see cref="Na"/>.
/// </summary>
/// <typeparam name="T">The underlying integer type. <see cref="IMinMaxValue{T}.MaxValue"/>
/// is reserved as the NA sentinel and is therefore not representable as a non-Na value;
/// <see cref="MaxValue"/> is exposed as <c>T.MaxValue - 1</c>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public readonly struct NaInt<T>
    : INaNumber<NaInt<T>, T>,
      IBinaryInteger<NaInt<T>>
      where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
{
    private static readonly T s_sentinel = T.MaxValue;

    /// <summary>The reserved underlying value used to encode NA.</summary>
    public static T Sentinel => s_sentinel;

    /// <summary>Gets the canonical NA value.</summary>
    public static NaInt<T> Na => new(s_sentinel, true);

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
    private NaInt(T value, bool skipCheck)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _value = value;
    }

    /// <summary>
    /// Creates a new <see cref="NaInt{T}"/> wrapping <paramref name="value"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> equals
    /// <see cref="Sentinel"/> (which is reserved as NA).</exception>
    public NaInt(T value)
    {
        _value = (value == s_sentinel)
            ? throw new ArgumentOutOfRangeException(nameof(value),
                $"Value {s_sentinel} is reserved as NA")
            : value;
    }

    /// <summary>
    /// Creates a new <see cref="NaInt{T}"/> wrapping <paramref name="value"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> equals
    /// <see cref="Sentinel"/>.</exception>
    public static NaInt<T> FromValue(T value)
    {
        return new(value);
    }

    /// <summary>Implicitly wraps a non-NA <paramref name="value"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> equals <see cref="Sentinel"/>.</exception>
    public static implicit operator NaInt<T>(T value)
    {
        return FromValue(value);
    }

    /// <summary>Implicitly converts from <see cref="Nullable{T}"/>: <see langword="null"/> becomes <see cref="Na"/>.</summary>
    public static implicit operator NaInt<T>(T? value)
    {
        if (value is null)
        {
            return Na;
        }

        return new(value.Value);
    }

    /// <summary>Explicitly extracts the underlying <typeparamref name="T"/>.</summary>
    /// <exception cref="NaValueException">The value is NA.</exception>
    public static explicit operator T(NaInt<T> value)
    {
        return value.IsNa
            ? throw new NaValueException()
            : value._value;
    }

    /// <summary>Gets <see langword="true"/> when this value is NA.</summary>
    public bool IsNa => _value == s_sentinel;

    T INaValue<NaInt<T>, T>.Value => (T)this;

    bool INaValue.HasValue => !IsNa;

    /// <summary>
    /// Returns <see langword="true"/> when this value and <paramref name="other"/>
    /// have the same underlying value, <b>or</b> when both are NA. (Mirrors the
    /// <see cref="IEquatable{T}"/> behaviour expected by .NET dictionaries; the
    /// <c>==</c> operator instead uses NaN-style "Na is unequal to itself"
    /// semantics.)
    /// </summary>
    public bool Equals(NaInt<T> other)
    {
        // bool Equals(T) is true for NaInteger<T> Na == NaInteger<T> Na
        // bool == operator is false for NaInteger<T> Na == NaInteger<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return EqualOrBothNa(other);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is NaInt<T> n && Equals(n);
    }

    /// <summary>
    /// Three-valued equality: returns <see cref="Trilean.Na"/> when either side is
    /// NA, <see cref="Trilean.True"/> when both sides have equal underlying values,
    /// and <see cref="Trilean.False"/> otherwise.
    /// </summary>
    public Trilean TernaryEquals(NaInt<T> other)
    {
        if (IsNa || other.IsNa)
        {
            return Trilean.Na;
        }

        return _value == other._value ? Trilean.True : Trilean.False;
    }

    /// <summary>Returns <see langword="true"/> only when both values are non-NA and equal.</summary>
    public bool EqualAndNotNa(NaInt<T> other)
    {
        return !IsNa && !other.IsNa && _value == other._value;
    }

    /// <summary>Returns <see langword="true"/> when both values are NA, or both are non-NA and equal.</summary>
    public bool EqualOrBothNa(NaInt<T> other)
    {
        return (IsNa && other.IsNa) || _value == other._value;
    }

    /// <summary>Returns <see langword="true"/> when either value is NA, or both are equal.</summary>
    public bool EqualOrEitherNa(NaInt<T> other)
    {
        return IsNa || other.IsNa || _value == other._value;
    }

    /// <summary>
    /// Compares this value to <paramref name="other"/>. NA sorts before all other
    /// values and equals only itself, mirroring <see cref="float.NaN"/>'s
    /// <see cref="IComparable.CompareTo(object)"/> convention.
    /// </summary>
    public int CompareTo(NaInt<T> other)
    {
        // NA sorts before real values and equals only itself — consistent with Equals
        // and with the .NET IEEE 754 convention (float.NaN.CompareTo(x) returns -1).
        if (IsNa)
        {
            return other.IsNa ? 0 : -1;
        }

        return other.IsNa ? 1 : _value.CompareTo(other._value);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NaInt<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NaInt<>)}", nameof(obj));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    /// <summary>The additive identity (zero).</summary>
    public static NaInt<T> AdditiveIdentity => T.AdditiveIdentity;

    /// <summary>The numeric value one.</summary>
    public static NaInt<T> One => T.One;

    /// <summary>The radix used by <typeparamref name="T"/> (typically 2).</summary>
    public static int Radix => T.Radix;

    /// <summary>The numeric value zero.</summary>
    public static NaInt<T> Zero => T.Zero;

    /// <summary>The multiplicative identity (one).</summary>
    public static NaInt<T> MultiplicativeIdentity => T.MultiplicativeIdentity;

    /// <summary>
    /// The maximum representable non-NA value (<c>T.MaxValue - 1</c>);
    /// <see cref="IMinMaxValue{T}.MaxValue"/> itself is reserved as the NA sentinel.
    /// </summary>
    public static NaInt<T> MaxValue { get; } = T.MaxValue - T.One;

    /// <summary>The minimum representable value.</summary>
    public static NaInt<T> MinValue { get; } = T.MinValue;

    static NaInt<T> INaValue<NaInt<T>, T>.Na => Na;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NaInt<T> ResultIfNotNa(NaInt<T> x, NaInt<T> y, Func<T, T, T> op)
    {
        return x.IsNa | y.IsNa ? Na : new(op(x._value, y._value), false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NaInt<T> ResultIfNotNa(NaInt<T> x, T y, Func<T, T, T> op)
    {
        return x.IsNa ? Na : new(op(x._value, y), false);
    }

    /// <summary>Adds two values; if either is NA, returns NA.</summary>
    public static NaInt<T> operator +(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a + b);
    }

    /// <summary>Adds a value to a raw <typeparamref name="T"/>; if <paramref name="x"/> is NA, returns NA.</summary>
    public static NaInt<T> operator +(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a + b);
    }

    /// <summary>Subtracts two values; if either is NA, returns NA.</summary>
    public static NaInt<T> operator -(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a - b);
    }

    /// <summary>Subtracts a raw <typeparamref name="T"/>; if <paramref name="x"/> is NA, returns NA.</summary>
    public static NaInt<T> operator -(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a - b);
    }

    /// <summary>Multiplies two values; if either is NA, returns NA.</summary>
    public static NaInt<T> operator *(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a * b);
    }

    /// <summary>Multiplies by a raw <typeparamref name="T"/>; if <paramref name="x"/> is NA, returns NA.</summary>
    public static NaInt<T> operator *(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a * b);
    }

    /// <summary>Divides two values. Returns NA if either side is NA <i>or</i> the divisor is zero.</summary>
    public static NaInt<T> operator /(NaInt<T> x, NaInt<T> y)
    {
        return x.IsNa | y.IsNa | (y._value == T.Zero)
            ? Na
            : new(x._value / y._value, true);
    }

    /// <summary>Divides by a raw <typeparamref name="T"/>. Returns NA if <paramref name="x"/> is NA or <paramref name="y"/> is zero.</summary>
    public static NaInt<T> operator /(NaInt<T> x, T y)
    {
        return x.IsNa | (y == T.Zero)
            ? Na
            : new(x._value / y, true);
    }

    /// <summary>
    /// Equality operator. Mirrors <see cref="float.NaN"/>: <c>Na == Na</c> is
    /// <see langword="false"/>. Use <see cref="Equals(NaInt{T})"/> for the
    /// <see cref="IEquatable{T}"/>-style comparison that treats NA as self-equal.
    /// </summary>
    public static bool operator ==(NaInt<T> left, NaInt<T> right)
    {
        // bool == operator is false for NaInteger<T> Na == NaInteger<T> Na
        // bool Equals(T) is true for NaInteger<T> Na == NaInteger<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return left.EqualAndNotNa(right);
    }

    /// <summary>Inequality operator. See <see cref="op_Equality(NaInt{T}, NaInt{T})"/>.</summary>
    public static bool operator !=(NaInt<T> left, NaInt<T> right)
    {
        return !left.EqualAndNotNa(right);
    }

    /// <summary>Less-than. Always <see langword="false"/> when either side is NA.</summary>
    public static bool operator <(NaInt<T> x, NaInt<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value < y._value;
    }

    /// <summary>Greater-than. Always <see langword="false"/> when either side is NA.</summary>
    public static bool operator >(NaInt<T> x, NaInt<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value > y._value;
    }

    /// <summary>Returns <see cref="NaValue.NaValueLabel"/> if NA, otherwise the underlying value's <see cref="object.ToString"/>.</summary>
    public override string? ToString()
    {
        return IsNa ? NaValue.NaValueLabel : _value.ToString();
    }

    /// <summary>Returns the absolute value, or NA when <paramref name="value"/> is NA.</summary>
    public static NaInt<T> Abs(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return T.Abs(value._value);
    }

    /// <summary>NA values are not canonical; non-NA values delegate to <typeparamref name="T"/>.</summary>
    public static bool IsCanonical(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsCanonical(value._value);
    }

    /// <summary>NA values are not complex; non-NA values delegate to <typeparamref name="T"/>.</summary>
    public static bool IsComplexNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsComplexNumber(value._value);
    }

    /// <summary>Tests for an even integer; returns <see langword="false"/> for NA.</summary>
    public static bool IsEvenInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsEvenInteger(value._value);
    }

    /// <summary>Tests for finiteness; returns <see langword="false"/> for NA.</summary>
    public static bool IsFinite(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsFinite(value._value);
    }

    /// <summary>Tests whether the value is imaginary; always <see langword="false"/> for integer types.</summary>
    public static bool IsImaginaryNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsImaginaryNumber(value._value);
    }

    /// <summary>Tests for infinity; returns <see langword="false"/> for NA.</summary>
    public static bool IsInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInfinity(value._value);
    }

    /// <summary>Tests whether the value is an integer; always <see langword="true"/> for non-NA integer types.</summary>
    public static bool IsInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInteger(value._value);
    }

    /// <summary>NA values are reported as NaN here, mirroring the <see cref="float.NaN"/> convention.</summary>
    public static bool IsNaN(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return true;
        }

        return T.IsNaN(value._value);
    }

    /// <summary>Tests whether the value is negative; <see langword="false"/> for NA.</summary>
    public static bool IsNegative(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegative(value._value);
    }

    /// <summary>Tests for negative infinity; <see langword="false"/> for NA.</summary>
    public static bool IsNegativeInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegativeInfinity(value._value);
    }

    /// <summary>Tests for a normal number; <see langword="false"/> for NA.</summary>
    public static bool IsNormal(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNormal(value._value);
    }

    /// <summary>Tests for an odd integer; <see langword="false"/> for NA.</summary>
    public static bool IsOddInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsOddInteger(value._value);
    }

    /// <summary>Tests for a positive value; <see langword="false"/> for NA.</summary>
    public static bool IsPositive(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositive(value._value);
    }

    /// <summary>Tests for positive infinity; <see langword="false"/> for NA.</summary>
    public static bool IsPositiveInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositiveInfinity(value._value);
    }

    /// <summary>Tests for a real number; <see langword="false"/> for NA.</summary>
    public static bool IsRealNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsRealNumber(value._value);
    }

    /// <summary>Tests for a subnormal number; <see langword="false"/> for NA.</summary>
    public static bool IsSubnormal(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsSubnormal(value._value);
    }

    /// <summary>Tests for zero; <see langword="false"/> for NA.</summary>
    public static bool IsZero(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsZero(value._value);
    }

    /// <summary>Returns the value with the larger magnitude; NA if either side is NA.</summary>
    public static NaInt<T> MaxMagnitude(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MaxMagnitude);
    }

    /// <summary>Returns the value with the larger magnitude; NA if either side is NA.</summary>
    public static NaInt<T> MaxMagnitudeNumber(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MaxMagnitudeNumber);
    }

    /// <summary>Returns the value with the smaller magnitude; NA if either side is NA.</summary>
    public static NaInt<T> MinMagnitude(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MinMagnitude);
    }

    /// <summary>Returns the value with the smaller magnitude; NA if either side is NA.</summary>
    public static NaInt<T> MinMagnitudeNumber(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MinMagnitudeNumber);
    }

    /// <summary>Parses a value from <paramref name="s"/> using the given styles and provider.</summary>
    public static NaInt<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    /// <summary>Parses a value from <paramref name="s"/> using the given styles and provider.</summary>
    public static NaInt<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    /// <summary>Tries to convert <paramref name="value"/> with overflow checking.</summary>
    public static bool TryConvertFromChecked<TOther>(
        TOther value,
        [MaybeNullWhen(false)] out NaInt<T> result) where TOther : INumberBase<TOther>
    {
        if (T.TryConvertFromChecked(value, out var val))
        {
            if (val == Sentinel)
            {
                result = Na;
                return true;
            }

            result = new(val, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Tries to convert <paramref name="value"/>, saturating on overflow.</summary>
    public static bool TryConvertFromSaturating<TOther>(
        TOther value,
        [MaybeNullWhen(false)] out NaInt<T> result) where TOther : INumberBase<TOther>
    {
        if (T.TryConvertFromSaturating(value, out var val))
        {
            if (val == Sentinel)
            {
                result = Na;
                return true;
            }

            result = new(val, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Tries to convert <paramref name="value"/>, truncating on overflow.</summary>
    public static bool TryConvertFromTruncating<TOther>(
        TOther value,
        [MaybeNullWhen(false)] out NaInt<T> result)
        where TOther : INumberBase<TOther>
    {
        if (T.TryConvertFromTruncating(value, out var val))
        {
            if (val == Sentinel)
            {
                result = Na;
                return true;
            }

            result = new(val, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Tries to convert this value to <typeparamref name="TOther"/> with overflow checking.</summary>
    public static bool TryConvertToChecked<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToChecked(value._value, out result);
    }

    /// <summary>Tries to convert this value to <typeparamref name="TOther"/>, saturating on overflow.</summary>
    public static bool TryConvertToSaturating<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToSaturating(value._value, out result);
    }

    /// <summary>Tries to convert this value to <typeparamref name="TOther"/>, truncating on overflow.</summary>
    public static bool TryConvertToTruncating<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToTruncating(value._value, out result);
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInt<T> result)
    {
        if (T.TryParse(s, style, provider, out var value) && value != Sentinel)
        {
            result = new(value, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInt<T> result)
    {
        return TryParse(s.AsSpan(), style, provider, out result);
    }

    /// <summary>
    /// Tries to format the value into <paramref name="destination"/>. NA renders
    /// as <see cref="NaValue.NaValueLabel"/>; non-NA values delegate to the
    /// underlying <typeparamref name="T"/> formatting.
    /// </summary>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (IsNa)
        {
            if (destination.Length < NaValue.NaValueLabel.Length)
            {
                charsWritten = 0;
                return false;
            }

            NaValue.NaValueLabel.AsSpan().CopyTo(destination);
            charsWritten = NaValue.NaValueLabel.Length;
            return true;
        }

        return _value.TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>
    /// Returns the formatted string. NA renders as <see cref="NaValue.NaValueLabel"/>;
    /// non-NA values delegate to <typeparamref name="T"/>'s formatter.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return IsNa ? NaValue.NaValueLabel : _value.ToString(format, formatProvider);
    }

    /// <summary>Parses a value from <paramref name="s"/> using the given provider.</summary>
    public static NaInt<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInt<T> result)
    {
        if (T.TryParse(s, provider, out var value) && value != Sentinel)
        {
            result = new(value, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Parses a value from <paramref name="s"/> using the given provider.</summary>
    public static NaInt<T> Parse(string s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInt<T> result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    int IBinaryInteger<NaInt<T>>.GetByteCount()
    {
        return ((IBinaryInteger<T>)_value).GetByteCount();
    }

    int IBinaryInteger<NaInt<T>>.GetShortestBitLength()
    {
        return ((IBinaryInteger<T>)_value).GetShortestBitLength();
    }

    static NaInt<T> IBinaryInteger<NaInt<T>>.PopCount(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(T.PopCount(value._value), true);
    }

    static NaInt<T> IBinaryInteger<NaInt<T>>.TrailingZeroCount(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(T.TrailingZeroCount(value._value), true);
    }

    static bool IBinaryInteger<NaInt<T>>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out NaInt<T> value)
    {
        if (T.TryReadBigEndian(source, isUnsigned, out var result))
        {
            if (result == Sentinel)
            {
                value = Na;
                return true;
            }

            value = new NaInt<T>(result, true);
            return true;
        }

        value = default;
        return false;
    }

    static bool IBinaryInteger<NaInt<T>>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out NaInt<T> value)
    {
        if (T.TryReadLittleEndian(source, isUnsigned, out var result))
        {
            if (result == Sentinel)
            {
                value = Na;
                return true;
            }

            value = new NaInt<T>(result, true);
            return true;
        }

        value = default;
        return false;
    }

    bool IBinaryInteger<NaInt<T>>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
    {
        return _value.TryWriteBigEndian(destination, out bytesWritten);
    }

    bool IBinaryInteger<NaInt<T>>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        return _value.TryWriteLittleEndian(destination, out bytesWritten);
    }

    static bool IBinaryNumber<NaInt<T>>.IsPow2(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPow2(value._value);
    }

    static NaInt<T> IBinaryNumber<NaInt<T>>.Log2(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(T.Log2(value._value), true);
    }

    /// <summary>Less-than-or-equal. NA on either side returns <see langword="false"/>.</summary>
    public static bool operator <=(NaInt<T> left, NaInt<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Greater-than-or-equal. NA on either side returns <see langword="false"/>.</summary>
    public static bool operator >=(NaInt<T> left, NaInt<T> right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Modulo. Returns NA if either side is NA <i>or</i> the divisor is zero.</summary>
    public static NaInt<T> operator %(NaInt<T> left, NaInt<T> right)
    {
        return left.IsNa | right.IsNa | (right._value == T.Zero)
            ? Na
            : new(left._value % right._value, true);
    }

    /// <summary>Pre-decrement. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator --(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        var v = value._value;
        return --v;
    }

    /// <summary>Pre-increment. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator ++(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        var v = value._value;
        return ++v;
    }

    /// <summary>Negation. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator -(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return -value._value;
    }

    /// <summary>Unary plus. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator +(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return +value._value;
    }

    /// <summary>Bitwise AND. Returns NA if either side is NA.</summary>
    public static NaInt<T> operator &(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a & b);
    }

    /// <summary>Bitwise OR. Returns NA if either side is NA.</summary>
    public static NaInt<T> operator |(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a | b);
    }

    /// <summary>Bitwise XOR. Returns NA if either side is NA.</summary>
    public static NaInt<T> operator ^(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a ^ b);
    }

    /// <summary>Bitwise NOT. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator ~(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(~value._value, false);
    }

    /// <summary>Left shift. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator <<(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value << shiftAmount, false);
    }

    /// <summary>Arithmetic right shift. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator >>(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value >> shiftAmount, false);
    }

    /// <summary>Logical (unsigned) right shift. Returns NA if <paramref name="value"/> is NA.</summary>
    public static NaInt<T> operator >>>(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value >>> shiftAmount, false);
    }
}
