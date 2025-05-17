// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Numerics;

public static class NullableNumber
{
    public const string NoValueLabel = NullableValue.NoValueLabel;

    public static bool Equals<T>(NullableNumber<T> n1, T n2)
        where T : struct, INumber<T>
    {
        return n1.HasValue && n2.Equals(n1.Value);
    }

    public static bool Equals<T>(NullableNumber<T> n1, NullableNumber<T> n2)
        where T : struct, INumber<T>
    {
        return n2.HasValue && n1.HasValue && n2.Value.Equals(n1.Value);
    }

    public static int Compare<T>(NullableNumber<T> n1, NullableNumber<T> n2)
        where T : struct, INumber<T>
    {
        if (!n1.HasValue)
        {
            return n2.HasValue ? -1 : 0;
        }

        if (!n2.HasValue)
        {
            return 1;
        }

        return n1.Value.CompareTo(n2.Value);
    }
}

#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Operator overloads have named alternates

public readonly partial struct NullableNumber<T> :
    INumber<NullableNumber<T>>,
    INullable<NullableNumber<T>, T>,
    IComparable<NullableNumber<T>>,
    IComparable,
    ISpanParsable<NullableNumber<T>>,
    ISpanFormattable
    where T : struct, INumber<T>
{
    private readonly bool _hasValue;
    private readonly T _value;

    public bool HasValue => _hasValue;

    public T Value => HasValue ? _value : throw new InvalidOperationException();

    private NullableNumber(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static implicit operator NullableNumber<T>(T? value)
    {
        if (value is null)
        {
            return default;
        }

        return new(value.Value);
    }

    public static explicit operator T(NullableNumber<T> value)
    {
        return value.Value;
    }

    public override string ToString()
    {
        return _hasValue ? _value.ToString() ?? string.Empty : NullableNumber.NoValueLabel;
    }

    public override int GetHashCode()
    {
        return _hasValue ? _value.GetHashCode() : 0;
    }

    public bool Equals(NullableNumber<T> other)
    {
        return NullableNumber.Equals(this, other);
    }

    public bool Equals(T value)
    {
        return NullableNumber.Equals(this, value);
    }

    public override bool Equals(object? obj)
    {
        return (obj is NullableNumber<T> other && Equals(other))
            || (obj is T n && Equals(n));
    }

    public int CompareTo(NullableNumber<T> other)
    {
        return NullableNumber.Compare<NullableNumber<T>>(this, other);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NullableNumber<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NullableNumber<>)}", nameof(obj));
    }

    private static NullableNumber<T> Lift(Func<T, T, T> op, NullableNumber<T> x, NullableNumber<T> y)
    {
        return x.HasValue && y.HasValue ? new(op(x._value, y._value)) : default;
    }

    public static NullableNumber<T> operator +(NullableNumber<T> x, NullableNumber<T> y)
    {
        return Lift((a, b) => a + b, x, y);
    }

    public static NullableNumber<T> operator -(NullableNumber<T> x, NullableNumber<T> y)
    {
        return Lift((a, b) => a - b, x, y);
    }

    public static NullableNumber<T> operator *(NullableNumber<T> x, NullableNumber<T> y)
    {
        return Lift((a, b) => a * b, x, y);
    }

    public static NullableNumber<T> operator /(NullableNumber<T> x, NullableNumber<T> y)
    {
        return Lift((a, b) => a / b, x, y);
    }

    public static NullableNumber<T> operator %(NullableNumber<T> x, NullableNumber<T> y)
    {
        return Lift((a, b) => a % b, x, y);
    }

    public static NullableNumber<T> operator +(NullableNumber<T> value)
    {
        return value;
    }

    public static NullableNumber<T> operator -(NullableNumber<T> value)
    {
        return value.HasValue ? new(-value._value) : value;
    }

    public static NullableNumber<T> operator ++(NullableNumber<T> value)
    {
        return value.HasValue ? new(value._value + T.One) : value;
    }

    public static NullableNumber<T> operator --(NullableNumber<T> value)
    {
        return value.HasValue ? new(value._value - T.One) : value;
    }

    public static bool operator ==(NullableNumber<T> left, NullableNumber<T> right)
    {
        return NullableNumber.Equals(left, right);
    }

    public static bool operator !=(NullableNumber<T> left, NullableNumber<T> right)
    {
        return !(left == right);
    }

    public static bool operator >(NullableNumber<T> left, NullableNumber<T> right)
    {
        return NullableNumber.Compare<NullableNumber<T>>(left, right) > 0;
    }

    public static bool operator >=(NullableNumber<T> left, NullableNumber<T> right)
    {
        return NullableNumber.Compare<NullableNumber<T>>(left, right) >= 0;
    }

    public static bool operator <(NullableNumber<T> left, NullableNumber<T> right)
    {
        return NullableNumber.Compare<NullableNumber<T>>(left, right) < 0;
    }

    public static bool operator <=(NullableNumber<T> left, NullableNumber<T> right)
    {
        return NullableNumber.Compare<NullableNumber<T>>(left, right) <= 0;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.Zero => default;

    static NullableNumber<T> INumberBase<NullableNumber<T>>.One => new(T.One);

    static int INumberBase<NullableNumber<T>>.Radix => T.Radix;

    static bool INumberBase<NullableNumber<T>>.IsCanonical(NullableNumber<T> value)
    {
        return value.HasValue && T.IsCanonical(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsComplexNumber(NullableNumber<T> value)
    {
        return value.HasValue && T.IsComplexNumber(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsFinite(NullableNumber<T> value)
    {
        return !value.HasValue || T.IsFinite(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsImaginaryNumber(NullableNumber<T> value)
    {
        return value.HasValue && T.IsImaginaryNumber(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsInfinity(NullableNumber<T> value)
    {
        return value.HasValue && T.IsInfinity(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsInteger(NullableNumber<T> value)
    {
        return value.HasValue && T.IsInteger(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsNaN(NullableNumber<T> value)
    {
        return value.HasValue && T.IsNaN(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsNegative(NullableNumber<T> value)
    {
        return value.HasValue && T.IsNegative(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsNegativeInfinity(NullableNumber<T> value)
    {
        return value.HasValue && T.IsNegativeInfinity(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsNormal(NullableNumber<T> value)
    {
        return value.HasValue && T.IsNormal(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsPositive(NullableNumber<T> value)
    {
        return value.HasValue && T.IsPositive(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsPositiveInfinity(NullableNumber<T> value)
    {
        return value.HasValue && T.IsPositiveInfinity(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsRealNumber(NullableNumber<T> value)
    {
        return !value.HasValue || T.IsRealNumber(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsSubnormal(NullableNumber<T> value)
    {
        return value.HasValue && T.IsSubnormal(value._value);
    }

    static bool INumberBase<NullableNumber<T>>.IsZero(NullableNumber<T> value)
    {
        return !value.HasValue || T.IsZero(value._value);
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.Abs(NullableNumber<T> value)
    {
        return value.HasValue ? new(T.Abs(value._value)) : value;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.MaxMagnitude(
        NullableNumber<T> x, NullableNumber<T> y)
    {
        return x.HasValue && y.HasValue ? new(T.MaxMagnitude(x._value, y._value)) : default;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.MaxMagnitudeNumber(
        NullableNumber<T> x, NullableNumber<T> y)
    {
        return x.HasValue && y.HasValue ? new(T.MaxMagnitudeNumber(x._value, y._value)) : x.HasValue ? x : y;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.MinMagnitude(
        NullableNumber<T> x, NullableNumber<T> y)
    {
        return x.HasValue && y.HasValue ? new(T.MinMagnitude(x._value, y._value)) : default;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.MinMagnitudeNumber(
        NullableNumber<T> x, NullableNumber<T> y)
    {
        return x.HasValue && y.HasValue ? new(T.MinMagnitudeNumber(x._value, y._value)) : x.HasValue ? x : y;
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.CreateChecked<TOther>(TOther value)
    {
        return new(T.CreateChecked(value));
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.CreateSaturating<TOther>(TOther value)
    {
        return new(T.CreateSaturating(value));
    }

    static NullableNumber<T> INumberBase<NullableNumber<T>>.CreateTruncating<TOther>(TOther value)
    {
        return new(T.CreateTruncating(value));
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertFromChecked<TOther>(TOther value, out NullableNumber<T> result)
    {
        if (T.TryConvertFromChecked(value, out var tmp))
        {
            result = new(tmp);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertFromSaturating<TOther>(TOther value, out NullableNumber<T> result)
    {
        if (T.TryConvertFromSaturating(value, out var tmp))
        {
            result = new(tmp);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertFromTruncating<TOther>(TOther value, out NullableNumber<T> result)
    {
        if (T.TryConvertFromTruncating(value, out var tmp))
        {
            result = new(tmp);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertToChecked<TOther>(NullableNumber<T> value, out TOther result)
    {
        result = default!;
        return value.HasValue && T.TryConvertToChecked(value._value, out result!);
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertToSaturating<TOther>(NullableNumber<T> value, out TOther result)
    {
        result = default!;
        return value.HasValue && T.TryConvertToSaturating(value._value, out result!);
    }

    static bool INumberBase<NullableNumber<T>>.TryConvertToTruncating<TOther>(NullableNumber<T> value, out TOther result)
    {
        result = default!;
        return value.HasValue && T.TryConvertToTruncating(value._value, out result!);
    }

    public static bool IsEvenInteger(NullableNumber<T> value)
    {
        return value.HasValue && T.IsEvenInteger(value._value);
    }

    public static bool IsOddInteger(NullableNumber<T> value)
    {
        return value.HasValue && T.IsOddInteger(value._value);
    }

    static NullableNumber<T> IAdditiveIdentity<NullableNumber<T>, NullableNumber<T>>.AdditiveIdentity => default;

    static NullableNumber<T> IMultiplicativeIdentity<NullableNumber<T>, NullableNumber<T>>.MultiplicativeIdentity => new(T.MultiplicativeIdentity);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (!HasValue)
        {
            if (NullableNumber.NoValueLabel.TryCopyTo(destination))
            {
                charsWritten = NullableNumber.NoValueLabel.Length;
                return true;
            }

            charsWritten = 0;
            return false;
        }

        return _value.TryFormat(destination, out charsWritten, format, provider);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return HasValue ? _value.ToString(format, formatProvider) : string.Empty;
    }

    public static NullableNumber<T> Parse(string s, IFormatProvider? provider)
    {
        return new(T.Parse(s, provider));
    }

    public static NullableNumber<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        if (TryParse(s, style, provider, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(NullableNumber<>)}: {s}");
        return default; // unreachable
    }

    public static NullableNumber<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return Parse(s.AsSpan(), style, provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullableNumber<T> result)
    {
        if (s == NullableNumber.NoValueLabel)
        {
            result = default;
            return true;
        }

        if (T.TryParse(s, style, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        NumberStyles style,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullableNumber<T> result)
    {
        return TryParse(s.AsSpan(), style, provider, out result);
    }

    public static NullableNumber<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return Parse(s, NumberStyles.Any, provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullableNumber<T> result)
    {
        return TryParse(s, NumberStyles.Any, provider, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullableNumber<T> result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
