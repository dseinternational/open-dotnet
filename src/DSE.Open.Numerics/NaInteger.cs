// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Operator overloads have named alternates

[StructLayout(LayoutKind.Sequential)]
public readonly struct NaInteger<T>
    : INumber<NaInteger<T>>,
      IMinMaxValue<T>,
      INullable<NaInteger<T>, T>
      where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
{
    private static readonly T s_sentinel = T.MaxValue;

    public static T Sentinel => s_sentinel;

    public static NaInteger<T> Na => new(s_sentinel, true);

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
    private NaInteger(T value, bool skipCheck)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _value = value;
    }

    public NaInteger(T value)
    {
        _value = (value == s_sentinel)
            ? throw new ArgumentOutOfRangeException(nameof(value),
                $"Value {s_sentinel} is reserved as NA")
            : value;
    }

    public static implicit operator NaInteger<T>(T value)
    {
        return new(value);
    }

    public static implicit operator NaInteger<T>(T? value)
    {
        if (value is null)
        {
            return Na;
        }

        return new(value.Value);
    }

    public static explicit operator T(NaInteger<T> value)
    {
        return value.IsNa
            ? throw new UnknownValueException()
            : value._value;
    }

    public bool IsNa => _value == s_sentinel;

    T INullable<NaInteger<T>, T>.Value => (T)this;

    bool INullable.HasValue => !IsNa;

    public bool Equals(NaInteger<T> other)
    {
        return !IsNa & !other.IsNa && _value == other._value;
    }

    public int CompareTo(NaInteger<T> other)
    {
        return IsNa | other.IsNa ? 0 : _value.CompareTo(other._value);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NaInteger<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NaInteger<>)}", nameof(obj));
    }

    public override bool Equals(object? obj)
    {
        return obj is NaInteger<T> n && Equals(n);
    }

    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    public static NaInteger<T> AdditiveIdentity => T.AdditiveIdentity;

    public static NaInteger<T> One => T.One;

    public static int Radix => T.Radix;

    public static NaInteger<T> Zero => T.Zero;

    public static NaInteger<T> MultiplicativeIdentity => T.MultiplicativeIdentity;

    public static T MaxValue => T.MaxValue;

    public static T MinValue => T.MinValue + T.One;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NaInteger<T> Ternary(NaInteger<T> x, NaInteger<T> y, Func<T, T, T> op)
    {
        return x.IsNa | y.IsNa ? Na : new(op(x._value, y._value), true);
    }

    public static NaInteger<T> operator +(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, static (a, b) => a + b);
    }

    public static NaInteger<T> operator -(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, static (a, b) => a - b);
    }

    public static NaInteger<T> operator *(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, static (a, b) => a * b);
    }

    public static NaInteger<T> operator /(NaInteger<T> x, NaInteger<T> y)
    {
        return x.IsNa | y.IsNa | (y._value == T.Zero)
            ? Na
            : new(x._value / y._value, true);
    }

    public static bool operator ==(NaInteger<T> x, NaInteger<T> y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(NaInteger<T> x, NaInteger<T> y)
    {
        return !(x == y);
    }

    public static bool operator <(NaInteger<T> x, NaInteger<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value < y._value;
    }

    public static bool operator >(NaInteger<T> x, NaInteger<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value > y._value;
    }

    public override string? ToString()
    {
        return IsNa ? "NA" : _value.ToString();
    }

    public static NaInteger<T> Abs(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return Na;
        }

        return T.Abs(value._value);
    }

    public static bool IsCanonical(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsCanonical(value._value);
    }

    public static bool IsComplexNumber(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsComplexNumber(value._value);
    }

    public static bool IsEvenInteger(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsEvenInteger(value._value);
    }

    public static bool IsFinite(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsFinite(value._value);
    }

    public static bool IsImaginaryNumber(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsImaginaryNumber(value._value);
    }

    public static bool IsInfinity(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInfinity(value._value);
    }

    public static bool IsInteger(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsInteger(value._value);
    }

    public static bool IsNaN(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return true;
        }

        return T.IsNaN(value._value);
    }

    public static bool IsNegative(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegative(value._value);
    }

    public static bool IsNegativeInfinity(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNegativeInfinity(value._value);
    }

    public static bool IsNormal(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsNormal(value._value);
    }

    public static bool IsOddInteger(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsOddInteger(value._value);
    }

    public static bool IsPositive(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositive(value._value);
    }

    public static bool IsPositiveInfinity(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsPositiveInfinity(value._value);
    }

    public static bool IsRealNumber(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsRealNumber(value._value);
    }

    public static bool IsSubnormal(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsSubnormal(value._value);
    }

    public static bool IsZero(NaInteger<T> value)
    {
        if (value.IsNa)
        {
            return false;
        }

        return T.IsZero(value._value);
    }

    public static NaInteger<T> MaxMagnitude(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, T.MaxMagnitude);
    }

    public static NaInteger<T> MaxMagnitudeNumber(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, T.MaxMagnitudeNumber);
    }

    public static NaInteger<T> MinMagnitude(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, T.MinMagnitude);
    }

    public static NaInteger<T> MinMagnitudeNumber(NaInteger<T> x, NaInteger<T> y)
    {
        return Ternary(x, y, T.MinMagnitudeNumber);
    }

    public static NaInteger<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    public static NaInteger<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    public static bool TryConvertFromChecked<TOther>(
        TOther value,
        [MaybeNullWhen(false)] out NaInteger<T> result) where TOther : INumberBase<TOther>
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
        [MaybeNullWhen(false)] out NaInteger<T> result) where TOther : INumberBase<TOther>
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
        [MaybeNullWhen(false)] out NaInteger<T> result)
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
        NaInteger<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToChecked(value._value, out result);
    }

    public static bool TryConvertToSaturating<TOther>(
        NaInteger<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToSaturating(value._value, out result);
    }

    public static bool TryConvertToTruncating<TOther>(
        NaInteger<T> value,
        [MaybeNullWhen(false)] out TOther result)
        where TOther : INumberBase<TOther>
    {
        return T.TryConvertToTruncating(value._value, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInteger<T> result)
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
        [MaybeNullWhen(false)] out NaInteger<T> result)
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

    public static NaInteger<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInteger<T> result)
    {
        if (T.TryParse(s, provider, out var value) && value != Sentinel)
        {
            result = new(value, true);
            return true;
        }

        result = default;
        return false;
    }

    public static NaInteger<T> Parse(string s, IFormatProvider? provider)
    {
        return T.Parse(s, provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaInteger<T> result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool operator <=(NaInteger<T> left, NaInteger<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(NaInteger<T> left, NaInteger<T> right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static NaInteger<T> operator %(NaInteger<T> left, NaInteger<T> right)
    {
        return left % right;
    }

    public static NaInteger<T> operator --(NaInteger<T> value)
    {
        return --value;
    }

    public static NaInteger<T> operator ++(NaInteger<T> value)
    {
        return ++value;
    }

    public static NaInteger<T> operator -(NaInteger<T> value)
    {
        return -value;
    }

    public static NaInteger<T> operator +(NaInteger<T> value)
    {
        return +value;
    }
}
