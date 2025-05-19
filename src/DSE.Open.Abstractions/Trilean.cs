// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open;

/// <summary>
/// A value that can be known to be true, known to be false, or unknown (maybe true or false).
/// </summary>
public readonly struct Trilean
    : IEquatable<Trilean>,
      ITriEquatable<Trilean>,
      INaValue,
      ISpanFormattable,
      ISpanParsable<Trilean>
{
    private const byte UnknownValue = 0;
    private const byte TrueValue = 1;
    private const byte FalseValue = 2;

    private const string UnknownStringValue = "Unknown";
    private const string TrueStringValue = "True";
    private const string FalseStringValue = "False";

    private const string NullStringValue = "Null";

    /// <summary>
    /// A value that is known to be true.
    /// </summary>
    public static readonly Trilean True = new(TrueValue);

    /// <summary>
    /// A value that is known to be false.
    /// </summary>
    public static readonly Trilean False = new(FalseValue);

    /// <summary>
    /// A value that is unknown.
    /// </summary>
    public static readonly Trilean Na = new(UnknownValue);

    private readonly byte _value;

    private Trilean(byte value)
    {
        _value = value;
    }

    private Trilean(bool value) : this(value ? TrueValue : FalseValue) { }

    /// <summary>
    /// Indicates whether the value is <see cref="Na"/>.
    /// </summary>
    public bool IsNa => _value == UnknownValue;

    /// <summary>
    /// Indicates whether the value is <see cref="True"/>.
    /// </summary>
    public bool IsTrue => _value == TrueValue;

    /// <summary>
    /// Indicates whether the value is <see cref="False"/>.
    /// </summary>
    public bool IsFalse => _value == FalseValue;

    bool INaValue.HasValue => !IsNa;

    object INaValue.Value => _value switch
    {
        TrueValue => True,
        FalseValue => False,
        _ => Na,
    };

    public void Deconstruct(out bool? value)
    {
        value = ToNullableBoolean();
    }

    public Trilean Equals(Trilean other)
    {
        return Equals(this, other);
    }

    bool IEquatable<Trilean>.Equals(Trilean other)
    {
        // do not call (Tri.)EqualOrBothUnknown as it calls back via IEquatable<T>

        // if both values are unknown, return true.

        if (IsNa)
        {
            return other.IsNa;
        }

        // if both values are known, return true if equal or false if not equal.
        return Equals(other).IsTrue;
    }

    public bool EqualOrBothUnknown(Trilean other)
    {
        return Tri.EqualOrBothNa(this, other);
    }

    public bool EqualOrEitherUnknown(Trilean other)
    {
        return Tri.EqualOrEitherNa(this, other);
    }

    public bool EqualAndNeitherUnknown(Trilean other)
    {
        return Tri.EqualAndNeitherNa(this, other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Trilean t && EqualOrBothUnknown(t);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    /// <summary>
    /// Converts the current value to a signed integer of the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// A signed number that is equal to <see cref="INumberBase{TSelf}.One"/> if <see cref="IsTrue"/>,
    /// equal to <see cref="INumberBase{TSelf}.Zero"/> if <see cref="IsFalse"/>, or equal to
    /// <see cref="ISignedNumber{TSelf}.NegativeOne"/> if <see cref="IsNa"/>.
    /// </returns>
    public T ToSignedNumber<T>()
        where T : struct, ISignedNumber<T>
    {
        return _value switch
        {
            TrueValue => T.One,
            FalseValue => T.Zero,
            _ => T.NegativeOne,
        };
    }

    /// <summary>
    /// Converts the current value to an usigned integer of the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    /// An unsigned number that is equal to <see cref="INumberBase{T}.One"/> if <see cref="IsTrue"/>,
    /// equal to <see cref="INumberBase{T}.Zero"/> if <see cref="IsFalse"/>, or equal to
    /// <see cref="INumberBase{T}.One"/> + <see cref="INumberBase{T}.One"/> if <see cref="IsNa"/>.
    /// </returns>
    public T ToUnsignedNumber<T>()
        where T : struct, IUnsignedNumber<T>
    {
        return _value switch
        {
            TrueValue => T.One,
            FalseValue => T.One + T.One,
            _ => T.Zero,
        };
    }

    public byte ToSignedInteger()
    {
        return _value;
    }

    public bool ToBoolean()
    {
        return _value switch
        {
            TrueValue => true,
            FalseValue => false,
            _ => throw new NaValueException(
                $"Cannot convert {nameof(Na)} {nameof(Trilean)} to boolean value.")
        };
    }

    /// <summary>
    /// Returns a nullable boolean value that is equal to <see langword="true"/> if <see cref="IsTrue"/>,
    /// equal to <see langword="false"/> if <see cref="IsFalse"/>, or else <see cref="Nullable{T}.HasValue"/>
    /// is equal to <see langword="false"/> (so <see langword="null"/>).
    /// </summary>
    public bool? ToNullableBoolean()
    {
        return _value switch
        {
            TrueValue => true,
            FalseValue => false,
            _ => null
        };
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, null);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var value = _value switch
        {
            TrueValue => TrueStringValue,
            FalseValue => FalseStringValue,
            _ => UnknownStringValue
        };

        if (value.AsSpan().TryCopyTo(destination))
        {
            charsWritten = value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value switch
        {
            TrueValue => TrueStringValue,
            FalseValue => FalseStringValue,
            _ => UnknownStringValue
        };
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public static Trilean FromBoolean(bool value)
    {
        return new(value);
    }

    public static Trilean FromSignedNumber<T>(T value)
        where T : struct, ISignedNumber<T>
    {
        return value switch
        {
            { } v when v == T.One => True,
            { } v when v == T.Zero => False,
            { } v when v == T.NegativeOne => Na,
            _ => throw new InvalidOperationException("Cannot convert value to Trilean.")
        };
    }

    public static Trilean FromUnsignedNumber<T>(T value)
        where T : struct, IUnsignedNumber<T>
    {
        return value switch
        {
            { } v when v == T.Zero => Na,
            { } v when v == T.One => True,
            { } v when v == T.One + T.One => false,
            _ => throw new InvalidOperationException("Cannot convert value to Trilean.")
        };
    }

    public static Trilean FromInt16(short value)
    {
        return FromSignedNumber(value);
    }

    public static Trilean FromInt32(int value)
    {
        return FromSignedNumber(value);
    }

    public static Trilean FromInt64(long value)
    {
        return FromSignedNumber(value);
    }

    public static Trilean FromInteger(sbyte value)
    {
        return FromSignedNumber(value);
    }

    public static Trilean FromBoolean(bool? value)
    {
        return value.HasValue ? FromBoolean(value.Value) : Na;
    }

    public static Trilean FromSignedNumber<T>(T? value)
        where T : struct, ISignedNumber<T>
    {
        return value.HasValue ? FromSignedNumber(value.Value) : Na;
    }

    public static Trilean FromUnsignedNumber<T>(T? value)
        where T : struct, IUnsignedNumber<T>
    {
        return value.HasValue ? FromUnsignedNumber(value.Value) : Na;
    }

    public static explicit operator bool(Trilean t)
    {
        return t.ToBoolean();
    }

    public static implicit operator Trilean(bool b)
    {
        return FromBoolean(b);
    }

    /// <summary>
    /// Implicit conversion from nullable bool to Trilean.
    /// Null maps to Unknown.
    /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates -- FromBoolean
    public static implicit operator Trilean(bool? b)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return FromBoolean(b);
    }

    public static Trilean Equals(Trilean left, Trilean right)
    {
        if (left.IsNa || right.IsNa)
        {
            return null;
        }

        if (left._value.Equals(right._value))
        {
            return True;
        }

        return False;
    }

    public static Trilean operator ==(Trilean left, Trilean right)
    {
        return Equals(left, right);
    }

    public static Trilean operator !=(Trilean left, Trilean right)
    {
        return !Equals(left, right);
    }

    public static bool Equals(Trilean left, bool right)
    {
        return left._value == (right ? TrueValue : FalseValue);
    }

    public static bool operator ==(Trilean left, bool right)
    {
        return Equals(left, right);
    }

    public static bool operator ==(bool left, Trilean right)
    {
        return right == left;
    }

    public static bool operator !=(Trilean left, bool right)
    {
        return !Equals(left, right);
    }

    public static bool operator !=(bool left, Trilean right)
    {
        return !Equals(right, left);
    }

    public static Trilean LogicalNot(Trilean value)
    {
        return value._value switch
        {
            TrueValue => False,
            FalseValue => True,
            _ => Na
        };
    }

    public static Trilean operator !(Trilean value)
    {
        return LogicalNot(value);
    }

    public static Trilean LogicalAnd(Trilean left, Trilean right)
    {
        if (left._value == FalseValue || right._value == FalseValue)
        {
            return False;
        }

        if (left._value == TrueValue && right._value == TrueValue)
        {
            return true;
        }

        return Na;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalAnd
    public static Trilean operator &(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalAnd(left, right);
    }

    public static Trilean LogicalOr(Trilean left, Trilean right)
    {
        if (left._value == TrueValue || right._value == TrueValue)
        {
            return True;
        }

        if (left._value == FalseValue && right._value == FalseValue)
        {
            return False;
        }

        return Na;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalOr
    public static Trilean operator |(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalOr(left, right);
    }

    public static Trilean LogicalXor(Trilean left, Trilean right)
    {
        if (left._value == UnknownValue || right._value == UnknownValue)
        {
            return Na;
        }

        return (left._value == TrueValue ^ right._value == TrueValue) ? True : False;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalXor
    public static Trilean operator ^(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalXor(left, right);
    }

    public static Trilean Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(Trilean)}: {s}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Trilean result)
    {
        if (s.Equals(TrueStringValue, StringComparison.OrdinalIgnoreCase))
        {
            result = True;
            return true;
        }

        if (s.Equals(FalseStringValue, StringComparison.OrdinalIgnoreCase))
        {
            result = False;
            return true;
        }

        if (s.Equals(UnknownStringValue, StringComparison.OrdinalIgnoreCase)
            || s.Equals(NullStringValue, StringComparison.OrdinalIgnoreCase)
            || s.IsEmpty)
        {
            result = Na;
            return true;
        }

        if (s.Length == 1)
        {
            if (s[0] == '1')
            {
                result = True;
                return true;
            }

            if (s[0] == '2')
            {
                result = False;
                return true;
            }

            if (s[0] == '0')
            {
                result = Na;
                return true;
            }
        }

        result = default;
        return false;
    }

    public static Trilean Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s.AsSpan(), provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(Trilean)}: {s}");
        return default; // unreachable
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Trilean result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
