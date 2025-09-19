// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A weakly-typed value that can be used to represent any of the supported <see cref="Vector{T}"/> types
/// (see <see cref="VectorDataType"/>).
/// </summary>
public readonly struct VectorValue
    : IEquatable<VectorValue>,
      IEquatable<double>
{
    private readonly ulong _bits;
    private readonly object? _ref;
    private readonly VectorDataType _kind;

    private VectorValue(ulong bits, string? str, VectorDataType kind)
    {
        _bits = bits;
        _ref = str;
        _kind = kind;
    }

    public VectorDataType DataType => _kind;

    public static VectorValue FromInt8(sbyte value)
    {
        return new((ulong)value, null, VectorDataType.Int8);
    }

    public static VectorValue FromInt16(short value)
    {
        return new((ulong)value, null, VectorDataType.Int16);
    }

    public static VectorValue FromInt32(int value)
    {
        return new((ulong)value, null, VectorDataType.Int32);
    }

    public static VectorValue FromInt64(long value)
    {
        return new((ulong)value, null, VectorDataType.Int64);
    }

    public static VectorValue FromUInt8(byte value)
    {
        return new(value, null, VectorDataType.UInt8);
    }

    public static VectorValue FromUInt16(ushort value)
    {
        return new(value, null, VectorDataType.UInt16);
    }

    public static VectorValue FromUInt32(uint value)
    {
        return new(value, null, VectorDataType.UInt32);
    }

    public static VectorValue FromUInt64(ulong value)
    {
        return new(value, null, VectorDataType.UInt64);
    }

    public static VectorValue FromFloat16(Half value)
    {
        return new(BitConverter.HalfToUInt16Bits(value), null, VectorDataType.Float16);
    }

    public static VectorValue FromFloat32(float value)
    {
        return new(BitConverter.SingleToUInt32Bits(value), null, VectorDataType.Float32);
    }

    public static VectorValue FromFloat64(double value)
    {
        return new(BitConverter.DoubleToUInt64Bits(value), null, VectorDataType.Float64);
    }

    public static VectorValue FromString(string value)
    {
        return new(0, value, VectorDataType.String);
    }

    public static VectorValue FromChar(char value)
    {
        return new(value, null, VectorDataType.Char);
    }

    public static VectorValue FromBoolean(bool value)
    {
        return new(value ? (ulong)1 : 0, null, VectorDataType.Bool);
    }

    public static VectorValue FromDateTime64(DateTime64 value)
    {
        return new((ulong)value.TotalMilliseconds, null, VectorDataType.DateTime64);
    }

    public static VectorValue FromDateTime(DateTime value)
    {
        return new((ulong)value.Ticks, null, VectorDataType.DateTime);
    }

    public static VectorValue FromDateTimeOffset(DateTimeOffset value)
    {
        return new((ulong)value.Ticks, null, VectorDataType.DateTimeOffset);
    }

    public static VectorValue FromNaInt8(NaInt<sbyte> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaInt8);
        }

        return new((ulong)(sbyte)value, null, VectorDataType.NaInt8);
    }

    public static VectorValue FromNaInt16(NaInt<short> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaInt16);
        }

        return new((ulong)(short)value, null, VectorDataType.NaInt16);
    }

    public static VectorValue FromNaInt32(NaInt<int> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaInt32);
        }

        return new((ulong)(int)value, null, VectorDataType.NaInt32);
    }

    public static VectorValue FromNaInt64(NaInt<long> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaInt64);
        }

        return new((ulong)(long)value, null, VectorDataType.NaInt64);
    }

    public static VectorValue FromNaUInt8(NaInt<byte> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaUInt8);
        }

        return new((byte)value, null, VectorDataType.NaUInt8);
    }

    public static VectorValue FromNaUInt16(NaInt<ushort> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaUInt16);
        }

        return new((ushort)value, null, VectorDataType.NaUInt16);
    }

    public static VectorValue FromNaUInt32(NaInt<uint> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaUInt32);
        }

        return new((uint)value, null, VectorDataType.NaUInt32);
    }

    public static VectorValue FromNaUInt64(NaInt<ulong> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaUInt64);
        }

        return new((ulong)value, null, VectorDataType.NaUInt64);
    }

    public static VectorValue FromNaFloat32(NaFloat<float> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaFloat32);
        }

        return new(BitConverter.SingleToUInt32Bits((float)value), null, VectorDataType.NaFloat32);
    }

    public static VectorValue FromNaFloat16(NaFloat<Half> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaFloat16);
        }

        return new(BitConverter.HalfToUInt16Bits((Half)value), null, VectorDataType.NaFloat16);
    }

    public static VectorValue FromNaFloat64(NaFloat<double> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaFloat64);
        }

        return new(BitConverter.DoubleToUInt64Bits((double)value), null, VectorDataType.NaFloat64);
    }

    public static VectorValue FromNaString(NaValue<string> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaString);
        }

        return new(0, (string)value, VectorDataType.NaString);
    }

    public static VectorValue FromNaChar(NaValue<char> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaChar);
        }

        return new((char)value, null, VectorDataType.NaChar);
    }

    public static VectorValue FromNaBoolean(NaValue<bool> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaBool);
        }

        return new(((bool)value) ? (ulong)1 : 0, null, VectorDataType.NaBool);
    }

    public static VectorValue FromNaDateTime64(NaInt<DateTime64> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaDateTime64);
        }

        return new((ulong)((DateTime64)value).TotalMilliseconds, null, VectorDataType.NaDateTime64);
    }

    public static VectorValue FromNaDateTime(NaValue<DateTime> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaDateTime);
        }

        return new((ulong)((DateTime)value).Ticks, null, VectorDataType.NaDateTime);
    }

    public static VectorValue FromNaDateTimeOffset(NaValue<DateTimeOffset> value)
    {
        if (value.IsNa)
        {
            return new(0, null, VectorDataType.NaDateTimeOffset);
        }

        return new((ulong)((DateTimeOffset)value).Ticks, null, VectorDataType.NaDateTimeOffset);
    }

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

    public sbyte ToInt8()
    {
        if (_kind != VectorDataType.Int8)
        {
            throw new InvalidOperationException($"Cannot convert to Int8 from {_kind}.");
        }

        return (sbyte)_bits;
    }

    public short ToInt16()
    {
        if (_kind != VectorDataType.Int16)
        {
            throw new InvalidOperationException($"Cannot convert to Int16 from {_kind}.");
        }

        return (short)_bits;
    }

    public int ToInt32()
    {
        if (_kind != VectorDataType.Int32)
        {
            throw new InvalidOperationException($"Cannot convert to Int32 from {_kind}.");
        }

        return (int)_bits;
    }

    public long ToInt64()
    {
        if (_kind != VectorDataType.Int64)
        {
            throw new InvalidOperationException($"Cannot convert to Int64 from {_kind}.");
        }

        return (long)_bits;
    }

    public byte ToUInt8()
    {
        if (_kind != VectorDataType.UInt8)
        {
            throw new InvalidOperationException($"Cannot convert to UInt8 from {_kind}.");
        }

        return (byte)_bits;
    }

    public ushort ToUInt16()
    {
        if (_kind != VectorDataType.UInt16)
        {
            throw new InvalidOperationException($"Cannot convert to UInt16 from {_kind}.");
        }

        return (ushort)_bits;
    }

    public uint ToUInt32()
    {
        if (_kind != VectorDataType.UInt32)
        {
            throw new InvalidOperationException($"Cannot convert to UInt32 from {_kind}.");
        }

        return (uint)_bits;
    }

    public ulong ToUInt64()
    {
        if (_kind != VectorDataType.UInt64)
        {
            throw new InvalidOperationException($"Cannot convert to UInt64 from {_kind}.");
        }

        return _bits;
    }

    public Half ToFloat16()
    {
        if (_kind != VectorDataType.Float16)
        {
            throw new InvalidOperationException($"Cannot convert to Float16 from {_kind}.");
        }

        return BitConverter.UInt16BitsToHalf((ushort)_bits);
    }

    public float ToFloat32()
    {
        if (_kind != VectorDataType.Float32)
        {
            throw new InvalidOperationException($"Cannot convert to Float32 from {_kind}.");
        }

        return BitConverter.UInt32BitsToSingle((uint)_bits);
    }

    public double ToFloat64()
    {
        if (_kind != VectorDataType.Float64)
        {
            throw new InvalidOperationException($"Cannot convert to Float64 from {_kind}.");
        }

        return BitConverter.UInt64BitsToDouble(_bits);
    }

    public char ToChar()
    {
        if (_kind != VectorDataType.Char)
        {
            throw new InvalidOperationException($"Cannot convert to Char from {_kind}.");
        }

        return (char)_bits;
    }

    public bool ToBoolean()
    {
        if (_kind != VectorDataType.Bool)
        {
            throw new InvalidOperationException($"Cannot convert to Boolean from {_kind}.");
        }

        return _bits != 0;
    }

    public DateTime ToDateTime()
    {
        if (_kind != VectorDataType.DateTime)
        {
            throw new InvalidOperationException($"Cannot convert to DateTime from {_kind}.");
        }

        return new DateTime((long)_bits, DateTimeKind.Utc);
    }

    public DateTimeOffset ToDateTimeOffset()
    {
        if (_kind != VectorDataType.DateTimeOffset)
        {
            throw new InvalidOperationException($"Cannot convert to DateTimeOffset from {_kind}.");
        }

        return new DateTimeOffset((long)_bits, TimeSpan.Zero);
    }

    public DateTime64 ToDateTime64()
    {
        if (_kind != VectorDataType.DateTime64)
        {
            throw new InvalidOperationException($"Cannot convert to DateTime64 from {_kind}.");
        }

        return new DateTime64((long)_bits);
    }

    public NaInt<sbyte> ToNaInt8()
    {
        if (_kind != VectorDataType.NaInt8)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt8 from {_kind}.");
        }

        return (sbyte)_bits;
    }

    public NaInt<short> ToNaInt16()
    {
        if (_kind != VectorDataType.NaInt16)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt16 from {_kind}.");
        }

        return (short)_bits;
    }

    public NaInt<int> ToNaInt32()
    {
        if (_kind != VectorDataType.NaInt32)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt32 from {_kind}.");
        }

        return (int)_bits;
    }

    public NaInt<long> ToNaInt64()
    {
        if (_kind != VectorDataType.NaInt64)
        {
            throw new InvalidOperationException($"Cannot convert to NaInt64 from {_kind}.");
        }

        return (long)_bits;
    }

    public NaInt<byte> ToNaUInt8()
    {
        if (_kind != VectorDataType.NaUInt8)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt8 from {_kind}.");
        }

        return (byte)_bits;
    }

    public NaInt<ushort> ToNaUInt16()
    {
        if (_kind != VectorDataType.NaUInt16)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt16 from {_kind}.");
        }

        return (ushort)_bits;
    }

    public NaInt<uint> ToNaUInt32()
    {
        if (_kind != VectorDataType.NaUInt32)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt32 from {_kind}.");
        }

        return (uint)_bits;
    }

    public NaInt<ulong> ToNaUInt64()
    {
        if (_kind != VectorDataType.NaUInt64)
        {
            throw new InvalidOperationException($"Cannot convert to NaUInt64 from {_kind}.");
        }

        return _bits;
    }

    public NaFloat<float> ToNaFloat32()
    {
        if (_kind != VectorDataType.NaFloat32)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat32 from {_kind}.");
        }

        return BitConverter.UInt32BitsToSingle((uint)_bits);
    }

    public NaFloat<Half> ToNaFloat16()
    {
        if (_kind != VectorDataType.NaFloat16)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat16 from {_kind}.");
        }

        return BitConverter.UInt16BitsToHalf((ushort)_bits);
    }

    public NaFloat<double> ToNaFloat64()
    {
        if (_kind != VectorDataType.NaFloat64)
        {
            throw new InvalidOperationException($"Cannot convert to NaFloat64 from {_kind}.");
        }

        return BitConverter.UInt64BitsToDouble(_bits);
    }

    public NaValue<string> ToNaString()
    {
        if (_kind != VectorDataType.NaString)
        {
            throw new InvalidOperationException($"Cannot convert to NaString from {_kind}.");
        }

        return (string?)_ref ?? string.Empty;
    }

    public NaValue<char> ToNaChar()
    {
        if (_kind != VectorDataType.NaChar)
        {
            throw new InvalidOperationException($"Cannot convert to NaChar from {_kind}.");
        }

        return (char)_bits;
    }

    public NaValue<bool> ToNaBoolean()
    {
        if (_kind != VectorDataType.NaBool)
        {
            throw new InvalidOperationException($"Cannot convert to NaBool from {_kind}.");
        }

        return _bits != 0;
    }

    public NaValue<DateTime> ToNaDateTime()
    {
        if (_kind != VectorDataType.NaDateTime)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTime from {_kind}.");
        }

        return new DateTime((long)_bits, DateTimeKind.Utc);
    }

    public NaValue<DateTimeOffset> ToNaDateTimeOffset()
    {
        if (_kind != VectorDataType.NaDateTimeOffset)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTimeOffset from {_kind}.");
        }

        return new DateTimeOffset((long)_bits, TimeSpan.Zero);
    }

    public NaInt<DateTime64> ToNaDateTime64()
    {
        if (_kind != VectorDataType.NaDateTime64)
        {
            throw new InvalidOperationException($"Cannot convert to NaDateTime64 from {_kind}.");
        }

        return new DateTime64((long)_bits);
    }

    public override string ToString()
    {
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        return _kind switch
        {
            VectorDataType.Bool => ToBoolean().ToString(CultureInfo.InvariantCulture),
            VectorDataType.Char => ToChar().ToString(CultureInfo.InvariantCulture),
            VectorDataType.DateTime => ToDateTime().ToString(CultureInfo.InvariantCulture),
            VectorDataType.DateTime64 => ToDateTime64().ToString(CultureInfo.InvariantCulture),
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
            VectorDataType.NaBool => throw new NotImplementedException(),
            VectorDataType.NaChar => throw new NotImplementedException(),
            VectorDataType.NaDateTime => throw new NotImplementedException(),
            VectorDataType.NaDateTime64 => throw new NotImplementedException(),
            VectorDataType.NaDateTimeOffset => throw new NotImplementedException(),
            VectorDataType.NaFloat16 => throw new NotImplementedException(),
            VectorDataType.NaFloat32 => throw new NotImplementedException(),
            VectorDataType.NaFloat64 => throw new NotImplementedException(),
            VectorDataType.NaInt16 => throw new NotImplementedException(),
            VectorDataType.NaInt32 => throw new NotImplementedException(),
            VectorDataType.NaInt64 => throw new NotImplementedException(),
            VectorDataType.NaInt8 => throw new NotImplementedException(),
            VectorDataType.NaString => throw new NotImplementedException(),
            VectorDataType.NaUInt16 => throw new NotImplementedException(),
            VectorDataType.NaUInt32 => throw new NotImplementedException(),
            VectorDataType.NaUInt64 => throw new NotImplementedException(),
            VectorDataType.NaUInt8 => throw new NotImplementedException(),
            _ => _bits.ToString(CultureInfo.InvariantCulture),
        };
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }

    public bool Equals(VectorValue other)
    {
        return _kind == other._kind
            && _bits == other._bits
            && Equals(_ref, other._ref);
    }

    public bool Equals(double other)
    {
        return _kind == VectorDataType.Float64
            && _bits == BitConverter.DoubleToUInt64Bits(other);
    }

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

    public override int GetHashCode()
    {
        return HashCode.Combine(_bits, _ref, _kind);
    }

    public static bool operator ==(VectorValue left, VectorValue right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(VectorValue left, VectorValue right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static implicit operator VectorValue(sbyte value)
    {
        return FromInt8(value);
    }

    public static implicit operator VectorValue(short value)
    {
        return FromInt16(value);
    }

    public static implicit operator VectorValue(int value)
    {
        return FromInt32(value);
    }

    public static implicit operator VectorValue(long value)
    {
        return FromInt64(value);
    }

    public static implicit operator VectorValue(byte value)
    {
        return FromUInt8(value);
    }

    public static implicit operator VectorValue(ushort value)
    {
        return FromUInt16(value);
    }

    public static implicit operator VectorValue(uint value)
    {
        return FromUInt32(value);
    }

    public static implicit operator VectorValue(ulong value)
    {
        return FromUInt64(value);
    }

    public static implicit operator VectorValue(float value)
    {
        return FromFloat32(value);
    }

    public static implicit operator VectorValue(double value)
    {
        return FromFloat64(value);
    }

    public static implicit operator VectorValue(Half value)
    {
        return FromFloat16(value);
    }

    public static implicit operator VectorValue(string value)
    {
        return FromString(value);
    }

    public static implicit operator VectorValue(char value)
    {
        return FromChar(value);
    }

    public static implicit operator VectorValue(bool value)
    {
        return FromBoolean(value);
    }

    public static implicit operator VectorValue(DateTime value)
    {
        return FromDateTime(value);
    }

    public static implicit operator VectorValue(DateTimeOffset value)
    {
        return FromDateTimeOffset(value);
    }

    public static implicit operator VectorValue(DateTime64 value)
    {
        return FromDateTime64(value);
    }

    public static implicit operator VectorValue(NaInt<sbyte> value)
    {
        return FromNaInt8(value);
    }

    public static implicit operator VectorValue(NaInt<short> value)
    {
        return FromNaInt16(value);
    }

    public static implicit operator VectorValue(NaInt<int> value)
    {
        return FromNaInt32(value);
    }

    public static implicit operator VectorValue(NaInt<long> value)
    {
        return FromNaInt64(value);
    }

    public static implicit operator VectorValue(NaInt<byte> value)
    {
        return FromNaUInt8(value);
    }

    public static implicit operator VectorValue(NaInt<ushort> value)
    {
        return FromNaUInt16(value);
    }

    public static implicit operator VectorValue(NaInt<uint> value)
    {
        return FromNaUInt32(value);
    }

    public static implicit operator VectorValue(NaInt<ulong> value)
    {
        return FromNaUInt64(value);
    }

    public static implicit operator VectorValue(NaFloat<float> value)
    {
        return FromNaFloat32(value);
    }

    public static implicit operator VectorValue(NaFloat<double> value)
    {
        return FromNaFloat64(value);
    }

    public static implicit operator VectorValue(NaFloat<Half> value)
    {
        return FromNaFloat16(value);
    }

    public static implicit operator VectorValue(NaValue<string> value)
    {
        return FromNaString(value);
    }

    public static implicit operator VectorValue(NaValue<char> value)
    {
        return FromNaChar(value);
    }

    public static implicit operator VectorValue(NaValue<bool> value)
    {
        return FromNaBoolean(value);
    }

    public static implicit operator VectorValue(NaValue<DateTime> value)
    {
        return FromNaDateTime(value);
    }

    public static implicit operator VectorValue(NaValue<DateTimeOffset> value)
    {
        return FromNaDateTimeOffset(value);
    }

    public static implicit operator VectorValue(NaInt<DateTime64> value)
    {
        return FromNaDateTime64(value);
    }

    public static explicit operator sbyte(VectorValue value)
    {
        return value.ToInt8();
    }

    public static explicit operator short(VectorValue value)
    {
        return value.ToInt16();
    }

    public static explicit operator int(VectorValue value)
    {
        return value.ToInt32();
    }

    public static explicit operator long(VectorValue value)
    {
        return value.ToInt64();
    }

    public static explicit operator byte(VectorValue value)
    {
        return value.ToUInt8();
    }

    public static explicit operator ushort(VectorValue value)
    {
        return value.ToUInt16();
    }

    public static explicit operator uint(VectorValue value)
    {
        return value.ToUInt32();
    }

    public static explicit operator ulong(VectorValue value)
    {
        return value.ToUInt64();
    }

    public static explicit operator float(VectorValue value)
    {
        return value.ToFloat32();
    }

    public static explicit operator double(VectorValue value)
    {
        return value.ToFloat64();
    }

    public static explicit operator Half(VectorValue value)
    {
        return value.ToFloat16();
    }

    public static explicit operator string(VectorValue value)
    {
        return value.ToString();
    }

    public static explicit operator char(VectorValue value)
    {
        return value.ToChar();
    }

    public static explicit operator bool(VectorValue value)
    {
        return value.ToBoolean();
    }
}
