// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static class NullablePrimitiveValue
{
    public const string NoValueLabel = NullableValue.NoValueLabel;

    public static bool Equals<T>(NullablePrimitiveValue<T> n1, T n2)
        where T : unmanaged, IEquatable<T>, IComparable<T>, ISpanParsable<T>, ISpanFormattable
    {
        return n1.HasValue && n2.Equals(n1.Value);
    }

    public static bool Equals<T>(NullablePrimitiveValue<T> n1, NullablePrimitiveValue<T> n2)
        where T : unmanaged, IEquatable<T>, IComparable<T>, ISpanParsable<T>, ISpanFormattable
    {
        return n2.HasValue && n1.HasValue && n2.Value.Equals(n1.Value);
    }

    public static int Compare<T>(NullablePrimitiveValue<T> n1, NullablePrimitiveValue<T> n2)
        where T : unmanaged, IEquatable<T>, IComparable<T>, ISpanParsable<T>, ISpanFormattable
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

public readonly struct NullablePrimitiveValue<T> :
    INullable<NullablePrimitiveValue<T>, T>,
    IComparable<NullablePrimitiveValue<T>>,
    IComparable,
    ISpanParsable<NullablePrimitiveValue<T>>,
    ISpanFormattable
    where T : unmanaged, IEquatable<T>, IComparable<T>, ISpanParsable<T>, ISpanFormattable
{
    private readonly bool _hasValue;
    private readonly T _value;

    public bool HasValue => _hasValue;

    public T Value => HasValue ? _value : throw new InvalidOperationException();

    private NullablePrimitiveValue(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static implicit operator NullablePrimitiveValue<T>(T value)
    {
        return new(value);
    }

    public static explicit operator T(NullablePrimitiveValue<T> value)
    {
        return value.Value;
    }

    public override string ToString()
    {
        return _hasValue ? _value.ToString() ?? string.Empty : NullablePrimitiveValue.NoValueLabel;
    }

    public override int GetHashCode()
    {
        return _hasValue ? _value.GetHashCode() : 0;
    }

    public bool Equals(NullablePrimitiveValue<T> other)
    {
        return NullablePrimitiveValue.Equals(this, other);
    }

    public bool Equals(T value)
    {
        return NullablePrimitiveValue.Equals(this, value);
    }

    public override bool Equals(object? obj)
    {
        return (obj is NullablePrimitiveValue<T> other && Equals(other))
            || (obj is T n && Equals(n));
    }

    public int CompareTo(NullablePrimitiveValue<T> other)
    {
        return NullablePrimitiveValue.Compare<NullablePrimitiveValue<T>>(this, other);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is NullablePrimitiveValue<T> other
            ? CompareTo(other)
            : throw new ArgumentException($"Object is not a {nameof(NullablePrimitiveValue<>)}", nameof(obj));
    }

    public static bool operator ==(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return NullablePrimitiveValue.Equals(left, right);
    }

    public static bool operator !=(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return !(left == right);
    }

    public static bool operator >(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return NullablePrimitiveValue.Compare<NullablePrimitiveValue<T>>(left, right) > 0;
    }

    public static bool operator >=(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return NullablePrimitiveValue.Compare<NullablePrimitiveValue<T>>(left, right) >= 0;
    }

    public static bool operator <(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return NullablePrimitiveValue.Compare<NullablePrimitiveValue<T>>(left, right) < 0;
    }

    public static bool operator <=(NullablePrimitiveValue<T> left, NullablePrimitiveValue<T> right)
    {
        return NullablePrimitiveValue.Compare<NullablePrimitiveValue<T>>(left, right) <= 0;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (!HasValue)
        {
            if (NullablePrimitiveValue.NoValueLabel.TryCopyTo(destination))
            {
                charsWritten = NullablePrimitiveValue.NoValueLabel.Length;
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

    public static NullablePrimitiveValue<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(NullablePrimitiveValue<>)}: {s}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullablePrimitiveValue<T> result)
    {
        if (s == NullablePrimitiveValue.NoValueLabel)
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

    public static NullablePrimitiveValue<T> Parse(string s, IFormatProvider? provider)
    {
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NullablePrimitiveValue<T> result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
