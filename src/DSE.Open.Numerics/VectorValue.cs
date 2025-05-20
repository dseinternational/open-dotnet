// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

// TODO: is this helpfull for row enumeration?

public readonly struct VectorValue : IEquatable<VectorValue>
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

    public static VectorValue FromFloat(float value)
    {
        return new((ulong)BitConverter.SingleToInt32Bits(value), null, VectorDataType.Float32);
    }

    public static VectorValue FromDouble(double value)
    {
        return new(BitConverter.DoubleToUInt64Bits(value), null, VectorDataType.Float64);
    }

    public static VectorValue FromString(string value)
    {
        return new(0, value, VectorDataType.String);
    }

    public long AsInt64()
    {
        if (_kind != VectorDataType.Int64)
        {
            throw new InvalidOperationException($"Cannot convert to Int64 from {_kind}.");
        }

        return (long)_bits;
    }

    public int AsInt32()
    {
        if (_kind != VectorDataType.Int32)
        {
            throw new InvalidOperationException($"Cannot convert to Int32 from {_kind}.");
        }

        return (int)_bits;
    }

    public float AsFloat32()
    {
        if (_kind != VectorDataType.Float32)
        {
            throw new InvalidOperationException($"Cannot convert to Float32 from {_kind}.");
        }

        return BitConverter.Int32BitsToSingle((int)_bits);
    }

    public double AsFloat64()
    {
        if (_kind != VectorDataType.Float64)
        {
            throw new InvalidOperationException($"Cannot convert to Float64 from {_kind}.");
        }

        return BitConverter.Int64BitsToDouble((long)_bits);
    }

    public string AsString()
    {
        if (_kind != VectorDataType.String)
        {
            throw new InvalidOperationException($"Cannot convert to String from {_kind}.");
        }

        return _ref as string ?? string.Empty;
    }

    public override string ToString()
    {
        return _ref as string
            ?? _bits.ToString(); // todo
    }

    public bool Equals(VectorValue other)
    {
        return _kind == other._kind
            && _bits == other._bits
            && Equals(_ref, other._ref);
    }

    public override bool Equals(object? obj)
    {
        if (obj is VectorValue other)
        {
            return Equals(other);
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
}
