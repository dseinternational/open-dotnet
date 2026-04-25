// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// A floating-point value that may be 'not a value', missing, or not available, encoded
/// using the underlying type's <see cref="IFloatingPointIeee754{TSelf}.NaN"/> as the NA
/// sentinel.
/// </summary>
/// <typeparam name="T">The underlying IEEE 754 floating-point type.</typeparam>
/// <remarks>
/// Because IEEE 754 NaN already propagates through arithmetic operations as NaN, most
/// arithmetic on <see cref="NaFloat{T}"/> can simply delegate to the underlying type
/// without explicit NA checks — the propagation is handled by the float math itself.
/// Equality is the exception: <see cref="IEquatable{T}.Equals"/> treats two NA values
/// as equal (so the type can be a dictionary key), while <c>operator ==</c> follows
/// the IEEE 754 rule that NaN is unequal to itself.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct NaFloat<T>
    : INaNumber<NaFloat<T>, T>
      where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    /// <summary>The reserved underlying value used to encode NA (always <see cref="IFloatingPointIeee754{TSelf}.NaN"/>).</summary>
    public static T Sentinel => T.NaN;

    /// <summary>Gets the canonical NA value.</summary>
    public static NaFloat<T> Na { get; } = new(T.NaN);

    /// <summary>The maximum representable value (<see cref="IMinMaxValue{T}.MaxValue"/>).</summary>
    public static NaFloat<T> MaxValue { get; } = new(T.MaxValue);

    /// <summary>The minimum representable value (<see cref="IMinMaxValue{T}.MinValue"/>).</summary>
    public static NaFloat<T> MinValue { get; } = new(T.MinValue);

    static NaFloat<T> INaValue<NaFloat<T>, T>.Na { get; } = Na;

    private readonly T _value;

    /// <summary>Wraps <paramref name="value"/>. Pass <see cref="IFloatingPointIeee754{TSelf}.NaN"/> to construct an NA value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NaFloat(T value)
    {
        _value = value;
    }

    /// <summary>Wraps <paramref name="value"/>. Pass <see cref="IFloatingPointIeee754{TSelf}.NaN"/> to construct an NA value.</summary>
    public static NaFloat<T> FromValue(T value)
    {
        return new(value);
    }

    bool INaValue.HasValue => !T.IsNaN(_value);

    T INaValue<NaFloat<T>, T>.Value => _value;

    /// <summary>Gets <see langword="true"/> when this value is NA (i.e., the underlying value is NaN).</summary>
    public bool IsNa => T.IsNaN(_value);

    /// <summary>
    /// Returns <see langword="true"/> when both values are NA, or both are non-NA and
    /// the underlying values are bit-equal. Mirrors the dictionary-friendly contract
    /// expected by <see cref="IEquatable{T}"/>; the <c>==</c> operator instead follows
    /// IEEE 754 semantics (NaN unequal to NaN).
    /// </summary>
    public bool Equals(NaFloat<T> other)
    {
        // bool Equals(T) is true for NaFloat<T> Na == NaFloat<T> Na
        // bool == operator is false for NaFloat<T> Na == NaFloat<T> Na
        // as https://learn.microsoft.com/en-us/dotnet/api/system.single.nan?view=net-9.0#remarks
        return EqualOrBothNa(other);
    }

    /// <inheritdoc />
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

    /// <summary>
    /// Three-valued equality: returns <see cref="Trilean.Na"/> when either side is
    /// NA, <see cref="Trilean.True"/> when both sides have equal underlying values,
    /// and <see cref="Trilean.False"/> otherwise.
    /// </summary>
    public Trilean TernaryEquals(NaFloat<T> other)
    {
        if (IsNa || other.IsNa)
        {
            return Trilean.Na;
        }

        return _value == other._value ? Trilean.True : Trilean.False;
    }

    /// <summary>Returns <see langword="true"/> only when both values are non-NA and equal.</summary>
    public bool EqualAndNotNa(NaFloat<T> other)
    {
        return !IsNa && !other.IsNa && _value == other._value;
    }

    /// <summary>Returns <see langword="true"/> when both values are NA, or both are non-NA and equal.</summary>
    public bool EqualOrBothNa(NaFloat<T> other)
    {
        return (IsNa && other.IsNa) || (!IsNa && !other.IsNa && _value == other._value);
    }

    /// <summary>Returns <see langword="true"/> when either value is NA, or both are equal.</summary>
    public bool EqualOrEitherNa(NaFloat<T> other)
    {
        return IsNa || other.IsNa || _value == other._value;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return IsNa ? 0 : _value.GetHashCode();
    }

    /// <summary>Implicitly wraps an underlying <typeparamref name="T"/>.</summary>
    public static implicit operator NaFloat<T>(T value)
    {
        return new(value);
    }

    /// <summary>Implicitly converts from <see cref="Nullable{T}"/>: <see langword="null"/> becomes <see cref="Na"/>.</summary>
    public static implicit operator NaFloat<T>(T? value)
    {
        if (value is null)
        {
            return Na;
        }

        return new(value.Value);
    }

    /// <summary>Implicitly extracts the underlying <typeparamref name="T"/>; returns NaN for an NA value.</summary>
    public static implicit operator T(NaFloat<T> value)
    {
        return value._value;
    }

    // -----------------------------------------------------------------
    // INumberBase

    /// <summary>The numeric value one.</summary>
    public static NaFloat<T> One => T.One;

    static int INumberBase<NaFloat<T>>.Radix => T.Radix;

    /// <summary>The numeric value zero.</summary>
    public static NaFloat<T> Zero => new(T.Zero);

    static NaFloat<T> IAdditiveIdentity<NaFloat<T>, NaFloat<T>>.AdditiveIdentity => new(T.Zero);

    static NaFloat<T> IMultiplicativeIdentity<NaFloat<T>, NaFloat<T>>.MultiplicativeIdentity => new(T.One);

    /// <summary>Greater-than. Always <see langword="false"/> when either side is NA (IEEE 754 NaN comparison).</summary>
    public static bool operator >(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value > right._value;
    }

    /// <summary>Greater-than-or-equal. Always <see langword="false"/> when either side is NA.</summary>
    public static bool operator >=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value >= right._value;
    }

    /// <summary>Less-than. Always <see langword="false"/> when either side is NA.</summary>
    public static bool operator <(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value < right._value;
    }

    /// <summary>Less-than-or-equal. Always <see langword="false"/> when either side is NA.</summary>
    public static bool operator <=(NaFloat<T> left, NaFloat<T> right)
    {
        return left._value <= right._value;
    }

    /// <summary>Modulo. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator %(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value % right._value);
    }

    /// <summary>Adds two values. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator +(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value + right._value);
    }

    /// <summary>Pre-decrement. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator --(NaFloat<T> value)
    {
        var v = value._value;
        return new NaFloat<T>(--v);
    }

    /// <summary>Divides two values. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator /(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value / right._value);
    }

    /// <summary>Pre-increment. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator ++(NaFloat<T> value)
    {
        var v = value._value;
        return new NaFloat<T>(++v);
    }

    /// <summary>Multiplies two values. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator *(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value * right._value);
    }

    /// <summary>Subtracts two values. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator -(NaFloat<T> left, NaFloat<T> right)
    {
        return new NaFloat<T>(left._value - right._value);
    }

    /// <summary>Negation. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator -(NaFloat<T> value)
    {
        return new NaFloat<T>(-value._value);
    }

    /// <summary>Unary plus. NA propagates through IEEE 754 NaN semantics.</summary>
    public static NaFloat<T> operator +(NaFloat<T> value)
    {
        return new NaFloat<T>(+value._value);
    }

    /// <summary>
    /// Equality operator. Mirrors <see cref="float.NaN"/>: <c>Na == Na</c> is
    /// <see langword="false"/>. Use <see cref="Equals(NaFloat{T})"/> for the
    /// <see cref="IEquatable{T}"/>-style comparison that treats NA as self-equal.
    /// </summary>
    public static bool operator ==(NaFloat<T> left, NaFloat<T> right)
    {
        // NaN == NaN: False
        return left._value == right._value;
    }

    /// <summary>Inequality operator. See <see cref="op_Equality(NaFloat{T}, NaFloat{T})"/>.</summary>
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

    /// <summary>Compares this value to <paramref name="other"/>; delegates to the underlying type's comparison.</summary>
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

    /// <summary>Returns <see langword="true"/> when <paramref name="value"/> is NA (NaN-encoded).</summary>
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

    /// <summary>Parses a value from <paramref name="s"/> using the given styles and provider.</summary>
    public static NaFloat<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return new(T.Parse(s, style, provider));
    }

    /// <summary>Parses a value from <paramref name="s"/> using the given styles and provider.</summary>
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
        if (typeof(TOther) == typeof(NaFloat<T>))
        {
            result = (TOther)(object)value;
            return true;
        }

        // Mirror CreateChecked's two-direction fallback so that both T's own
        // narrowing conversions and TOther's widening conversions are reachable.
        return T.TryConvertToChecked(value._value, out result!)
            || TOther.TryConvertFromChecked(value._value, out result!);
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToSaturating<TOther>(NaFloat<T> value, out TOther result)
    {
        if (typeof(TOther) == typeof(NaFloat<T>))
        {
            result = (TOther)(object)value;
            return true;
        }

        return T.TryConvertToSaturating(value._value, out result!)
            || TOther.TryConvertFromSaturating(value._value, out result!);
    }

    static bool INumberBase<NaFloat<T>>.TryConvertToTruncating<TOther>(NaFloat<T> value, out TOther result)
    {
        if (typeof(TOther) == typeof(NaFloat<T>))
        {
            result = (TOther)(object)value;
            return true;
        }

        return T.TryConvertToTruncating(value._value, out result!)
            || TOther.TryConvertFromTruncating(value._value, out result!);
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
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

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
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

    /// <summary>
    /// Tries to format the value into <paramref name="destination"/>. NA renders
    /// as <see cref="NaValue.NaValueLabel"/>; non-NA values delegate to the
    /// underlying <typeparamref name="T"/> formatting.
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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

        if (_value.TryFormat(destination, out charsWritten, format, provider))
        {
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <summary>
    /// Returns the formatted string. NA renders as <see cref="NaValue.NaValueLabel"/>;
    /// non-NA values delegate to <typeparamref name="T"/>'s formatter.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return IsNa ? NaValue.NaValueLabel : _value.ToString(format, formatProvider);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return IsNa ? NaValue.NaValueLabel : _value.ToString() ?? string.Empty;
    }

    /// <summary>Parses a value from <paramref name="s"/>; returns <see cref="Na"/> on parse failure.</summary>
    public static NaFloat<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            return new(valResult);
        }

        return Na;
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
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

    /// <summary>Parses a value from <paramref name="s"/>; returns <see cref="Na"/> on parse failure.</summary>
    public static NaFloat<T> Parse(string s, IFormatProvider? provider)
    {
        if (T.TryParse(s, provider, out var valResult))
        {
            return new(valResult);
        }

        return Na;
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
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
