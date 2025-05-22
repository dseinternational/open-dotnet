// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Operator overloads have named alternates

// **** TODO ****

[StructLayout(LayoutKind.Sequential)]
public readonly struct NaFloat<T>
    : INaNumber<NaFloat<T>, T>
      where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    public static T Sentinel => T.NaN;

    public static NaFloat<T> Na { get; } = new(T.NaN);

    public static NaFloat<T> MaxValue { get; } = new(T.MaxValue);

    public static NaFloat<T> MinValue { get; } = new(T.MinValue);

    static NaFloat<T> INaValue<NaFloat<T>, T>.Na { get; } = Na;

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NaFloat(T value)
    {
        _value = value;
    }

    public static NaFloat<T> FromValue(T value)
    {
        return new(value);
    }

    bool INaValue.HasValue => !T.IsNaN(_value);

    T INaValue<NaFloat<T>, T>.Value => _value;

    public bool IsNa => T.IsNaN(_value);

    public bool Equals(NaFloat<T> other)
    {
        // bool Equals(T) is true for NaFloat<T> Na == NaFloat<T> Na
        // bool == operator is false for NaFloat<T> Na == NaFloat<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return EqualOrBothNa(other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is NaFloat<T> other)
        {
            return Equals(other);
        }

        if (obj is T value)
        {
            return value.Equals(_value);
        }

        return false;
    }

    public Trilean TernaryEquals(NaFloat<T> other)
    {
        if (IsNa || other.IsNa)
        {
            return Trilean.Na;
        }

        return _value == other._value ? Trilean.True : Trilean.False;
    }

    public bool EqualAndNotNa(NaFloat<T> other)
    {
        return !IsNa && !other.IsNa && _value == other._value;
    }

    public bool EqualOrBothNa(NaFloat<T> other)
    {
        return (IsNa && other.IsNa) || (!IsNa && !other.IsNa && _value == other._value);
    }

    public bool EqualOrEitherNa(NaFloat<T> other)
    {
        return (IsNa && other.IsNa) || _value == other._value;
    }

    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    public static implicit operator NaFloat<T>(T value)
    {
        return new(value);
    }

    public static implicit operator NaFloat<T>(T? value)
    {
        if (value is null)
        {
            return Na;
        }

        return new(value.Value);
    }

    public static implicit operator T(NaFloat<T> value)
    {
        return value._value;
    }

    // -----------------------------------------------------------------
    // INumberBase

    public static NaFloat<T> One => T.One;

    static int INumberBase<NaFloat<T>>.Radix => T.Radix;

    public static NaFloat<T> Zero => new(T.Zero);

    static NaFloat<T> IAdditiveIdentity<NaFloat<T>, NaFloat<T>>.AdditiveIdentity => new(T.Zero);

    static NaFloat<T> IMultiplicativeIdentity<NaFloat<T>, NaFloat<T>>.MultiplicativeIdentity => new(T.One);

    public static bool operator >(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value > right._value;
    }

    public static bool operator >=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value >= right._value;
    }

    public static bool operator <(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value < right._value;
    }

    public static bool operator <=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value <= right._value;
    }

    public static NaFloat<T> operator %(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value % right._value);
    }

    public static NaFloat<T> operator +(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value + right._value);
    }

    public static NaFloat<T> operator --(NaFloat<T> value)
    {
        var v = value._value;
        return new NaFloat<T>(--v);
    }

    public static NaFloat<T> operator /(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value / right._value);
    }

    public static NaFloat<T> operator ++(NaFloat<T> value)
    {
        var v = value._value;
        return new NaFloat<T>(++v);
    }

    public static NaFloat<T> operator *(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value * right._value);
    }

    public static NaFloat<T> operator -(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value - right._value);
    }

    public static NaFloat<T> operator -(NaFloat<T> value)
    {
        return new NaFloat<T>(-value._value);
    }

    public static NaFloat<T> operator +(NaFloat<T> value)
    {
        return new NaFloat<T>(+value._value);
    }

    public static bool operator ==(NaFloat<T> left, NaFloat<T> right)
    {
        // NaN == NaN: False
        return left._value == right._value;
    }

    public static bool operator !=(NaFloat<T> left, NaFloat<T> right)
    {
        // NaN != NaN: True
        return left._value != right._value;
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is NaFloat<T> other)
        {
            return CompareTo(other);
        }

        if (obj is T value)
        {
            return _value.CompareTo(value);
        }

        throw new ArgumentException($"Object must be of type {nameof(NaFloat<>)}", nameof(obj));
    }

    public int CompareTo(NaFloat<T> other)
    {
        return _value.CompareTo(other._value);
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.Abs(NaFloat<T> value)
    {
        return new NaFloat<T>(T.Abs(value._value));
    }

    static bool INumberBase<NaFloat<T>>.IsCanonical(NaFloat<T> value)
    {
        return T.IsCanonical(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsComplexNumber(NaFloat<T> value)
    {
        return T.IsComplexNumber(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsEvenInteger(NaFloat<T> value)
    {
        return T.IsEvenInteger(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsFinite(NaFloat<T> value)
    {
        return T.IsFinite(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsImaginaryNumber(NaFloat<T> value)
    {
        return T.IsImaginaryNumber(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsInfinity(NaFloat<T> value)
    {
        return T.IsInfinity(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsInteger(NaFloat<T> value)
    {
        return T.IsInteger(value._value);
    }

    public static bool IsNaN(NaFloat<T> value)
    {
        return value.IsNa;
    }

    static bool INumberBase<NaFloat<T>>.IsNegative(NaFloat<T> value)
    {
        return T.IsNegative(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsNegativeInfinity(NaFloat<T> value)
    {
        return T.IsNegativeInfinity(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsNormal(NaFloat<T> value)
    {
        return T.IsNormal(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsOddInteger(NaFloat<T> value)
    {
        return T.IsOddInteger(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsPositive(NaFloat<T> value)
    {
        return T.IsPositive(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsPositiveInfinity(NaFloat<T> value)
    {
        return T.IsPositiveInfinity(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsRealNumber(NaFloat<T> value)
    {
        return T.IsRealNumber(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsSubnormal(NaFloat<T> value)
    {
        return T.IsSubnormal(value._value);
    }

    static bool INumberBase<NaFloat<T>>.IsZero(NaFloat<T> value)
    {
        return T.IsZero(value._value);
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MaxMagnitude(NaFloat<T> x, NaFloat<T> y)
    {
        return T.MaxMagnitude(x._value, y._value);
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MaxMagnitudeNumber(NaFloat<T> x, NaFloat<T> y)
    {
        return T.MaxMagnitudeNumber(x._value, y._value);
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MinMagnitude(NaFloat<T> x, NaFloat<T> y)
    {
        return T.MinMagnitude(x._value, y._value);
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MinMagnitudeNumber(NaFloat<T> x, NaFloat<T> y)
    {
        return T.MinMagnitudeNumber(x._value, y._value);
    }

    public static NaFloat<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return new(T.Parse(s, style, provider));
    }

    public static NaFloat<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return T.Parse(s, style, provider);
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromChecked<TOther>(TOther value, out NaFloat<T> result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = new(tValue);
            return true;
        }

        if (T.TryConvertFromChecked(value, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromSaturating<TOther>(TOther value, out NaFloat<T> result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = new(tValue);
            return true;
        }

        if (T.TryConvertFromSaturating(value, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromTruncating<TOther>(TOther value, out NaFloat<T> result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = new(tValue);
            return true;
        }

        if (T.TryConvertFromTruncating(value, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToChecked<TOther>(NaFloat<T> value, out TOther result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = (TOther)(object)naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = (TOther)(object)tValue;
            return true;
        }

        if (T.TryConvertToChecked<T>(value._value, out var valResult))
        {
            result = (TOther)(object)valResult;
            return true;
        }

        result = default!;
        return false;
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToSaturating<TOther>(NaFloat<T> value, out TOther result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = (TOther)(object)naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = (TOther)(object)tValue;
            return true;
        }

        if (T.TryConvertToSaturating<T>(value._value, out var valResult))
        {
            result = (TOther)(object)valResult;
            return true;
        }

        result = default!;
        return false;
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToTruncating<TOther>(NaFloat<T> value, out TOther result)
    {
        if (value is NaFloat<T> naValue)
        {
            result = (TOther)(object)naValue;
            return true;
        }

        if (value is T tValue)
        {
            result = (TOther)(object)tValue;
            return true;
        }

        if (T.TryConvertToTruncating<T>(value._value, out var valResult))
        {
            result = (TOther)(object)valResult;
            return true;
        }

        result = default!;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out NaFloat<T> result)
    {
        if (T.TryParse(s, style, provider, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out NaFloat<T> result)
    {
        // todo: null handling

        if (T.TryParse(s, style, provider, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (_value.TryFormat(destination, out charsWritten, format, provider))
        {
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString(format, formatProvider);
    }

    public static NaFloat<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            return new(valResult);
        }

        return Na;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out NaFloat<T> result)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            result = new(valResult);
            return true;
        }

        result = default;
        return false;
    }

    public static NaFloat<T> Parse(string s, IFormatProvider? provider)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            return new(valResult);
        }

        return Na;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out NaFloat<T> result)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            result = new(valResult);
            return true;

        }

        result = default;
        return false;
    }
}
