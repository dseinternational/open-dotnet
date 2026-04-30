// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open;

/// <summary>
/// A value that can be known to be true, known to be false, or unknown (maybe true or false).
/// </summary>
public readonly struct Trilean
    : IEquatable<Trilean>,
      ITernaryEquatable<Trilean>,
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
        _ => throw new NaValueException(
            $"Cannot access value as the {nameof(Trilean)} value is Na."),
    };

    /// <summary>
    /// Deconstructs the value into a nullable <see cref="bool"/>, where <see cref="Na"/> maps to
    /// <see langword="null"/>.
    /// </summary>
    public void Deconstruct(out bool? value)
    {
        value = ToNullableBoolean();
    }

    /// <summary>
    /// Returns the three-valued (ternary) equality result for this value compared to <paramref name="other"/>.
    /// </summary>
    public Trilean TernaryEquals(Trilean other)
    {
        return TernaryEquals(this, other);
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
        return TernaryEquals(other).IsTrue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if this value equals <paramref name="other"/>, including when
    /// both values are <see cref="Na"/>.
    /// </summary>
    public bool EqualOrBothNa(Trilean other)
    {
        return Ternary.EqualOrBothNa(this, other);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this value equals <paramref name="other"/> or either value
    /// is <see cref="Na"/>.
    /// </summary>
    public bool EqualOrEitherNa(Trilean other)
    {
        return Ternary.EqualOrEitherNa(this, other);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this value equals <paramref name="other"/> and neither
    /// value is <see cref="Na"/>.
    /// </summary>
    public bool EqualAndNotNa(Trilean other)
    {
        return Ternary.EqualAndNeitherNa(this, other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Trilean t && EqualOrBothNa(t);
    }

    /// <inheritdoc/>
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
    /// Converts the current value to an unsigned integer of the specified type.
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
            FalseValue => T.Zero,
            _ => T.One + T.One,
        };
    }

    /// <summary>
    /// Converts the current value to a signed 8-bit integer.
    /// </summary>
    /// <returns>
    /// <c>1</c> if <see cref="IsTrue"/>, <c>0</c> if <see cref="IsFalse"/>, or <c>-1</c> if <see cref="IsNa"/>.
    /// </returns>
    public sbyte ToSignedInteger()
    {
        return ToSignedNumber<sbyte>();
    }

    /// <summary>
    /// Converts the current value to a <see cref="bool"/>.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="IsTrue"/>, <see langword="false"/> if <see cref="IsFalse"/>.</returns>
    /// <exception cref="NaValueException">The value is <see cref="Na"/>.</exception>
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
    /// Converts the current value to a <see cref="SqlBoolean"/>, mapping <see cref="Na"/> to
    /// <see cref="SqlBoolean.Null"/>.
    /// </summary>
    public SqlBoolean ToSqlBoolean()
    {
        return _value switch
        {
            TrueValue => SqlBoolean.True,
            FalseValue => SqlBoolean.False,
            _ => SqlBoolean.Null
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

    /// <summary>
    /// Attempts to write the textual representation of the current value to <paramref name="destination"/>.
    /// </summary>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, null);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value switch
        {
            TrueValue => TrueStringValue,
            FalseValue => FalseStringValue,
            _ => UnknownStringValue
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// Returns <see cref="True"/> when <paramref name="value"/> is <see langword="true"/> and
    /// <see cref="False"/> when <paramref name="value"/> is <see langword="false"/>.
    /// </summary>
    public static Trilean FromBoolean(bool value)
    {
        return new(value);
    }

    /// <summary>
    /// Maps a signed number to a <see cref="Trilean"/>: <c>1</c> to <see cref="True"/>,
    /// <c>0</c> to <see cref="False"/>, and <c>-1</c> to <see cref="Na"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">The value is not <c>1</c>, <c>0</c>, or <c>-1</c>.</exception>
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

    /// <summary>
    /// Maps an unsigned number to a <see cref="Trilean"/>: <c>1</c> to <see cref="True"/>,
    /// <c>0</c> to <see cref="False"/>, and <c>2</c> to <see cref="Na"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">The value is not <c>0</c>, <c>1</c>, or <c>2</c>.</exception>
    public static Trilean FromUnsignedNumber<T>(T value)
        where T : struct, IUnsignedNumber<T>
    {
        return value switch
        {
            { } v when v == T.Zero => False,
            { } v when v == T.One => True,
            { } v when v == T.One + T.One => Na,
            _ => throw new InvalidOperationException("Cannot convert value to Trilean.")
        };
    }

    /// <summary>
    /// Maps a <see cref="short"/> to a <see cref="Trilean"/> using <see cref="FromSignedNumber{T}(T)"/>.
    /// </summary>
    public static Trilean FromInt16(short value)
    {
        return FromSignedNumber(value);
    }

    /// <summary>
    /// Maps an <see cref="int"/> to a <see cref="Trilean"/> using <see cref="FromSignedNumber{T}(T)"/>.
    /// </summary>
    public static Trilean FromInt32(int value)
    {
        return FromSignedNumber(value);
    }

    /// <summary>
    /// Maps a <see cref="long"/> to a <see cref="Trilean"/> using <see cref="FromSignedNumber{T}(T)"/>.
    /// </summary>
    public static Trilean FromInt64(long value)
    {
        return FromSignedNumber(value);
    }

    /// <summary>
    /// Maps an <see cref="sbyte"/> to a <see cref="Trilean"/> using <see cref="FromSignedNumber{T}(T)"/>.
    /// </summary>
    public static Trilean FromInteger(sbyte value)
    {
        return FromSignedNumber(value);
    }

    /// <summary>
    /// Maps a nullable <see cref="bool"/> to a <see cref="Trilean"/>, where <see langword="null"/>
    /// becomes <see cref="Na"/>.
    /// </summary>
    public static Trilean FromBoolean(bool? value)
    {
        return value.HasValue ? FromBoolean(value.Value) : Na;
    }

    /// <summary>
    /// Maps a nullable signed number to a <see cref="Trilean"/>, where <see langword="null"/> becomes
    /// <see cref="Na"/>.
    /// </summary>
    public static Trilean FromSignedNumber<T>(T? value)
        where T : struct, ISignedNumber<T>
    {
        return value.HasValue ? FromSignedNumber(value.Value) : Na;
    }

    /// <summary>
    /// Maps a nullable unsigned number to a <see cref="Trilean"/>, where <see langword="null"/>
    /// becomes <see cref="Na"/>.
    /// </summary>
    public static Trilean FromUnsignedNumber<T>(T? value)
        where T : struct, IUnsignedNumber<T>
    {
        return value.HasValue ? FromUnsignedNumber(value.Value) : Na;
    }

    /// <summary>
    /// Explicitly converts a <see cref="Trilean"/> to a <see cref="bool"/>; throws when the value is <see cref="Na"/>.
    /// </summary>
    public static explicit operator bool(Trilean t)
    {
        return t.ToBoolean();
    }

    /// <summary>
    /// Implicitly converts a <see cref="Trilean"/> to a nullable <see cref="bool"/>, mapping
    /// <see cref="Na"/> to <see langword="null"/>.
    /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator bool?(Trilean t)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return t.ToNullableBoolean();
    }

    /// <summary>
    /// Implicitly converts a <see cref="Trilean"/> to a <see cref="SqlBoolean"/>.
    /// </summary>
    public static implicit operator SqlBoolean(Trilean t)
    {
        return t.ToSqlBoolean();
    }

    /// <summary>
    /// Implicitly converts a <see cref="bool"/> to a <see cref="Trilean"/>.
    /// </summary>
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

    /// <summary>
    /// Returns a three-valued (ternary) equality comparison of <paramref name="left"/> and
    /// <paramref name="right"/>: <see cref="Na"/> if either operand is <see cref="Na"/>,
    /// otherwise <see cref="True"/> or <see cref="False"/>.
    /// </summary>
    /// <remarks>
    /// The <c>==</c> and <c>!=</c> operators on two <see cref="Trilean"/> values return
    /// <see langword="bool"/> with standard <see cref="IEquatable{T}"/> semantics
    /// (<c>Na == Na</c> is <see langword="true"/>). Use this method when you need
    /// SQL-style three-valued equality.
    /// </remarks>
    public static Trilean TernaryEquals(Trilean left, Trilean right)
    {
        if (left.IsNa || right.IsNa)
        {
            return Na;
        }

        return left._value == right._value ? True : False;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/>
    /// are the same value, including when both are <see cref="Na"/>.
    /// </summary>
    /// <remarks>
    /// This matches <see cref="IEquatable{T}"/> semantics. For SQL-style three-valued
    /// equality (where <c>Na == Na</c> is <see cref="Na"/>), use <see cref="TernaryEquals(Trilean, Trilean)"/>.
    /// </remarks>
    public static bool operator ==(Trilean left, Trilean right)
    {
        return left._value == right._value;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not the
    /// same value. Two <see cref="Na"/> values are considered equal under this operator.
    /// </summary>
    public static bool operator !=(Trilean left, Trilean right)
    {
        return left._value != right._value;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents the same value as the
    /// <see cref="bool"/> <paramref name="right"/>. <see cref="Na"/> never equals a <see cref="bool"/>.
    /// </summary>
    public static bool Equals(Trilean left, bool right)
    {
        return left._value == (right ? TrueValue : FalseValue);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents the same value as
    /// <paramref name="right"/>.
    /// </summary>
    public static bool operator ==(Trilean left, bool right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="right"/> represents the same value as
    /// <paramref name="left"/>.
    /// </summary>
    public static bool operator ==(bool left, Trilean right)
    {
        return right == left;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> does not represent the same value as
    /// <paramref name="right"/>.
    /// </summary>
    public static bool operator !=(Trilean left, bool right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="right"/> does not represent the same value as
    /// <paramref name="left"/>.
    /// </summary>
    public static bool operator !=(bool left, Trilean right)
    {
        return !Equals(right, left);
    }

    /// <summary>
    /// Returns the three-valued logical negation of <paramref name="value"/>: <see cref="True"/> becomes
    /// <see cref="False"/>, <see cref="False"/> becomes <see cref="True"/>, and <see cref="Na"/> remains
    /// <see cref="Na"/>.
    /// </summary>
    public static Trilean LogicalNot(Trilean value)
    {
        return value._value switch
        {
            TrueValue => False,
            FalseValue => True,
            _ => Na
        };
    }

    /// <summary>
    /// Returns the three-valued logical negation of <paramref name="value"/> via <see cref="LogicalNot(Trilean)"/>.
    /// </summary>
    public static Trilean operator !(Trilean value)
    {
        return LogicalNot(value);
    }

    /// <summary>
    /// Returns the three-valued logical AND of <paramref name="left"/> and <paramref name="right"/>:
    /// <see cref="False"/> if either operand is <see cref="False"/>, <see cref="True"/> if both are
    /// <see cref="True"/>, otherwise <see cref="Na"/>.
    /// </summary>
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

    /// <summary>
    /// Returns the three-valued logical AND of <paramref name="left"/> and <paramref name="right"/>
    /// via <see cref="LogicalAnd(Trilean, Trilean)"/>.
    /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalAnd
    public static Trilean operator &(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalAnd(left, right);
    }

    /// <summary>
    /// Returns the three-valued logical OR of <paramref name="left"/> and <paramref name="right"/>:
    /// <see cref="True"/> if either operand is <see cref="True"/>, <see cref="False"/> if both are
    /// <see cref="False"/>, otherwise <see cref="Na"/>.
    /// </summary>
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

    /// <summary>
    /// Returns the three-valued logical OR of <paramref name="left"/> and <paramref name="right"/>
    /// via <see cref="LogicalOr(Trilean, Trilean)"/>.
    /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalOr
    public static Trilean operator |(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalOr(left, right);
    }

    /// <summary>
    /// Returns the three-valued logical XOR of <paramref name="left"/> and <paramref name="right"/>:
    /// <see cref="Na"/> if either operand is <see cref="Na"/>, otherwise <see cref="True"/> when
    /// exactly one operand is <see cref="True"/> and <see cref="False"/> otherwise.
    /// </summary>
    public static Trilean LogicalXor(Trilean left, Trilean right)
    {
        if (left._value == UnknownValue || right._value == UnknownValue)
        {
            return Na;
        }

        return (left._value == TrueValue ^ right._value == TrueValue) ? True : False;
    }

    /// <summary>
    /// Returns the three-valued logical XOR of <paramref name="left"/> and <paramref name="right"/>
    /// via <see cref="LogicalXor(Trilean, Trilean)"/>.
    /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates - LogicalXor
    public static Trilean operator ^(Trilean left, Trilean right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return LogicalXor(left, right);
    }

    /// <summary>
    /// Parses a <see cref="Trilean"/> from the given span, throwing if the input is not recognised.
    /// </summary>
    /// <remarks>
    /// Accepted inputs (case-insensitive):
    /// <list type="bullet">
    ///   <item><description><c>"True"</c> or <c>"1"</c> — <see cref="True"/>.</description></item>
    ///   <item><description><c>"False"</c> or <c>"2"</c> — <see cref="False"/>.</description></item>
    ///   <item><description><c>"Unknown"</c>, <c>"Null"</c>, <c>"0"</c>, or the empty span — <see cref="Na"/>.</description></item>
    /// </list>
    /// Note that an empty span parses successfully as <see cref="Na"/>; it does not fail.
    /// </remarks>
    public static Trilean Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(Trilean)}: {s}");
        return default; // unreachable
    }

    /// <summary>
    /// Attempts to parse a <see cref="Trilean"/> from the given span.
    /// </summary>
    /// <remarks>
    /// Accepted inputs (case-insensitive):
    /// <list type="bullet">
    ///   <item><description><c>"True"</c> or <c>"1"</c> — <see cref="True"/>.</description></item>
    ///   <item><description><c>"False"</c> or <c>"2"</c> — <see cref="False"/>.</description></item>
    ///   <item><description><c>"Unknown"</c>, <c>"Null"</c>, <c>"0"</c>, or the empty span — <see cref="Na"/>.</description></item>
    /// </list>
    /// Note that an empty span parses successfully as <see cref="Na"/>; the method returns
    /// <see langword="true"/> and sets <paramref name="result"/> to <see cref="Na"/>.
    /// </remarks>
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

    /// <summary>
    /// Parses a <see cref="Trilean"/> from the given string, using the same accepted inputs as
    /// <see cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a recognised value.</exception>
    public static Trilean Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s.AsSpan(), provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Invalid {nameof(Trilean)}: {s}");
        return default; // unreachable
    }

    /// <summary>
    /// Attempts to parse a <see cref="Trilean"/> from the given string, using the same accepted
    /// inputs as <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out Trilean)"/>. Returns
    /// <see langword="false"/> when <paramref name="s"/> is <see langword="null"/>.
    /// </summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Trilean result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }
}
