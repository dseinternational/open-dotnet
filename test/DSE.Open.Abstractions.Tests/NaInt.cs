// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Abstractions.Tests;

internal readonly struct NaInt : INaValue<NaInt, int>, IEquatable<NaInt>
{
    private readonly int _value;

    public static NaInt Na { get; }

    public NaInt(int value)
    {
        _value = value;
        HasValue = true;
    }

    public bool HasValue { get; }

    public bool IsNa => !HasValue;

    public int Value => HasValue ? _value : throw new InvalidOperationException();

    public bool Equals(NaInt other)
    {
        return HasValue && other.HasValue && Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is NaInt && Equals((NaInt)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, HasValue);
    }

    public override string ToString()
    {
        return HasValue ? Value.ToString(CultureInfo.InvariantCulture) : "Na";
    }

    public Trilean TernaryEquals(NaInt other)
    {
        return Ternary.Equals(this, other);
    }

    public bool EqualOrBothNa(NaInt other)
    {
        return Ternary.EqualOrBothNa(this, other);
    }

    public bool EqualOrEitherNa(NaInt other)
    {
        return Ternary.EqualOrEitherNa(this, other);
    }

    public static bool operator ==(NaInt left, NaInt right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NaInt left, NaInt right)
    {
        return !left.Equals(right);
    }

    public static implicit operator NaInt(int value)
    {
        return new NaInt(value);
    }

    public static explicit operator int(NaInt naValue)
    {
        if (naValue.IsNa)
        {
            throw new NaValueException("Value is Na");
        }

        return naValue.Value;
    }
}
