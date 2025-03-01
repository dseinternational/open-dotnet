// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Represents a date as the whole number of milliseconds since the Unix epoch (1970-01-01T00:00:00Z).
/// </summary>
[JsonConverter(typeof(JsonDateTime64NumberConverter))]
public readonly record struct DateTime64 : INumber<DateTime64>
{
    internal const long MinMilliseconds = -62135596800000L;
    internal const long MaxMilliseconds = 253402300799999L;

    public static readonly DateTime64 MinValue = new(MinMilliseconds);

    public static readonly DateTime64 MaxValue = new(MaxMilliseconds);

    public DateTime64(long millisecondsSinceUnixEpoch)
    {
        Guard.IsInRange(millisecondsSinceUnixEpoch, MinMilliseconds, MaxMilliseconds + 1, nameof(millisecondsSinceUnixEpoch));
        TotalMilliseconds = millisecondsSinceUnixEpoch;
    }

    public DateTime64(DateTimeOffset dateTime)
    {
        TotalMilliseconds = dateTime.ToUnixTimeMilliseconds();
    }

    public DateTime Date => DateTimeOffset.FromUnixTimeMilliseconds(TotalMilliseconds).Date;

    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(TotalMilliseconds).DateTime;

    public DateTime UtcDateTime => DateTimeOffset.FromUnixTimeMilliseconds(TotalMilliseconds).UtcDateTime;

    public long TotalMilliseconds { get; }

    public DateTimeOffset ToDateTimeOffset()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(TotalMilliseconds);
    }

    public static DateTime64 Now => DateTimeOffset.Now;

    public static DateTime64 UtcNow => DateTimeOffset.UtcNow;

    public static DateTime64 FromUnixTimeMilliseconds(long milliseconds)
    {
        return new(milliseconds);
    }

    public static DateTime64 FromUnixTimeSeconds(long seconds)
    {
        return new(seconds * 1000);
    }

    public static DateTime64 One { get; } = new(1L);

    static int INumberBase<DateTime64>.Radix { get; } = 2;

    public static DateTime64 Zero { get; } = new(0L);

    static DateTime64 IAdditiveIdentity<DateTime64, DateTime64>.AdditiveIdentity => Zero;

    static DateTime64 IMultiplicativeIdentity<DateTime64, DateTime64>.MultiplicativeIdentity => One;

    static DateTime64 INumberBase<DateTime64>.Abs(DateTime64 value)
    {
        return new(long.Abs(value.TotalMilliseconds));
    }

    static bool INumberBase<DateTime64>.IsCanonical(DateTime64 value)
    {
        return true;
    }

    static bool INumberBase<DateTime64>.IsComplexNumber(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsEvenInteger(DateTime64 value)
    {
        return long.IsEvenInteger(value.TotalMilliseconds);
    }

    static bool INumberBase<DateTime64>.IsFinite(DateTime64 value)
    {
        return true;
    }

    static bool INumberBase<DateTime64>.IsImaginaryNumber(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsInfinity(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsInteger(DateTime64 value)
    {
        return true;
    }

    static bool INumberBase<DateTime64>.IsNaN(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsNegative(DateTime64 value)
    {
        return long.IsNegative(value.TotalMilliseconds);
    }

    static bool INumberBase<DateTime64>.IsNegativeInfinity(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsNormal(DateTime64 value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<DateTime64>.IsOddInteger(DateTime64 value)
    {
        return long.IsOddInteger(value.TotalMilliseconds);
    }

    static bool INumberBase<DateTime64>.IsPositive(DateTime64 value)
    {
        return long.IsPositive(value.TotalMilliseconds);
    }

    static bool INumberBase<DateTime64>.IsPositiveInfinity(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsRealNumber(DateTime64 value)
    {
        return true;
    }

    static bool INumberBase<DateTime64>.IsSubnormal(DateTime64 value)
    {
        return false;
    }

    static bool INumberBase<DateTime64>.IsZero(DateTime64 value)
    {
        return value.TotalMilliseconds == 0L;
    }

    static DateTime64 INumberBase<DateTime64>.MaxMagnitude(DateTime64 x, DateTime64 y)
    {
        return new(long.MaxMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static DateTime64 INumberBase<DateTime64>.MaxMagnitudeNumber(DateTime64 x, DateTime64 y)
    {
        return new(long.MaxMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static DateTime64 INumberBase<DateTime64>.MinMagnitude(DateTime64 x, DateTime64 y)
    {
        return new(long.MinMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static DateTime64 INumberBase<DateTime64>.MinMagnitudeNumber(DateTime64 x, DateTime64 y)
    {
        return new(long.MinMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    public static DateTime64 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return new DateTime64(long.Parse(s, style, provider));
    }

    public static DateTime64 Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return new DateTime64(long.Parse(s, style, provider));
    }

    public static DateTime64 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return new DateTime64(long.Parse(s, provider));
    }

    public static DateTime64 Parse(string s, IFormatProvider? provider)
    {
        return new DateTime64(long.Parse(s, provider));
    }

    static bool INumberBase<DateTime64>.TryConvertFromChecked<TOther>(TOther value, out DateTime64 result)
    {
        if (NumberHelper.TryConvertToInt64Checked(value, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<DateTime64>.TryConvertFromSaturating<TOther>(TOther value, out DateTime64 result)
    {
        if (NumberHelper.TryConvertToInt64Saturating(value, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<DateTime64>.TryConvertFromTruncating<TOther>(TOther value, out DateTime64 result)
    {
        if (NumberHelper.TryConvertToInt64Truncating(value, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<DateTime64>.TryConvertToChecked<TOther>(DateTime64 value, out TOther result)
    {
        return TOther.TryConvertFromChecked(value.TotalMilliseconds, out result!);
    }

    static bool INumberBase<DateTime64>.TryConvertToSaturating<TOther>(DateTime64 value, out TOther result)
    {
        return TOther.TryConvertFromSaturating(value.TotalMilliseconds, out result!);
    }

    static bool INumberBase<DateTime64>.TryConvertToTruncating<TOther>(DateTime64 value, out TOther result)
    {
        return TOther.TryConvertFromTruncating(value.TotalMilliseconds, out result!);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        out DateTime64 result)
    {
        if (long.TryParse(s, style, provider, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        string? s,
        NumberStyles style,
        IFormatProvider? provider,
        out DateTime64 result)
    {
        if (long.TryParse(s, style, provider, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateTime64 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out DateTime64 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var ms))
        {
            result = new DateTime64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public bool Equals(DateTime64 other)
    {
        return TotalMilliseconds == other.TotalMilliseconds;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TotalMilliseconds);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return TotalMilliseconds.ToString(format, formatProvider);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return TotalMilliseconds.TryFormat(destination, out charsWritten, format, provider);
    }

    public int CompareTo(object? obj)
    {
        return TotalMilliseconds.CompareTo(obj);
    }

    public int CompareTo(DateTime64 other)
    {
        return TotalMilliseconds.CompareTo(other.TotalMilliseconds);
    }

    public DateTime64 Add(DateTime64 value)
    {
        return new(TotalMilliseconds + value.TotalMilliseconds);
    }

    private DateTime64 Divide(DateTime64 value)
    {
        return new(TotalMilliseconds / value.TotalMilliseconds);
    }

    private DateTime64 Multiply(DateTime64 value)
    {
        return new(TotalMilliseconds * value.TotalMilliseconds);
    }

    private DateTime64 Subtract(DateTime64 value)
    {
        return new(TotalMilliseconds - value.TotalMilliseconds);
    }

    static DateTime64 IUnaryPlusOperators<DateTime64, DateTime64>.operator +(DateTime64 value)
    {
        return value;
    }

    static DateTime64 IAdditionOperators<DateTime64, DateTime64, DateTime64>.operator +(DateTime64 left, DateTime64 right)
    {
        return left.Add(right);
    }

    static DateTime64 IUnaryNegationOperators<DateTime64, DateTime64>.operator -(DateTime64 value)
    {
        return value.Subtract(One);
    }

    static DateTime64 ISubtractionOperators<DateTime64, DateTime64, DateTime64>.operator -(DateTime64 left, DateTime64 right)
    {
        return left.Subtract(right);
    }

    static DateTime64 IIncrementOperators<DateTime64>.operator ++(DateTime64 value)
    {
        return value.Add(One);
    }

    static DateTime64 IDecrementOperators<DateTime64>.operator --(DateTime64 value)
    {
        return value.Subtract(One);
    }

    static DateTime64 IMultiplyOperators<DateTime64, DateTime64, DateTime64>.operator *(DateTime64 left, DateTime64 right)
    {
        return left.Multiply(right);
    }

    static DateTime64 IDivisionOperators<DateTime64, DateTime64, DateTime64>.operator /(DateTime64 left, DateTime64 right)
    {
        return left.Divide(right);
    }

    static bool IEqualityOperators<DateTime64, DateTime64, bool>.operator ==(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds == right.TotalMilliseconds;
    }

    static bool IEqualityOperators<DateTime64, DateTime64, bool>.operator !=(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds != right.TotalMilliseconds;
    }

    public static bool operator >(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds > right.TotalMilliseconds;
    }

    public static bool operator >=(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds >= right.TotalMilliseconds;
    }

    public static bool operator <(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds < right.TotalMilliseconds;
    }

    public static bool operator <=(DateTime64 left, DateTime64 right)
    {
        return left.TotalMilliseconds <= right.TotalMilliseconds;
    }

    public static DateTime64 operator %(DateTime64 left, DateTime64 right)
    {
        return Remainder(left, right);
    }

    public static DateTime64 Remainder(DateTime64 left, DateTime64 right)
    {
        return new DateTime64(left.TotalMilliseconds % right.TotalMilliseconds);
    }

    public static implicit operator DateTimeOffset(DateTime64 value)
    {
        return value.ToDateTimeOffset();
    }

    public static DateTime64 FromDateTimeOffset(DateTimeOffset value)
    {
        return new(value);
    }

    public static implicit operator DateTime64(DateTimeOffset value)
    {
        return FromDateTimeOffset(value);
    }
}
