// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA1000 // Do not declare static members on generic types

[StructLayout(LayoutKind.Sequential)]
public readonly struct NaNumber<T>
    : IEquatable<NaNumber<T>>,
      IComparable<NaNumber<T>>,
      IAdditionOperators<NaNumber<T>, NaNumber<T>, NaNumber<T>>,
      IAdditiveIdentity<NaNumber<T>, NaNumber<T>>,
      ISubtractionOperators<NaNumber<T>, NaNumber<T>, NaNumber<T>>,
      IMultiplyOperators<NaNumber<T>, NaNumber<T>, NaNumber<T>>,
      IDivisionOperators<NaNumber<T>, NaNumber<T>, NaNumber<T>>
      where T : struct, INumber<T>, IMinMaxValue<T>
{
    private static readonly T s_sentinel = T.MinValue;

    public static T Sentinel => s_sentinel;

    public static NaNumber<T> Na => new(s_sentinel, true);

    private readonly T _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
    private NaNumber(T value, bool skipCheck)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _value = value;
    }

    public NaNumber(T value)
    {
        _value = (value == s_sentinel)
            ? throw new ArgumentOutOfRangeException(nameof(value),
                $"Value {s_sentinel} is reserved as NA")
            : value;
    }

    public static implicit operator NaNumber<T>(T value)
    {
        return new(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static explicit operator T(NaNumber<T> value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return value.IsNa
                ? throw new InvalidOperationException("Value is NA")
                : value._value;
    }

    public bool IsNa => _value == s_sentinel;

    public static NaNumber<T> AdditiveIdentity => T.AdditiveIdentity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NaNumber<T> Ternary(NaNumber<T> x, NaNumber<T> y, Func<T, T, T> op)
    {
        return x.IsNa | y.IsNa ? Na : new(op(x._value, y._value), true);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static NaNumber<T> operator +(NaNumber<T> x, NaNumber<T> y)
    {
        return Ternary(x, y, static (a, b) => a + b);
    }

    public static NaNumber<T> operator -(NaNumber<T> x, NaNumber<T> y)
    {
        return Ternary(x, y, static (a, b) => a - b);
    }

    public static NaNumber<T> operator *(NaNumber<T> x, NaNumber<T> y)
    {
        return Ternary(x, y, static (a, b) => a * b);
    }

    public static NaNumber<T> operator /(NaNumber<T> x, NaNumber<T> y)
    {
        return x.IsNa | y.IsNa | (y._value == T.Zero)
            ? Na
            : new(x._value / y._value, true);
    }

    public bool Equals(NaNumber<T> other)
    {
        return !IsNa & !other.IsNa && _value == other._value;
    }

    public int CompareTo(NaNumber<T> other)
    {
        return IsNa | other.IsNa ? 0 : _value.CompareTo(other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is NaNumber<T> n && Equals(n);
    }

    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    public static bool operator ==(NaNumber<T> x, NaNumber<T> y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(NaNumber<T> x, NaNumber<T> y)
    {
        return !(x == y);
    }

    public static bool operator <(NaNumber<T> x, NaNumber<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value < y._value;
    }

    public static bool operator >(NaNumber<T> x, NaNumber<T> y)
    {
        return !x.IsNa & !y.IsNa && x._value > y._value;
    }

    public override string? ToString()
    {
        return IsNa ? "NA" : _value.ToString();
    }

    public NaNumber<T> ToNaNumber()
    {
        throw new NotImplementedException();
    }
    public static bool operator <=(NaNumber<T> left, NaNumber<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(NaNumber<T> left, NaNumber<T> right)
    {
        return left.CompareTo(right) >= 0;
    }
}
