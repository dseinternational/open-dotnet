// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>
/// Static helpers for <see cref="NaValue{T}"/> — comparison, equality and the
/// shared label constants used by NA-aware formatting across the project.
/// </summary>
public static class NaValue
{
    /// <summary>The label rendered for NA values by <see cref="NaValue{T}.ToString()"/>, <see cref="NaInt{T}"/>, and <see cref="NaFloat{T}"/>.</summary>
    public const string NaValueLabel = "NA";
    internal const string NanValueLabel = "NAN";
    internal const string NullValueLabel = "NULL";

    /// <summary>Returns <see langword="true"/> when <paramref name="n1"/> has a value equal to <paramref name="n2"/>.</summary>
    public static bool Equals<T>(NaValue<T> n1, T n2)
        where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
    {
        return n1.HasValue && n2.Equals(n1.Value);
    }

    /// <summary>Returns <see langword="true"/> only when both values are non-NA and equal.</summary>
    public static bool EqualAndNotNa<T>(NaValue<T> n1, NaValue<T> n2)
        where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
    {
        return n2.HasValue && n1.HasValue && n2.Value.Equals(n1.Value);
    }

    /// <summary>
    /// Compares two values. NA sorts before all other values and equals only itself,
    /// matching the convention used by <see cref="NaInt{T}"/>.
    /// </summary>
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

/// <summary>
/// A value of type <typeparamref name="T"/> that may also be NA (missing / not
/// available). Used as the NA-aware container for non-numeric types
/// (<see cref="bool"/>, <see cref="char"/>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, <see cref="string"/>) where the
/// numeric-specific <see cref="NaInt{T}"/> / <see cref="NaFloat{T}"/> wrappers
/// don't apply.
/// </summary>
/// <typeparam name="T">The underlying value type.</typeparam>
public readonly struct NaValue<T> :
    INaValue<NaValue<T>, T>,
    IComparable<NaValue<T>>,
    IComparable,
    ISpanParsable<NaValue<T>>
    where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
{
    /// <summary>Gets the canonical NA value (the default, with <see cref="HasValue"/> = <see langword="false"/>).</summary>
    public static NaValue<T> Na { get; }

    private readonly bool _hasValue;
    private readonly T _value;

    /// <summary>Gets <see langword="true"/> when this instance carries a non-NA value.</summary>
    public bool HasValue => _hasValue;

    /// <summary>Gets the underlying value, or throws when this instance is NA.</summary>
    /// <exception cref="NaValueException">The value is NA.</exception>
    public T Value => HasValue ? _value : throw new NaValueException();

    /// <summary>Gets <see langword="true"/> when this instance is NA.</summary>
    public bool IsNa => !_hasValue;

    private NaValue(T value)
    {
        _value = value;
        _hasValue = true;
    }

    /// <summary>Implicitly converts from <typeparamref name="T"/>: <see langword="null"/> becomes <see cref="Na"/>.</summary>
    public static implicit operator NaValue<T>(T? value)
    {
        if (value is null)
        {
            return default;
        }

        return new(value);
    }

    /// <summary>Explicitly extracts the underlying value.</summary>
    /// <exception cref="NaValueException">The value is NA.</exception>
    public static explicit operator T(NaValue<T> value)
    {
        return value.Value;
    }

    /// <summary>Returns the underlying value's string representation, or <see cref="NaValue.NaValueLabel"/> when NA.</summary>
    public override string ToString()
    {
        return _hasValue ? _value.ToString() ?? string.Empty : NaValue.NaValueLabel;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _hasValue ? _value.GetHashCode() : 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> only when both values are non-NA and equal.
    /// (NA values are <i>not</i> equal to each other under this comparison; see
    /// <see cref="EqualOrBothNa(NaValue{T})"/> for NA-equates-NA semantics.)
    /// </summary>
    public bool Equals(NaValue<T> other)
    {
        return NaValue.EqualAndNotNa(this, other);
    }

    /// <summary>Returns <see langword="true"/> when this instance has a value equal to <paramref name="value"/>.</summary>
    public bool Equals(T value)
    {
        return NaValue.Equals(this, value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return (obj is NaValue<T> other && Equals(other))
            || (obj is T n && Equals(n));
    }

    /// <summary>
    /// Three-valued equality: returns <see cref="Trilean.Na"/> when either side is
    /// NA, <see cref="Trilean.True"/> when both sides have equal underlying values,
    /// and <see cref="Trilean.False"/> otherwise.
    /// </summary>
    public Trilean TernaryEquals(NaValue<T> other)
    {
        if (IsNa || other.IsNa)
        {
            return Trilean.Na;
        }

        return _value.Equals(other._value) ? Trilean.True : Trilean.False;
    }

    /// <summary>Returns <see langword="true"/> only when both values are non-NA and equal.</summary>
    public bool EqualAndNotNa(NaValue<T> other)
    {
        return NaValue.EqualAndNotNa(this, other);
    }

    /// <summary>Returns <see langword="true"/> when both values are NA, or both are non-NA and equal.</summary>
    public bool EqualOrBothNa(NaValue<T> other)
    {
        return (IsNa && other.IsNa)
            || (!IsNa && !other.IsNa && _value.Equals(other._value));
    }

    /// <summary>Returns <see langword="true"/> when either value is NA, or both are equal.</summary>
    public bool EqualOrEitherNa(NaValue<T> other)
    {
        return IsNa || other.IsNa || _value.Equals(other._value);
    }

    /// <summary>Compares this value to <paramref name="other"/>; NA sorts before all values and equals only itself.</summary>
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

    /// <summary>
    /// Equality operator. Mirrors <see cref="float.NaN"/>: <c>Na == Na</c> is
    /// <see langword="false"/>. Use <see cref="Equals(NaValue{T})"/> for the
    /// <see cref="IEquatable{T}"/>-style comparison that treats NA as self-equal.
    /// </summary>
    public static bool operator ==(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.EqualAndNotNa(left, right);
    }

    /// <summary>Inequality operator. See <see cref="op_Equality(NaValue{T}, NaValue{T})"/>.</summary>
    public static bool operator !=(NaValue<T> left, NaValue<T> right)
    {
        return !(left == right);
    }

    /// <summary>Greater-than. NA sorts before all other values.</summary>
    public static bool operator >(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) > 0;
    }

    /// <summary>Greater-than-or-equal. NA sorts before all other values.</summary>
    public static bool operator >=(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) >= 0;
    }

    /// <summary>Less-than. NA sorts before all other values.</summary>
    public static bool operator <(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) < 0;
    }

    /// <summary>Less-than-or-equal. NA sorts before all other values.</summary>
    public static bool operator <=(NaValue<T> left, NaValue<T> right)
    {
        return NaValue.Compare(left, right) <= 0;
    }

    /// <summary>
    /// Parses a value from <paramref name="s"/>. The literal
    /// <see cref="NaValue.NaValueLabel"/> (<c>"NA"</c>) parses as <see cref="Na"/>.
    /// </summary>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid representation.</exception>
    public static NaValue<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(NaValue)}: {s}");
        return default; // unreachable
    }

    /// <summary>
    /// Attempts to parse a value from <paramref name="s"/>. The literal
    /// <see cref="NaValue.NaValueLabel"/> (<c>"NA"</c>) parses as <see cref="Na"/>.
    /// </summary>
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

    /// <summary>Parses a value from <paramref name="s"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid representation.</exception>
    public static NaValue<T> Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>Attempts to parse a value from <paramref name="s"/>.</summary>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NaValue<T> result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }
}
