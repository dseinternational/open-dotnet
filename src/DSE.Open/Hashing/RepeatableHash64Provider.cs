// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Hashing;

public abstract class RepeatableHash64Provider
{
    public static readonly RepeatableHash64Provider Default = new XxHash3RepeatableHash64Provider();

    public ulong GetRepeatableHashCode(byte value)
    {
        return GetRepeatableHashCodeCore([value]);
    }

    public ulong GetRepeatableHashCode(char value)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        BinaryPrimitives.WriteUInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(short value)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(ushort value)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(int value)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(uint value)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(long value)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(ulong value)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(DateTime value)
    {
        return GetRepeatableHashCode(value.Ticks);
    }

    public ulong GetRepeatableHashCode(DateTimeOffset value)
    {
        return GetRepeatableHashCode(value.Ticks);
    }

    public ulong GetRepeatableHashCode(decimal value)
    {
        Span<int> bits = stackalloc int[4];
        _ = decimal.GetBits(value, bits);
        return GetRepeatableHashCode(bits);
    }

    public ulong GetRepeatableHashCode(float value)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(double value)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    public ulong GetRepeatableHashCode(Timestamp value)
    {
        return GetRepeatableHashCodeCore(value.AsSpan());
    }

    public ulong GetRepeatableHashCode(string value)
    {
        return GetRepeatableHashCode(value.AsSpan());
    }

    public ulong GetRepeatableHashCode(CharSequence value)
    {
        return GetRepeatableHashCode(value.AsSpan());
    }

    public ulong GetRepeatableHashCode(AsciiString value)
    {
        return GetRepeatableHashCode(value.AsSpan());
    }

