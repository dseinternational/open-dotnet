// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Represents a date as the whole number of milliseconds since the Unix epoch (1970-01-01T00:00:00Z).
/// </summary>
[JsonConverter(typeof(JsonDate64NumberConverter))]
public readonly record struct Date64 : INumber<Date64>
{
    internal const long MinMilliseconds = -62135596800000L;
    internal const long MaxMilliseconds = 253402300799999L;

    public static readonly Date64 MinValue = new(MinMilliseconds);

    public static readonly Date64 MaxValue = new(MaxMilliseconds);

    public Date64(long millisecondsSinceUnixEpoch)
    {
        Guard.IsInRange(millisecondsSinceUnixEpoch, MinMilliseconds, MaxMilliseconds + 1, nameof(millisecondsSinceUnixEpoch));
        TotalMilliseconds = millisecondsSinceUnixEpoch;
    }

    public Date64(DateTimeOffset dateTime)
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

    public static Date64 Now => DateTimeOffset.Now;

    public static Date64 UtcNow => DateTimeOffset.UtcNow;

    public static Date64 FromUnixTimeMilliseconds(long milliseconds)
    {
        return new(milliseconds);
    }

    public static Date64 FromUnixTimeSeconds(long seconds)
    {
        return new(seconds * 1000);
    }

    public static Date64 One { get; } = new(1L);

    static int INumberBase<Date64>.Radix { get; } = 2;

    public static Date64 Zero { get; } = new(0L);

    static Date64 IAdditiveIdentity<Date64, Date64>.AdditiveIdentity => Zero;

    static Date64 IMultiplicativeIdentity<Date64, Date64>.MultiplicativeIdentity => One;

    static Date64 INumberBase<Date64>.Abs(Date64 value)
    {
        return new(long.Abs(value.TotalMilliseconds));
    }

    static bool INumberBase<Date64>.IsCanonical(Date64 value)
    {
        return true;
    }

    static bool INumberBase<Date64>.IsComplexNumber(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsEvenInteger(Date64 value)
    {
        return long.IsEvenInteger(value.TotalMilliseconds);
    }

    static bool INumberBase<Date64>.IsFinite(Date64 value)
    {
        return true;
    }

    static bool INumberBase<Date64>.IsImaginaryNumber(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsInfinity(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsInteger(Date64 value)
    {
        return true;
    }

    static bool INumberBase<Date64>.IsNaN(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsNegative(Date64 value)
    {
        return long.IsNegative(value.TotalMilliseconds);
    }

    static bool INumberBase<Date64>.IsNegativeInfinity(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsNormal(Date64 value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Date64>.IsOddInteger(Date64 value)
    {
        return long.IsOddInteger(value.TotalMilliseconds);
    }

    static bool INumberBase<Date64>.IsPositive(Date64 value)
    {
        return long.IsPositive(value.TotalMilliseconds);
    }

    static bool INumberBase<Date64>.IsPositiveInfinity(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsRealNumber(Date64 value)
    {
        return true;
    }

    static bool INumberBase<Date64>.IsSubnormal(Date64 value)
    {
        return false;
    }

    static bool INumberBase<Date64>.IsZero(Date64 value)
    {
        return value.TotalMilliseconds == 0L;
    }

    static Date64 INumberBase<Date64>.MaxMagnitude(Date64 x, Date64 y)
    {
        return new(long.MaxMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static Date64 INumberBase<Date64>.MaxMagnitudeNumber(Date64 x, Date64 y)
    {
        return new(long.MaxMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static Date64 INumberBase<Date64>.MinMagnitude(Date64 x, Date64 y)
    {
        return new(long.MinMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    static Date64 INumberBase<Date64>.MinMagnitudeNumber(Date64 x, Date64 y)
    {
        return new(long.MinMagnitude(x.TotalMilliseconds, y.TotalMilliseconds));
    }

    public static Date64 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return new Date64(long.Parse(s, style, provider));
    }

    public static Date64 Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return new Date64(long.Parse(s, style, provider));
    }

    public static Date64 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return new Date64(long.Parse(s, provider));
    }

    public static Date64 Parse(string s, IFormatProvider? provider)
    {
        return new Date64(long.Parse(s, provider));
    }

    static bool INumberBase<Date64>.TryConvertFromChecked<TOther>(TOther value, out Date64 result)
    {
        if (NumberHelper.TryConvertToInt64Checked(value, out var ms))
        {
            result = new Date64(ms);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<Date64>.TryConvertFromSaturating<TOther>(TOther value, out Date64 result)
    {
        // todo: add implementation to NumberHelper
        throw new NotImplementedException();
    }

    static bool INumberBase<Date64>.TryConvertFromTruncating<TOther>(TOther value, out Date64 result)
    {
        // todo: add implementation to NumberHelper
        throw new NotImplementedException();
    }

    static bool INumberBase<Date64>.TryConvertToChecked<TOther>(Date64 value, out TOther result)
    {
        // todo: add implementation to NumberHelper
        throw new NotImplementedException();
    }

    static bool INumberBase<Date64>.TryConvertToSaturating<TOther>(Date64 value, out TOther result)
    {
        // todo: add implementation to NumberHelper
        throw new NotImplementedException();
    }

    static bool INumberBase<Date64>.TryConvertToTruncating<TOther>(Date64 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        NumberStyles style,
        IFormatProvider? provider,
        out Date64 result)
    {
        if (long.TryParse(s, style, provider, out var ms))
        {
            result = new Date64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        string? s,
        NumberStyles style,
        IFormatProvider? provider,
        out Date64 result)
    {
        if (long.TryParse(s, style, provider, out var ms))
        {
            result = new Date64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Date64 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var ms))
        {
            result = new Date64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Date64 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var ms))
        {
            result = new Date64(ms);
            return true;
        }

        result = default;
        return false;
    }

    public bool Equals(Date64 other)
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

    public int CompareTo(Date64 other)
    {
        return TotalMilliseconds.CompareTo(other.TotalMilliseconds);
    }

    static Date64 IUnaryPlusOperators<Date64, Date64>.operator +(Date64 value)
    {
        throw new NotImplementedException();
    }

    static Date64 IAdditionOperators<Date64, Date64, Date64>.operator +(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    static Date64 IUnaryNegationOperators<Date64, Date64>.operator -(Date64 value)
    {
        throw new NotImplementedException();
    }

    static Date64 ISubtractionOperators<Date64, Date64, Date64>.operator -(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    static Date64 IIncrementOperators<Date64>.operator ++(Date64 value)
    {
        throw new NotImplementedException();
    }

    static Date64 IDecrementOperators<Date64>.operator --(Date64 value)
    {
        throw new NotImplementedException();
    }

    static Date64 IMultiplyOperators<Date64, Date64, Date64>.operator *(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    static Date64 IDivisionOperators<Date64, Date64, Date64>.operator /(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    static bool IEqualityOperators<Date64, Date64, bool>.operator ==(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    static bool IEqualityOperators<Date64, Date64, bool>.operator !=(Date64 left, Date64 right)
    {
        throw new NotImplementedException();
    }

    public static bool operator >(Date64 left, Date64 right)
    {
        return left.TotalMilliseconds > right.TotalMilliseconds;
    }

    public static bool operator >=(Date64 left, Date64 right)
    {
        return left.TotalMilliseconds >= right.TotalMilliseconds;
    }

    public static bool operator <(Date64 left, Date64 right)
    {
        return left.TotalMilliseconds < right.TotalMilliseconds;
    }

    public static bool operator <=(Date64 left, Date64 right)
    {
        return left.TotalMilliseconds <= right.TotalMilliseconds;
    }

    public static Date64 operator %(Date64 left, Date64 right)
    {
        return Remainder(left, right);
    }

    public static Date64 Remainder(Date64 left, Date64 right)
    {
        return new Date64(left.TotalMilliseconds % right.TotalMilliseconds);
    }

    public static implicit operator DateTimeOffset(Date64 value)
    {
        return value.ToDateTimeOffset();
    }

    public static Date64 FromDateTimeOffset(DateTimeOffset value)
    {
        return new(value);
    }

    public static implicit operator Date64(DateTimeOffset value)
    {
        return FromDateTimeOffset(value);
    }
}
