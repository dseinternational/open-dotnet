// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A weakly-typed value that can be used to represent any of the supported <see cref="Vector{T}"/> types
/// (see <see cref="VectorDataType"/>).
/// </summary>
/// <remarks>
/// <see cref="VectorValue"/> is a small (≈24-byte) tagged union of a <see cref="ulong"/>
/// bit pattern, an optional reference (used for strings and as the NA sentinel), and a
/// <see cref="VectorDataType"/> tag. It is the type used by
/// <see cref="IReadOnlyVector.GetVectorValue(int)"/> and the row collection types
/// (<see cref="DataFrameRow"/>, <see cref="ReadOnlyDataFrameRow"/>) so that callers
/// can read individual cells without knowing the column's element type at compile time
/// and without boxing.
/// <para>
/// Use the <see cref="FromValue{T}(T)"/> generic factory or one of the typed
/// <c>From*</c> factories to construct a value, and the corresponding <c>To*</c>
/// accessor (or one of the explicit conversion operators) to extract the typed value.
/// </para>
/// </remarks>
public readonly struct VectorValue
    : IEquatable<VectorValue>,
      IEquatable<double>
{
    /// <summary>
    /// Singleton reference used to mark an Na <see cref="VectorValue"/> independently of
    /// the value bits. Required because the natural sentinels of the typed Na wrappers
    /// (e.g. <see cref="NaInt{T}"/>'s <c>T.MaxValue</c>, <see cref="NaFloat{T}"/>'s
    /// <c>NaN</c>) cannot all be represented and round-tripped through the public typed
    /// constructors of those wrappers — and because <c>NaBool</c>/<c>NaChar</c>/
    /// <c>NaDateTime</c>/<c>NaDateTimeOffset</c> have no in-band sentinel at all.
    /// </summary>
    private static readonly object s_naSentinel = new();

    private readonly ulong _bits;
    private readonly object? _ref;
    private readonly VectorDataType _kind;

    private VectorValue(ulong bits, object? @ref, VectorDataType kind)
    {
        _bits = bits;
        _ref = @ref;
        _kind = kind;
    }

    private bool IsNaSentinel => ReferenceEquals(_ref, s_naSentinel);

    /// <summary>
    /// Gets the <see cref="VectorDataType"/> tag identifying the runtime element type.
    /// </summary>
    public VectorDataType DataType => _kind;

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="sbyte"/>.</summary>
    public static VectorValue FromInt8(sbyte value)
    {
        return new((ulong)value, null, VectorDataType.Int8);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="short"/>.</summary>
    public static VectorValue FromInt16(short value)
    {
        return new((ulong)value, null, VectorDataType.Int16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="int"/>.</summary>
    public static VectorValue FromInt32(int value)
    {
        return new((ulong)value, null, VectorDataType.Int32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="long"/>.</summary>
    public static VectorValue FromInt64(long value)
    {
        return new((ulong)value, null, VectorDataType.Int64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="byte"/>.</summary>
    public static VectorValue FromUInt8(byte value)
    {
        return new(value, null, VectorDataType.UInt8);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="ushort"/>.</summary>
    public static VectorValue FromUInt16(ushort value)
    {
        return new(value, null, VectorDataType.UInt16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="uint"/>.</summary>
    public static VectorValue FromUInt32(uint value)
    {
        return new(value, null, VectorDataType.UInt32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="ulong"/>.</summary>
    public static VectorValue FromUInt64(ulong value)
    {
        return new(value, null, VectorDataType.UInt64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="Half"/>.</summary>
    public static VectorValue FromFloat16(Half value)
    {
        return new(BitConverter.HalfToUInt16Bits(value), null, VectorDataType.Float16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="float"/>.</summary>
    public static VectorValue FromFloat32(float value)
    {
        return new(BitConverter.SingleToUInt32Bits(value), null, VectorDataType.Float32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="double"/>.</summary>
    public static VectorValue FromFloat64(double value)
    {
        return new(BitConverter.DoubleToUInt64Bits(value), null, VectorDataType.Float64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="string"/>.</summary>
    public static VectorValue FromString(string value)
    {
        return new(0, value, VectorDataType.String);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="char"/>.</summary>
    public static VectorValue FromChar(char value)
    {
        return new(value, null, VectorDataType.Char);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="bool"/>.</summary>
    public static VectorValue FromBoolean(bool value)
    {
        return new(value ? (ulong)1 : 0, null, VectorDataType.Bool);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="DateTime64"/>.</summary>
    public static VectorValue FromDateTime64(DateTime64 value)
    {
        return new((ulong)value.TotalMilliseconds, null, VectorDataType.DateTime64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="DateTime"/> (ticks-encoded).</summary>
    public static VectorValue FromDateTime(DateTime value)
    {
        return new((ulong)value.Ticks, null, VectorDataType.DateTime);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping a <see cref="DateTimeOffset"/> (ticks-encoded).</summary>
    public static VectorValue FromDateTimeOffset(DateTimeOffset value)
    {
        return new((ulong)value.Ticks, null, VectorDataType.DateTimeOffset);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="sbyte"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaInt8(NaInt<sbyte> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaInt8)
            : new((ulong)(sbyte)value, null, VectorDataType.NaInt8);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="short"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaInt16(NaInt<short> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaInt16)
            : new((ulong)(short)value, null, VectorDataType.NaInt16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="int"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaInt32(NaInt<int> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaInt32)
            : new((ulong)(int)value, null, VectorDataType.NaInt32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="long"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaInt64(NaInt<long> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaInt64)
            : new((ulong)(long)value, null, VectorDataType.NaInt64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="byte"/>.</summary>
    public static VectorValue FromNaUInt8(NaInt<byte> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaUInt8)
            : new((byte)value, null, VectorDataType.NaUInt8);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="ushort"/>.</summary>
    public static VectorValue FromNaUInt16(NaInt<ushort> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaUInt16)
            : new((ushort)value, null, VectorDataType.NaUInt16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="uint"/>.</summary>
    public static VectorValue FromNaUInt32(NaInt<uint> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaUInt32)
            : new((uint)value, null, VectorDataType.NaUInt32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="ulong"/>.</summary>
    public static VectorValue FromNaUInt64(NaInt<ulong> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaUInt64)
            : new((ulong)value, null, VectorDataType.NaUInt64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaFloat{T}"/> over <see cref="float"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaFloat32(NaFloat<float> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaFloat32)
            : new(BitConverter.SingleToUInt32Bits((float)value), null, VectorDataType.NaFloat32);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaFloat{T}"/> over <see cref="Half"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaFloat16(NaFloat<Half> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaFloat16)
            : new(BitConverter.HalfToUInt16Bits((Half)value), null, VectorDataType.NaFloat16);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaFloat{T}"/> over <see cref="double"/>. NA-ness is preserved across <c>From</c>/<c>To</c> round-trips.</summary>
    public static VectorValue FromNaFloat64(NaFloat<double> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaFloat64)
            : new(BitConverter.DoubleToUInt64Bits((double)value), null, VectorDataType.NaFloat64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaValue{T}"/> over <see cref="string"/>. Na is encoded as a null reference and is recovered by <see cref="ToNaString"/>.</summary>
    public static VectorValue FromNaString(NaValue<string> value)
    {
        // For NaString the existing convention is to encode Na as _ref==null and a
        // present value as _ref==<the string>; preserved here.
        return value.IsNa
            ? new(0, null, VectorDataType.NaString)
            : new(0, (string)value, VectorDataType.NaString);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaValue{T}"/> over <see cref="char"/>.</summary>
    public static VectorValue FromNaChar(NaValue<char> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaChar)
            : new((char)value, null, VectorDataType.NaChar);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaValue{T}"/> over <see cref="bool"/>.</summary>
    public static VectorValue FromNaBoolean(NaValue<bool> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaBool)
            : new(((bool)value) ? (ulong)1 : 0, null, VectorDataType.NaBool);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaInt{T}"/> over <see cref="DateTime64"/>.</summary>
    public static VectorValue FromNaDateTime64(NaInt<DateTime64> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaDateTime64)
            : new((ulong)((DateTime64)value).TotalMilliseconds, null, VectorDataType.NaDateTime64);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaValue{T}"/> over <see cref="DateTime"/>.</summary>
    public static VectorValue FromNaDateTime(NaValue<DateTime> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaDateTime)
            : new((ulong)((DateTime)value).Ticks, null, VectorDataType.NaDateTime);
    }

    /// <summary>Creates a <see cref="VectorValue"/> wrapping an <see cref="NaValue{T}"/> over <see cref="DateTimeOffset"/>.</summary>
    public static VectorValue FromNaDateTimeOffset(NaValue<DateTimeOffset> value)
    {
        return value.IsNa
            ? new(0, s_naSentinel, VectorDataType.NaDateTimeOffset)
            : new((ulong)((DateTimeOffset)value).Ticks, null, VectorDataType.NaDateTimeOffset);
    }

    /// <summary>
    /// Creates a <see cref="VectorValue"/> from a generic <typeparamref name="T"/> by
    /// dispatching on the runtime type to the appropriate strongly-typed factory
    /// (<see cref="FromInt32(int)"/>, <see cref="FromString(string)"/>, etc.).
    /// </summary>
    /// <typeparam name="T">The element type. Must be one of the runtime types that
    /// has a corresponding <see cref="VectorDataType"/>.</typeparam>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not a
    /// supported element type.</exception>
    public static VectorValue FromValue<T>(T value)
        where T : IEquatable<T>
    {
        switch (value)
        {
            case sbyte int8Value:
                return FromInt8(int8Value);
            case short int16Value:
                return FromInt16(int16Value);
            case int int32alue:
                return FromInt32(int32alue);
            case long int64Value:
                return FromInt64(int64Value);
            case byte uint8Value:
                return FromUInt8(uint8Value);
            case ushort uint16Value:
                return FromUInt16(uint16Value);
            case uint uint32Value:
                return FromUInt32(uint32Value);
            case ulong uint64Value:
                return FromUInt64(uint64Value);
            case float float32Value:
                return FromFloat32(float32Value);
            case double float64Value:
                return FromFloat64(float64Value);
            case Half float16Value:
                return FromFloat16(float16Value);
            case string stringValue:
                return FromString(stringValue);
            case char charValue:
                return FromChar(charValue);
            case bool boolValue:
                return FromBoolean(boolValue);
            case DateTime dateTimeValue:
                return FromDateTime(dateTimeValue);
            case DateTimeOffset dateTimeOffsetValue:
                return FromDateTimeOffset(dateTimeOffsetValue);
            case DateTime64 dateTime64Value:
                return FromDateTime64(dateTime64Value);
            case NaInt<sbyte> int8Value:
                return FromNaInt8(int8Value);
            case NaInt<short> int16Value:
                return FromNaInt16(int16Value);
            case NaInt<int> int32alue:
                return FromNaInt32(int32alue);
            case NaInt<long> int64Value:
                return FromNaInt64(int64Value);
            case NaInt<byte> uint8Value:
                return FromNaUInt8(uint8Value);
            case NaInt<ushort> uint16Value:
                return FromNaUInt16(uint16Value);
            case NaInt<uint> uint32Value:
                return FromNaUInt32(uint32Value);
            case NaInt<ulong> uint64Value:
                return FromNaUInt64(uint64Value);
            case NaFloat<float> float32Value:
                return FromNaFloat32(float32Value);
            case NaFloat<double> float64Value:
                return FromNaFloat64(float64Value);
            case NaFloat<Half> float16Value:
                return FromNaFloat16(float16Value);
            case NaValue<string> stringValue:
                return FromNaString(stringValue);
            case NaValue<char> charValue:
                return FromNaChar(charValue);
            case NaValue<bool> boolValue:
                return FromNaBoolean(boolValue);
            case NaValue<DateTime> dateTimeValue:
                return FromNaDateTime(dateTimeValue);
            case NaValue<DateTimeOffset> dateTimeOffsetValue:
                return FromNaDateTimeOffset(dateTimeOffsetValue);
            case NaInt<DateTime64> dateTime64Value:
                return FromNaDateTime64(dateTime64Value);
            case VectorValue vectorValue:
                return vectorValue;
            default:
                break;
        }

        throw new NotSupportedException($"Cannot convert {typeof(T)} to VectorValue.");
    }

    /// <summary>Returns the underlying <see cref="sbyte"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Int8"/>.</exception>
    public sbyte ToInt8()
    {
        if (_kind != VectorDataType.Int8)
        {
            throw new InvalidOperationException($"Cannot convert to Int8 from {_kind}.");
        }

        return (sbyte)_bits;
    }

    /// <summary>Returns the underlying <see cref="short"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Int16"/>.</exception>
    public short ToInt16()
    {
        if (_kind != VectorDataType.Int16)
        {
            throw new InvalidOperationException($"Cannot convert to Int16 from {_kind}.");
        }

        return (short)_bits;
    }

    /// <summary>Returns the underlying <see cref="int"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Int32"/>.</exception>
    public int ToInt32()
    {
        if (_kind != VectorDataType.Int32)
        {
            throw new InvalidOperationException($"Cannot convert to Int32 from {_kind}.");
        }

        return (int)_bits;
    }

    /// <summary>Returns the underlying <see cref="long"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Int64"/>.</exception>
    public long ToInt64()
    {
        if (_kind != VectorDataType.Int64)
        {
            throw new InvalidOperationException($"Cannot convert to Int64 from {_kind}.");
        }

        return (long)_bits;
    }

    /// <summary>Returns the underlying <see cref="byte"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.UInt8"/>.</exception>
    public byte ToUInt8()
    {
        if (_kind != VectorDataType.UInt8)
        {
            throw new InvalidOperationException($"Cannot convert to UInt8 from {_kind}.");
        }

        return (byte)_bits;
    }

    /// <summary>Returns the underlying <see cref="ushort"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.UInt16"/>.</exception>
    public ushort ToUInt16()
    {
        if (_kind != VectorDataType.UInt16)
        {
            throw new InvalidOperationException($"Cannot convert to UInt16 from {_kind}.");
        }

        return (ushort)_bits;
    }

    /// <summary>Returns the underlying <see cref="uint"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.UInt32"/>.</exception>
    public uint ToUInt32()
    {
        if (_kind != VectorDataType.UInt32)
        {
            throw new InvalidOperationException($"Cannot convert to UInt32 from {_kind}.");
        }

        return (uint)_bits;
    }

    /// <summary>Returns the underlying <see cref="ulong"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.UInt64"/>.</exception>
    public ulong ToUInt64()
    {
        if (_kind != VectorDataType.UInt64)
        {
            throw new InvalidOperationException($"Cannot convert to UInt64 from {_kind}.");
        }

        return _bits;
    }

    /// <summary>Returns the underlying <see cref="Half"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Float16"/>.</exception>
    public Half ToFloat16()
    {
        if (_kind != VectorDataType.Float16)
        {
            throw new InvalidOperationException($"Cannot convert to Float16 from {_kind}.");
        }

        return BitConverter.UInt16BitsToHalf((ushort)_bits);
    }

    /// <summary>Returns the underlying <see cref="float"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Float32"/>.</exception>
    public float ToFloat32()
    {
        if (_kind != VectorDataType.Float32)
        {
            throw new InvalidOperationException($"Cannot convert to Float32 from {_kind}.");
        }

        return BitConverter.UInt32BitsToSingle((uint)_bits);
    }

    /// <summary>Returns the underlying <see cref="double"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Float64"/>.</exception>
    public double ToFloat64()
    {
        if (_kind != VectorDataType.Float64)
        {
            throw new InvalidOperationException($"Cannot convert to Float64 from {_kind}.");
        }

        return BitConverter.UInt64BitsToDouble(_bits);
    }

    /// <summary>Returns the underlying <see cref="char"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Char"/>.</exception>
    public char ToChar()
    {
        if (_kind != VectorDataType.Char)
        {
            throw new InvalidOperationException($"Cannot convert to Char from {_kind}.");
        }

        return (char)_bits;
    }

    /// <summary>Returns the underlying <see cref="bool"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.Bool"/>.</exception>
    public bool ToBoolean()
    {
        if (_kind != VectorDataType.Bool)
        {
            throw new InvalidOperationException($"Cannot convert to Boolean from {_kind}.");
        }

        return _bits != 0;
    }

    /// <summary>Returns the underlying <see cref="DateTime"/> value (UTC kind).</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.DateTime"/>.</exception>
    public DateTime ToDateTime()
    {
        if (_kind != VectorDataType.DateTime)
        {
            throw new InvalidOperationException($"Cannot convert to DateTime from {_kind}.");
        }

        return new DateTime((long)_bits, DateTimeKind.Utc);
    }

    /// <summary>Returns the underlying <see cref="DateTimeOffset"/> value (zero offset).</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.DateTimeOffset"/>.</exception>
    public DateTimeOffset ToDateTimeOffset()
    {
        if (_kind != VectorDataType.DateTimeOffset)
        {
            throw new InvalidOperationException($"Cannot convert to DateTimeOffset from {_kind}.");
        }

        return new DateTimeOffset((long)_bits, TimeSpan.Zero);
    }

    /// <summary>Returns the underlying <see cref="DateTime64"/> value.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.DateTime64"/>.</exception>
    public DateTime64 ToDateTime64()
    {
        if (_kind != VectorDataType.DateTime64)
        {
            throw new InvalidOperationException($"Cannot convert to DateTime64 from {_kind}.");
        }

        return new DateTime64((long)_bits);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="sbyte"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaInt8"/>.</exception>
    public NaInt<sbyte> ToNaInt8()
    {
        if (_kind != VectorDataType.NaInt8)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt8 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<sbyte>.Na : (sbyte)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="short"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaInt16"/>.</exception>
    public NaInt<short> ToNaInt16()
    {
        if (_kind != VectorDataType.NaInt16)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt16 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<short>.Na : (short)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="int"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaInt32"/>.</exception>
    public NaInt<int> ToNaInt32()
    {
        if (_kind != VectorDataType.NaInt32)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt32 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<int>.Na : (int)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="long"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaInt64"/>.</exception>
    public NaInt<long> ToNaInt64()
    {
        if (_kind != VectorDataType.NaInt64)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt64 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<long>.Na : (long)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="byte"/>.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaUInt8"/>.</exception>
    public NaInt<byte> ToNaUInt8()
    {
        if (_kind != VectorDataType.NaUInt8)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt8 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<byte>.Na : (byte)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="ushort"/>.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaUInt16"/>.</exception>
    public NaInt<ushort> ToNaUInt16()
    {
        if (_kind != VectorDataType.NaUInt16)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt16 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<ushort>.Na : (ushort)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="uint"/>.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaUInt32"/>.</exception>
    public NaInt<uint> ToNaUInt32()
    {
        if (_kind != VectorDataType.NaUInt32)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt32 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<uint>.Na : (uint)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="ulong"/>.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaUInt64"/>.</exception>
    public NaInt<ulong> ToNaUInt64()
    {
        if (_kind != VectorDataType.NaUInt64)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt64 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<ulong>.Na : _bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaFloat{T}"/> over <see cref="float"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaFloat32"/>.</exception>
    public NaFloat<float> ToNaFloat32()
    {
        if (_kind != VectorDataType.NaFloat32)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat32 from {_kind}.");
        }

        return IsNaSentinel ? NaFloat<float>.Na : BitConverter.UInt32BitsToSingle((uint)_bits);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaFloat{T}"/> over <see cref="Half"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaFloat16"/>.</exception>
    public NaFloat<Half> ToNaFloat16()
    {
        if (_kind != VectorDataType.NaFloat16)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat16 from {_kind}.");
        }

        return IsNaSentinel ? NaFloat<Half>.Na : BitConverter.UInt16BitsToHalf((ushort)_bits);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaFloat{T}"/> over <see cref="double"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaFloat64"/>.</exception>
    public NaFloat<double> ToNaFloat64()
    {
        if (_kind != VectorDataType.NaFloat64)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat64 from {_kind}.");
        }

        return IsNaSentinel ? NaFloat<double>.Na : BitConverter.UInt64BitsToDouble(_bits);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaValue{T}"/> over <see cref="string"/>. A null reference is decoded as Na.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaString"/>.</exception>
    public NaValue<string> ToNaString()
    {
        if (_kind != VectorDataType.NaString)
        {
            throw new InvalidOperationException($"Cannot convert to NaString from {_kind}.");
        }

        // _ref==null encodes Na for NaString; the implicit operator NaValue<string>(string?)
        // returns the Na default for null and wraps non-null strings.
        return (string?)_ref;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaValue{T}"/> over <see cref="char"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaChar"/>.</exception>
    public NaValue<char> ToNaChar()
    {
        if (_kind != VectorDataType.NaChar)
        {
            throw new InvalidOperationException($"Cannot convert to NaChar from {_kind}.");
        }

        // Important: explicit `default(NaValue<char>)` — using bare `default` here
        // would let the conditional infer `char` (from the other branch) and bind
        // the result to NaValue<char>('\0') via the implicit operator.
        return IsNaSentinel ? default(NaValue<char>) : (char)_bits;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaValue{T}"/> over <see cref="bool"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaBool"/>.</exception>
    public NaValue<bool> ToNaBoolean()
    {
        if (_kind != VectorDataType.NaBool)
        {
            throw new InvalidOperationException($"Cannot convert to NaBool from {_kind}.");
        }

        return IsNaSentinel ? default(NaValue<bool>) : _bits != 0;
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaValue{T}"/> over <see cref="DateTime"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaDateTime"/>.</exception>
    public NaValue<DateTime> ToNaDateTime()
    {
        if (_kind != VectorDataType.NaDateTime)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTime from {_kind}.");
        }

        return IsNaSentinel ? default(NaValue<DateTime>) : new DateTime((long)_bits, DateTimeKind.Utc);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaValue{T}"/> over <see cref="DateTimeOffset"/>; preserves NA-ness across the round-trip.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaDateTimeOffset"/>.</exception>
    public NaValue<DateTimeOffset> ToNaDateTimeOffset()
    {
        if (_kind != VectorDataType.NaDateTimeOffset)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTimeOffset from {_kind}.");
        }

        return IsNaSentinel ? default(NaValue<DateTimeOffset>) : new DateTimeOffset((long)_bits, TimeSpan.Zero);
    }

    /// <summary>Returns the underlying NA-aware <see cref="NaInt{T}"/> over <see cref="DateTime64"/>.</summary>
    /// <exception cref="InvalidOperationException">The data type is not <see cref="VectorDataType.NaDateTime64"/>.</exception>
    public NaInt<DateTime64> ToNaDateTime64()
    {
        if (_kind != VectorDataType.NaDateTime64)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTime64 from {_kind}.");
        }

        return IsNaSentinel ? NaInt<DateTime64>.Na : new DateTime64((long)_bits);
    }

    /// <summary>
    /// Returns the value's invariant string representation. Numeric and date types
    /// format with <see cref="CultureInfo.InvariantCulture"/>; NA-aware numeric and
    /// date types render as <see cref="NaValue.NaValueLabel"/> when the value is Na;
    /// strings round-trip as themselves with <see cref="NaValue.NullValueLabel"/>
    /// for a null <see cref="VectorDataType.String"/>.
    /// </summary>
    public override string ToString()
    {
        return _kind switch
        {
            VectorDataType.Bool => ToBoolean().ToString(),
            VectorDataType.Char => ToChar().ToString(),
            VectorDataType.DateTime => ToDateTime().ToString(CultureInfo.InvariantCulture),
            VectorDataType.DateTime64 => ToDateTime64().ToString(),
            VectorDataType.DateTimeOffset => ToDateTimeOffset().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Float16 => ToFloat16().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Float32 => ToFloat32().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Float64 => ToFloat64().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Int16 => ToInt16().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Int32 => ToInt32().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Int64 => ToInt64().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Int8 => ToInt8().ToString(CultureInfo.InvariantCulture),
            VectorDataType.String => _ref?.ToString() ?? NaValue.NullValueLabel,
            VectorDataType.UInt16 => ToUInt16().ToString(CultureInfo.InvariantCulture),
            VectorDataType.UInt32 => ToUInt32().ToString(CultureInfo.InvariantCulture),
            VectorDataType.UInt64 => ToUInt64().ToString(CultureInfo.InvariantCulture),
            VectorDataType.UInt8 => ToUInt8().ToString(CultureInfo.InvariantCulture),
            // Na* numeric branches mirror their non-Na siblings by formatting with
            // CultureInfo.InvariantCulture; NaInt<T>.ToString(format, provider) and
            // NaFloat<T>.ToString(format, provider) honour IsNa internally and emit
            // NaValue.NaValueLabel themselves.
            VectorDataType.NaBool => ToNaBoolean().ToString(),
            VectorDataType.NaChar => ToNaChar().ToString(),
            VectorDataType.NaDateTime => ToNaDateTime().IsNa
                ? NaValue.NaValueLabel
                : ToNaDateTime().Value.ToString(CultureInfo.InvariantCulture),
            VectorDataType.NaDateTime64 => ToNaDateTime64().ToString() ?? NaValue.NaValueLabel,
            VectorDataType.NaDateTimeOffset => ToNaDateTimeOffset().IsNa
                ? NaValue.NaValueLabel
                : ToNaDateTimeOffset().Value.ToString(CultureInfo.InvariantCulture),
            VectorDataType.NaFloat16 => ToNaFloat16().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaFloat32 => ToNaFloat32().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaFloat64 => ToNaFloat64().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaInt16 => ToNaInt16().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaInt32 => ToNaInt32().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaInt64 => ToNaInt64().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaInt8 => ToNaInt8().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaString => ToNaString().ToString(),
            VectorDataType.NaUInt16 => ToNaUInt16().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaUInt32 => ToNaUInt32().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaUInt64 => ToNaUInt64().ToString("G", CultureInfo.InvariantCulture),
            VectorDataType.NaUInt8 => ToNaUInt8().ToString("G", CultureInfo.InvariantCulture),
            _ => _bits.ToString(CultureInfo.InvariantCulture),
        };
    }

    /// <summary>
    /// Compares this value with <paramref name="other"/> by both data type and
    /// underlying bits/reference. Two NA values of the same kind compare equal;
    /// two values of different kinds compare unequal even if their bit patterns coincide.
    /// </summary>
    public bool Equals(VectorValue other)
    {
        return _kind == other._kind
            && _bits == other._bits
            && Equals(_ref, other._ref);
    }

    /// <summary>
    /// Returns <see langword="true"/> when this value is a <see cref="VectorDataType.Float64"/>
    /// equal in bit pattern to <paramref name="other"/>. Note: this is bitwise equality,
    /// so positive/negative zero are distinct and <see cref="double.NaN"/> only equals NaN
    /// with the same bit pattern.
    /// </summary>
    public bool Equals(double other)
    {
        return _kind == VectorDataType.Float64
            && _bits == BitConverter.DoubleToUInt64Bits(other);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is VectorValue other)
        {
            return Equals(other);
        }

        if (obj is double otherDouble)
        {
            return Equals(otherDouble);
        }

        return false;
    }

    /// <summary>Returns a hash code derived from the data type, the bit pattern and the optional reference.</summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(_bits, _ref, _kind);
    }

    /// <summary>Equality operator. See <see cref="Equals(VectorValue)"/>.</summary>
    public static bool operator ==(VectorValue left, VectorValue right)
    {
        return left.Equals(right);
    }

    /// <summary>Inequality operator. See <see cref="Equals(VectorValue)"/>.</summary>
    public static bool operator !=(VectorValue left, VectorValue right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>Implicitly wraps an <see cref="sbyte"/> via <see cref="FromInt8(sbyte)"/>.</summary>
    public static implicit operator VectorValue(sbyte value)
    {
        return FromInt8(value);
    }

    /// <summary>Implicitly wraps a <see cref="short"/> via <see cref="FromInt16(short)"/>.</summary>
    public static implicit operator VectorValue(short value)
    {
        return FromInt16(value);
    }

    /// <summary>Implicitly wraps an <see cref="int"/> via <see cref="FromInt32(int)"/>.</summary>
    public static implicit operator VectorValue(int value)
    {
        return FromInt32(value);
    }

    /// <summary>Implicitly wraps a <see cref="long"/> via <see cref="FromInt64(long)"/>.</summary>
    public static implicit operator VectorValue(long value)
    {
        return FromInt64(value);
    }

    /// <summary>Implicitly wraps a <see cref="byte"/> via <see cref="FromUInt8(byte)"/>.</summary>
    public static implicit operator VectorValue(byte value)
    {
        return FromUInt8(value);
    }

    /// <summary>Implicitly wraps a <see cref="ushort"/> via <see cref="FromUInt16(ushort)"/>.</summary>
    public static implicit operator VectorValue(ushort value)
    {
        return FromUInt16(value);
    }

    /// <summary>Implicitly wraps a <see cref="uint"/> via <see cref="FromUInt32(uint)"/>.</summary>
    public static implicit operator VectorValue(uint value)
    {
        return FromUInt32(value);
    }

    /// <summary>Implicitly wraps a <see cref="ulong"/> via <see cref="FromUInt64(ulong)"/>.</summary>
    public static implicit operator VectorValue(ulong value)
    {
        return FromUInt64(value);
    }

    /// <summary>Implicitly wraps a <see cref="float"/> via <see cref="FromFloat32(float)"/>.</summary>
    public static implicit operator VectorValue(float value)
    {
        return FromFloat32(value);
    }

    /// <summary>Implicitly wraps a <see cref="double"/> via <see cref="FromFloat64(double)"/>.</summary>
    public static implicit operator VectorValue(double value)
    {
        return FromFloat64(value);
    }

    /// <summary>Implicitly wraps a <see cref="Half"/> via <see cref="FromFloat16(Half)"/>.</summary>
    public static implicit operator VectorValue(Half value)
    {
        return FromFloat16(value);
    }

    /// <summary>Implicitly wraps a <see cref="string"/> via <see cref="FromString(string)"/>.</summary>
    public static implicit operator VectorValue(string value)
    {
        return FromString(value);
    }

    /// <summary>Implicitly wraps a <see cref="char"/> via <see cref="FromChar(char)"/>.</summary>
    public static implicit operator VectorValue(char value)
    {
        return FromChar(value);
    }

    /// <summary>Implicitly wraps a <see cref="bool"/> via <see cref="FromBoolean(bool)"/>.</summary>
    public static implicit operator VectorValue(bool value)
    {
        return FromBoolean(value);
    }

    /// <summary>Implicitly wraps a <see cref="DateTime"/> via <see cref="FromDateTime(DateTime)"/>.</summary>
    public static implicit operator VectorValue(DateTime value)
    {
        return FromDateTime(value);
    }

    /// <summary>Implicitly wraps a <see cref="DateTimeOffset"/> via <see cref="FromDateTimeOffset(DateTimeOffset)"/>.</summary>
    public static implicit operator VectorValue(DateTimeOffset value)
    {
        return FromDateTimeOffset(value);
    }

    /// <summary>Implicitly wraps a <see cref="DateTime64"/> via <see cref="FromDateTime64(DateTime64)"/>.</summary>
    public static implicit operator VectorValue(DateTime64 value)
    {
        return FromDateTime64(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="sbyte"/>.</summary>
    public static implicit operator VectorValue(NaInt<sbyte> value)
    {
        return FromNaInt8(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="short"/>.</summary>
    public static implicit operator VectorValue(NaInt<short> value)
    {
        return FromNaInt16(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="int"/>.</summary>
    public static implicit operator VectorValue(NaInt<int> value)
    {
        return FromNaInt32(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="long"/>.</summary>
    public static implicit operator VectorValue(NaInt<long> value)
    {
        return FromNaInt64(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="byte"/>.</summary>
    public static implicit operator VectorValue(NaInt<byte> value)
    {
        return FromNaUInt8(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="ushort"/>.</summary>
    public static implicit operator VectorValue(NaInt<ushort> value)
    {
        return FromNaUInt16(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="uint"/>.</summary>
    public static implicit operator VectorValue(NaInt<uint> value)
    {
        return FromNaUInt32(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="ulong"/>.</summary>
    public static implicit operator VectorValue(NaInt<ulong> value)
    {
        return FromNaUInt64(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaFloat{T}"/> over <see cref="float"/>.</summary>
    public static implicit operator VectorValue(NaFloat<float> value)
    {
        return FromNaFloat32(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaFloat{T}"/> over <see cref="double"/>.</summary>
    public static implicit operator VectorValue(NaFloat<double> value)
    {
        return FromNaFloat64(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaFloat{T}"/> over <see cref="Half"/>.</summary>
    public static implicit operator VectorValue(NaFloat<Half> value)
    {
        return FromNaFloat16(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaValue{T}"/> over <see cref="string"/>.</summary>
    public static implicit operator VectorValue(NaValue<string> value)
    {
        return FromNaString(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaValue{T}"/> over <see cref="char"/>.</summary>
    public static implicit operator VectorValue(NaValue<char> value)
    {
        return FromNaChar(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaValue{T}"/> over <see cref="bool"/>.</summary>
    public static implicit operator VectorValue(NaValue<bool> value)
    {
        return FromNaBoolean(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaValue{T}"/> over <see cref="DateTime"/>.</summary>
    public static implicit operator VectorValue(NaValue<DateTime> value)
    {
        return FromNaDateTime(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaValue{T}"/> over <see cref="DateTimeOffset"/>.</summary>
    public static implicit operator VectorValue(NaValue<DateTimeOffset> value)
    {
        return FromNaDateTimeOffset(value);
    }

    /// <summary>Implicitly wraps an <see cref="NaInt{T}"/> over <see cref="DateTime64"/>.</summary>
    public static implicit operator VectorValue(NaInt<DateTime64> value)
    {
        return FromNaDateTime64(value);
    }

    /// <summary>Explicitly extracts the <see cref="sbyte"/> value via <see cref="ToInt8"/>.</summary>
    public static explicit operator sbyte(VectorValue value)
    {
        return value.ToInt8();
    }

    /// <summary>Explicitly extracts the <see cref="short"/> value via <see cref="ToInt16"/>.</summary>
    public static explicit operator short(VectorValue value)
    {
        return value.ToInt16();
    }

    /// <summary>Explicitly extracts the <see cref="int"/> value via <see cref="ToInt32"/>.</summary>
    public static explicit operator int(VectorValue value)
    {
        return value.ToInt32();
    }

    /// <summary>Explicitly extracts the <see cref="long"/> value via <see cref="ToInt64"/>.</summary>
    public static explicit operator long(VectorValue value)
    {
        return value.ToInt64();
    }

    /// <summary>Explicitly extracts the <see cref="byte"/> value via <see cref="ToUInt8"/>.</summary>
    public static explicit operator byte(VectorValue value)
    {
        return value.ToUInt8();
    }

    /// <summary>Explicitly extracts the <see cref="ushort"/> value via <see cref="ToUInt16"/>.</summary>
    public static explicit operator ushort(VectorValue value)
    {
        return value.ToUInt16();
    }

    /// <summary>Explicitly extracts the <see cref="uint"/> value via <see cref="ToUInt32"/>.</summary>
    public static explicit operator uint(VectorValue value)
    {
        return value.ToUInt32();
    }

    /// <summary>Explicitly extracts the <see cref="ulong"/> value via <see cref="ToUInt64"/>.</summary>
    public static explicit operator ulong(VectorValue value)
    {
        return value.ToUInt64();
    }

    /// <summary>Explicitly extracts the <see cref="float"/> value via <see cref="ToFloat32"/>.</summary>
    public static explicit operator float(VectorValue value)
    {
        return value.ToFloat32();
    }

    /// <summary>Explicitly extracts the <see cref="double"/> value via <see cref="ToFloat64"/>.</summary>
    public static explicit operator double(VectorValue value)
    {
        return value.ToFloat64();
    }

    /// <summary>Explicitly extracts the <see cref="Half"/> value via <see cref="ToFloat16"/>.</summary>
    public static explicit operator Half(VectorValue value)
    {
        return value.ToFloat16();
    }

    /// <summary>Returns the value's invariant string representation via <see cref="ToString"/>.</summary>
    public static explicit operator string(VectorValue value)
    {
        return value.ToString();
    }

    /// <summary>Explicitly extracts the <see cref="char"/> value via <see cref="ToChar"/>.</summary>
    public static explicit operator char(VectorValue value)
    {
        return value.ToChar();
    }

    /// <summary>Explicitly extracts the <see cref="bool"/> value via <see cref="ToBoolean"/>.</summary>
    public static explicit operator bool(VectorValue value)
    {
        return value.ToBoolean();
    }
}
