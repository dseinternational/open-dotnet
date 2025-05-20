// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static class NaValue
{
    public const string NaValueLabel = "NA";
    internal const string NanValueLabel = "NAN";
    internal const string NullValueLabel = "NULL";

    public static bool Equals<T>(NaValue<T> n1, T n2)
        where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
    {
        return n1.HasValue && n2.Equals(n1.Value);
    }

    public static bool Equals<T>(NaValue<T> n1, NaValue<T> n2)
        where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
    {
        return n2.HasValue && n1.HasValue && n2.Value.Equals(n1.Value);
    }

    public static int Compare<T>(NaValue<T> n1, NaValue<T> n2)
        where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
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

public readonly struct NaValue<T> :
    INaValue<NaValue<T>, T>,
    IComparable<NaValue<T>>,
    IComparable,
    ISpanParsable<NaValue<T>>
    where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
{
    public static NaValue<T> Na { get; }

    private readonly bool _hasValue;
    private readonly T _value;

    public bool HasValue => _hasValue;

    public T Value => HasValue ? _value : throw new NaValueException();

    private NaValue(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static implicit operator NaValue<T>(T? value)
    {
        if (value is null)
        {
            return default;
        }

        return new(value);
    }

    public static explicit operator T(NaValue<T> value)
    {
        return value.Value;
    }

    public override string ToString()
    {
        return _hasValue ? _value.ToString() ?? string.Empty : NaValue.NaValueLabel;
    }

    public override int GetHashCode()
    {
        return _hasValue ? _value.GetHashCode() : 0;
    }

    public bool Equals(NaValue<T> other)
    {
        return NaValue.Equals(this, other);
    }

    public bool Equals(T value)
    {
        return NaValue.Equals(this, value);
    }

    public override bool Equals(object? obj)
    {
        return (obj is NaValue<T> other && Equals(other))
            || (obj is T n && Equals(n));
    }

    public int CompareTo(NaValue<T> other)
    {
        return NaValue.Compare(this, other);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NaValue<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NaValue)}", nameof(obj));
    }

    public static bool operator ==(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Equals(left, right);
    }

    public static bool operator !=(NaValue<T> left, NaValue<T> right)
    {
        return !(left == right);
    }

    public static bool operator >(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) > 0;
    }

    public static bool operator >=(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) >= 0;
    }

    public static bool operator <(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) < 0;
    }

    public static bool operator <=(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) <= 0;
    }

    public static NaValue<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(NaValue)}: {s}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NaValue<T> result)
    {
        if (s == NaValue.NaValueLabel)
        {
            result = default;
            return true;
        }

        if (T.TryParse(s, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    public static NaValue<T> Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NaValue<T> result)
    {
        throw new NotImplementedException();
    }
}
