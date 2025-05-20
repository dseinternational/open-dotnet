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

    internal static NaFloat<T> Na => new(T.NaN);

    public static NaFloat<T> MaxValue { get; } = T.MaxValue;

    public static NaFloat<T> MinValue { get; } = T.MaxValue;

    static NaFloat<T> INaValue<NaFloat<T>, T>.Na { get; } = Na;

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private NaFloat(T value)
    {
        _value = value;
    }

    public static NaFloat<T> FromValue(T value)
    {
        return new(value);
    }

    bool INaValue.HasValue => !T.IsNaN(_value);

    T INaValue<NaFloat<T>, T>.Value => _value;

    public bool Equals(NaFloat<T> other)
    {
        return _value.Equals(other._value);
    }

    Trilean ITernaryEquatable<NaFloat<T>>.TernaryEquals(NaFloat<T> other)
    {
        throw new NotImplementedException();
    }

    bool ITernaryEquatable<NaFloat<T>>.EqualAndNotNa(NaFloat<T> other)
    {
        throw new NotImplementedException();
    }

    bool ITernaryEquatable<NaFloat<T>>.EqualOrBothNa(NaFloat<T> other)
    {
        throw new NotImplementedException();
    }

    bool ITernaryEquatable<NaFloat<T>>.EqualOrEitherNa(NaFloat<T> other)
    {
        throw new NotImplementedException();
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

    public override int GetHashCode()
    {
        return HashCode.Combine(_value);
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

    static NaFloat<T> INumberBase<NaFloat<T>>.One => T.One;

    static int INumberBase<NaFloat<T>>.Radix => T.Radix;

    static NaFloat<T> INumberBase<NaFloat<T>>.Zero => throw new NotImplementedException();

    static NaFloat<T> IAdditiveIdentity<NaFloat<T>, NaFloat<T>>.AdditiveIdentity => throw new NotImplementedException();

    static NaFloat<T> IMultiplicativeIdentity<NaFloat<T>, NaFloat<T>>.MultiplicativeIdentity => throw new NotImplementedException();

    static bool IComparisonOperators<NaFloat<T>, NaFloat<T>, bool>.operator >(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static bool IComparisonOperators<NaFloat<T>, NaFloat<T>, bool>.operator >=(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static bool IComparisonOperators<NaFloat<T>, NaFloat<T>, bool>.operator <(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static bool IComparisonOperators<NaFloat<T>, NaFloat<T>, bool>.operator <=(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IModulusOperators<NaFloat<T>, NaFloat<T>, NaFloat<T>>.operator %(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IAdditionOperators<NaFloat<T>, NaFloat<T>, NaFloat<T>>.operator +(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IDecrementOperators<NaFloat<T>>.operator --(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IDivisionOperators<NaFloat<T>, NaFloat<T>, NaFloat<T>>.operator /(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IIncrementOperators<NaFloat<T>>.operator ++(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IMultiplyOperators<NaFloat<T>, NaFloat<T>, NaFloat<T>>.operator *(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> ISubtractionOperators<NaFloat<T>, NaFloat<T>, NaFloat<T>>.operator -(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IUnaryNegationOperators<NaFloat<T>, NaFloat<T>>.operator -(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IUnaryPlusOperators<NaFloat<T>, NaFloat<T>>.operator +(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool IEqualityOperators<NaFloat<T>, NaFloat<T>, bool>.operator ==(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    static bool IEqualityOperators<NaFloat<T>, NaFloat<T>, bool>.operator !=(NaFloat<T> left, NaFloat<T> right)
    {
        throw new NotImplementedException();
    }

    int IComparable.CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    int IComparable<NaFloat<T>>.CompareTo(NaFloat<T> other)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.Abs(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsCanonical(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsComplexNumber(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsEvenInteger(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsFinite(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsImaginaryNumber(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsInfinity(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsInteger(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsNaN(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsNegative(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsNegativeInfinity(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsNormal(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsOddInteger(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsPositive(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsPositiveInfinity(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsRealNumber(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsSubnormal(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.IsZero(NaFloat<T> value)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MaxMagnitude(NaFloat<T> x, NaFloat<T> y)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MaxMagnitudeNumber(NaFloat<T> x, NaFloat<T> y)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MinMagnitude(NaFloat<T> x, NaFloat<T> y)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.MinMagnitudeNumber(NaFloat<T> x, NaFloat<T> y)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INumberBase<NaFloat<T>>.Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromChecked<TOther>(TOther value, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromSaturating<TOther>(TOther value, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertFromTruncating<TOther>(TOther value, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToChecked<TOther>(NaFloat<T> value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToSaturating<TOther>(NaFloat<T> value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToTruncating<TOther>(NaFloat<T> value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<NaFloat<T>>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> ISpanParsable<NaFloat<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static bool ISpanParsable<NaFloat<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> IParsable<NaFloat<T>>.Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static bool IParsable<NaFloat<T>>.TryParse(string? s, IFormatProvider? provider, out NaFloat<T> result)
    {
        throw new NotImplementedException();
    }

    static NaFloat<T> INaNumber<NaFloat<T>, T>.FromValue(T value)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(NaFloat<T> left, NaFloat<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NaFloat<T> left, NaFloat<T> right)
    {
        return !(left == right);
    }

    public static bool operator <(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value < right._value;
    }

    public static bool operator <=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value <= right._value;
    }

    public static bool operator >(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value > right._value;
    }

    public static bool operator >=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value >= right._value;
    }
}
