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
/// <typeparam name="T"></typeparam>
[StructLayout(LayoutKind.Sequential)]
public readonly struct NaInt<T>
    : INaNumber<NaInt<T>, T>,
      IBinaryInteger<NaInt<T>>
      where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
{
    private static readonly T s_sentinel = T.MaxValue;

    public static T Sentinel => s_sentinel;

    public static NaInt<T> Na => new(s_sentinel, true);

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
    private NaInt(T value, bool skipCheck)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _value = value;
    }

    public NaInt(T value)
    {
        _value = (value == s_sentinel)
            ? throw new ArgumentOutOfRangeException(nameof(value),
                $"Value {s_sentinel} is reserved as NA")
            : value;
    }

    public static NaInt<T> FromValue(T value)
    {
        return new(value);
    }

    public static implicit operator NaInt<T>(T value)
    {
        return FromValue(value);
    }

    public static implicit operator NaInt<T>(T? value)
    {
        if (value is null)
        {
            return Na;
        }

        return new(value.Value);
    }

    public static explicit operator T(NaInt<T> value)
    {
        return value.IsNa
            ? throw new NaValueException()
            : value._value;
    }

    public bool IsNa => _value == s_sentinel;

    T INaValue<NaInt<T>, T>.Value => (T)this;

    bool INaValue.HasValue => !IsNa;

    public bool Equals(NaInt<T> other)
    {
        // bool Equals(T) is true for NaInteger<T> Na == NaInteger<T> Na
        // bool == operator is false for NaInteger<T> Na == NaInteger<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return EqualOrBothNa(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is NaInt<T> n && Equals(n);
    }

    public Trilean TernaryEquals(NaInt<T> other)
    {
        if (IsNa || other.IsNa)
        {
            return Trilean.Na;
        }

        return _value == other._value ? Trilean.True : Trilean.False;
    }

    public bool EqualAndNotNa(NaInt<T> other)
    {
        return !IsNa && !other.IsNa && _value == other._value;
    }

    public bool EqualOrBothNa(NaInt<T> other)
    {
        return (IsNa && other.IsNa) || _value == other._value;
    }

    public bool EqualOrEitherNa(NaInt<T> other)
    {
        return IsNa || other.IsNa || _value == other._value;
    }

    public int CompareTo(NaInt<T> other)
    {
        return IsNa | other.IsNa ? 0 : _value.CompareTo(other._value);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NaInt<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NaInt<>)}", nameof(obj));
    }

    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    public static NaInt<T> AdditiveIdentity => T.AdditiveIdentity;

    public static NaInt<T> One => T.One;

    public static int Radix => T.Radix;

    public static NaInt<T> Zero => T.Zero;

    public static NaInt<T> MultiplicativeIdentity => T.MultiplicativeIdentity;

    public static NaInt<T> MaxValue { get; } = T.MaxValue - T.One;

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

    public static NaInt<T> operator +(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a + b);
    }

    public static NaInt<T> operator +(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a + b);
    }

    public static NaInt<T> operator -(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a - b);
    }

    public static NaInt<T> operator -(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a - b);
    }

    public static NaInt<T> operator *(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a * b);
    }

    public static NaInt<T> operator *(NaInt<T> x, T y)
    {
        return ResultIfNotNa(x, y, static (a, b) => a * b);
    }

    public static NaInt<T> operator /(NaInt<T> x, NaInt<T> y)
    {
        return x.IsNa | y.IsNa | (y._value == T.Zero)
            ? Na
            : new(x._value / y._value, true);
    }

    public static NaInt<T> operator /(NaInt<T> x, T y)
    {
        return x.IsNa | (y == T.Zero)
            ? Na
            : new(x._value / y, true);
    }

    public static bool operator ==(NaInt<T> left, NaInt<T> right)
    {
        // bool == operator is false for NaInteger<T> Na == NaInteger<T> Na
        // bool Equals(T) is true for NaInteger<T> Na == NaInteger<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return left.EqualAndNotNa(right);
    }

    public static bool operator !=(NaInt<T> left, NaInt<T> right)
    {
        return !left.EqualAndNotNa(right);
    }

    public static bool operator <(NaInt<T> x, NaInt<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value < y._value;
    }

    public static bool operator >(NaInt<T> x, NaInt<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value > y._value;
    }

    public override string? ToString()
    {
        return IsNa ? NaValue.NaValueLabel : _value.ToString();
    }

    public static NaInt<T> Abs(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return T.Abs(value._value);
    }

    public static bool IsCanonical(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsCanonical(value._value);
    }

    public static bool IsComplexNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsComplexNumber(value._value);
    }

    public static bool IsEvenInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsEvenInteger(value._value);
    }

    public static bool IsFinite(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsFinite(value._value);
    }

    public static bool IsImaginaryNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsImaginaryNumber(value._value);
    }

    public static bool IsInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInfinity(value._value);
    }

    public static bool IsInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInteger(value._value);
    }

    public static bool IsNaN(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return true;
        }

        return T.IsNaN(value._value);
    }

    public static bool IsNegative(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegative(value._value);
    }

    public static bool IsNegativeInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegativeInfinity(value._value);
    }

    public static bool IsNormal(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNormal(value._value);
    }

    public static bool IsOddInteger(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsOddInteger(value._value);
    }

    public static bool IsPositive(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositive(value._value);
    }

    public static bool IsPositiveInfinity(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositiveInfinity(value._value);
    }

    public static bool IsRealNumber(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsRealNumber(value._value);
    }

    public static bool IsSubnormal(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsSubnormal(value._value);
    }

    public static bool IsZero(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsZero(value._value);
    }

    public static NaInt<T> MaxMagnitude(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MaxMagnitude);
    }

    public static NaInt<T> MaxMagnitudeNumber(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MaxMagnitudeNumber);
    }

    public static NaInt<T> MinMagnitude(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MinMagnitude);
    }

    public static NaInt<T> MinMagnitudeNumber(NaInt<T> x, NaInt<T> y)
    {
        return ResultIfNotNa(x, y, T.MinMagnitudeNumber);
    }

    public static NaInt<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    public static NaInt<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

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

    public static bool TryConvertToChecked<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToChecked(value._value, out result);
    }

    public static bool TryConvertToSaturating<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToSaturating(value._value, out result);
    }

    public static bool TryConvertToTruncating<TOther>(
        NaInt<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToTruncating(value._value, out result);
    }

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

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInt<T> result)
    {
        return TryParse(s.AsSpan(), style, provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return _value.TryFormat(destination, out charsWritten, format, provider);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString(format, formatProvider);
    }

    public static NaInt<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

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

    public static NaInt<T> Parse(string s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

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

    public static bool operator <=(NaInt<T> left, NaInt<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(NaInt<T> left, NaInt<T> right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static NaInt<T> operator %(NaInt<T> left, NaInt<T> right)
    {
        return left % right;
    }

    public static NaInt<T> operator --(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        var v = value._value;
        return --v;
    }

    public static NaInt<T> operator ++(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        var v = value._value;
        return ++v;
    }

    public static NaInt<T> operator -(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return -value._value;
    }

    public static NaInt<T> operator +(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return +value._value;
    }

    public static NaInt<T> operator &(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a & b);
    }

    public static NaInt<T> operator |(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a | b);
    }

    public static NaInt<T> operator ^(NaInt<T> left, NaInt<T> right)
    {
        return ResultIfNotNa(left, right, static (a, b) => a ^ b);
    }

    public static NaInt<T> operator ~(NaInt<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(~value._value, false);
    }

    public static NaInt<T> operator <<(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value << shiftAmount, false);
    }

    public static NaInt<T> operator >>(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value >> shiftAmount, false);
    }

    public static NaInt<T> operator >>>(NaInt<T> value, int shiftAmount)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return new(value._value >>> shiftAmount, false);
    }
}