    public ulong GetRepeatableHashCode(Utf8String value)
    {
        return GetRepeatableHashCode(value.Span);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<char> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<char> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        ushort[]? rented = null;

        Span<ushort> reversed = MemoryThresholds.CanStackalloc<ushort>(value.Length)
            ? (rented = ArrayPool<ushort>.Shared.Rent(value.Length))
            : stackalloc ushort[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<char, ushort>(value), reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<ushort>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<short> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<short> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        short[]? rented = null;

        Span<short> reversed = MemoryThresholds.CanStackalloc<short>(value.Length)
            ? (rented = ArrayPool<short>.Shared.Rent(value.Length))
            : stackalloc short[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<short>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        ushort[]? rented = null;

        Span<ushort> reversed = MemoryThresholds.CanStackalloc<ushort>(value.Length)
            ? (rented = ArrayPool<ushort>.Shared.Rent(value.Length))
            : stackalloc ushort[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<ushort>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<int> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<int> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        int[]? rented = null;

        Span<int> reversed = MemoryThresholds.CanStackalloc<int>(value.Length)
            ? (rented = ArrayPool<int>.Shared.Rent(value.Length))
            : stackalloc int[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<int>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<uint> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<uint> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        uint[]? rented = null;

        Span<uint> reversed = MemoryThresholds.CanStackalloc<uint>(value.Length)
            ? (rented = ArrayPool<uint>.Shared.Rent(value.Length))
            : stackalloc uint[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<uint>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<long> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<long> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        long[]? rented = null;

        Span<long> reversed = MemoryThresholds.CanStackalloc<long>(value.Length)
            ? (rented = ArrayPool<long>.Shared.Rent(value.Length))
            : stackalloc long[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<long>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    [SkipLocalsInit]
    internal ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value, bool reverseEndianness)
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        ulong[]? rented = null;

        Span<ulong> reversed = MemoryThresholds.CanStackalloc<ulong>(value.Length)
            ? (rented = ArrayPool<ulong>.Shared.Rent(value.Length))
            : stackalloc ulong[value.Length];

        try
        {
            BinaryPrimitives.ReverseEndianness(value, reversed);
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<ulong>.Shared.Return(rented);
            }
        }
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<AsciiChar> value)
    {
        return GetRepeatableHashCode(MemoryMarshal.Cast<AsciiChar, byte>(value));
    }

    public ulong GetRepeatableHashCode(BinaryValue value)
    {
        return GetRepeatableHashCodeCore(value.AsSpan());
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<byte> value)
    {
        return GetRepeatableHashCodeCore(value);
    }

    protected abstract ulong GetRepeatableHashCodeCore(ReadOnlySpan<byte> value);

#pragma warning disable CA1822 // Mark members as static

    public ulong CombineHashCodes(ulong h0, ulong h1)
    {
        return h0 ^ h1;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2)
    {
        return h0 ^ h1 ^ h2;
    }

#pragma warning restore CA1822 // Mark members as static

    public bool TryGetRepeatableHashCode<T>(T value, out ulong hash)
    {
        if (value is null)
        {
            goto Failed;
        }

        if (value is IRepeatableHash64 implemented)
        {
            hash = implemented.GetRepeatableHashCode();
            return true;
        }

        if (value is byte byteValue)
        {
            hash = GetRepeatableHashCode(byteValue);
            return true;
        }

        if (value is short shortValue)
        {
            hash = GetRepeatableHashCode(shortValue);
            return true;
        }

        if (value is ushort ushortValue)
        {
            hash = GetRepeatableHashCode(ushortValue);
            return true;
        }

        if (value is int intValue)
        {
            hash = GetRepeatableHashCode(intValue);
            return true;
        }

        if (value is uint uintValue)
        {
            hash = GetRepeatableHashCode(uintValue);
            return true;
        }

        if (value is long longValue)
        {
            hash = GetRepeatableHashCode(longValue);
            return true;
        }

        if (value is ulong ulongValue)
        {
            hash = GetRepeatableHashCode(ulongValue);
            return true;
        }

        if (value is decimal decimalValue)
        {
            hash = GetRepeatableHashCode(decimalValue);
            return true;
        }

        if (value is float floatValue)
        {
            hash = GetRepeatableHashCode(floatValue);
            return true;
        }

        if (value is double doubleValue)
        {
            hash = GetRepeatableHashCode(doubleValue);
            return true;
        }

        if (value is string stringValue)
        {
            hash = GetRepeatableHashCode(stringValue);
            return true;
        }

        if (value is char[] charArray)
        {
            hash = GetRepeatableHashCode(charArray);
            return true;
        }

        if (value is short[] shortArray)
        {
            hash = GetRepeatableHashCode(shortArray);
            return true;
        }

        if (value is ushort[] ushortArray)
        {
            hash = GetRepeatableHashCode(ushortArray);
            return true;
        }

        if (value is int[] intArray)
        {
            hash = GetRepeatableHashCode(intArray);
            return true;
        }

        if (value is uint[] uintArray)
        {
            hash = GetRepeatableHashCode(uintArray);
            return true;
        }

        if (value is long[] longArray)
        {
            hash = GetRepeatableHashCode(longArray);
            return true;
        }

        if (value is ulong[] ulongArray)
        {
            hash = GetRepeatableHashCode(ulongArray);
            return true;
        }

        if (value is ReadOnlyMemory<char> charMemory)
        {
            hash = GetRepeatableHashCode(charMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<short> shortMemory)
        {
            hash = GetRepeatableHashCode(shortMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<ushort> ushortMemory)
        {
            hash = GetRepeatableHashCode(ushortMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<int> intMemory)
        {
            hash = GetRepeatableHashCode(intMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<uint> uintMemory)
        {
            hash = GetRepeatableHashCode(uintMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<long> longMemory)
        {
            hash = GetRepeatableHashCode(longMemory.Span);
            return true;
        }

        if (value is ReadOnlyMemory<ulong> ulongMemory)
        {
            hash = GetRepeatableHashCode(ulongMemory.Span);
            return true;
        }

    Failed:
        hash = 0;
        return false;
    }
}
