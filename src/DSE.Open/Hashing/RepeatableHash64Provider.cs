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

    public ulong GetRepeatableHashCode(bool value)
    {
        return GetRepeatableHashCode(value ? 1 : 0);
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
        return GetRepeatableHashCode(value.Span);
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
    private ulong GetRepeatableHashCodeSpan<T>(ReadOnlySpan<T> value, bool reverseEndianness) where T : unmanaged
    {
        if (!reverseEndianness)
        {
            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(value));
        }

        T[]? rented = null;

        Span<T> reversed = MemoryThresholds.CanStackalloc<T>(value.Length)
            ? (rented = ArrayPool<T>.Shared.Rent(value.Length))
            : stackalloc T[MemoryThresholds.StackallocByteThreshold / Unsafe.SizeOf<T>()];

        try
        {
            if (typeof(T) == typeof(short))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, short>(value), MemoryMarshal.Cast<T, short>(reversed));
            }
            else if (typeof(T) == typeof(ushort) || typeof(T) == typeof(char))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, ushort>(value), MemoryMarshal.Cast<T, ushort>(reversed));
            }
            else if (typeof(T) == typeof(int))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, int>(value), MemoryMarshal.Cast<T, int>(reversed));
            }
            else if (typeof(T) == typeof(uint))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, uint>(value), MemoryMarshal.Cast<T, uint>(reversed));
            }
            else if (typeof(T) == typeof(long))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, long>(value), MemoryMarshal.Cast<T, long>(reversed));
            }
            else if (typeof(T) == typeof(ulong))
            {
                BinaryPrimitives.ReverseEndianness(MemoryMarshal.Cast<T, ulong>(value), MemoryMarshal.Cast<T, ulong>(reversed));
            }
            else
            {
                throw new NotSupportedException($"Type '{typeof(T)}' is not supported.");
            }

            return GetRepeatableHashCodeCore(MemoryMarshal.AsBytes(reversed[..value.Length]));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<T>.Shared.Return(rented);
            }
        }
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<char> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<short> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<short> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<int> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<int> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<uint> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<uint> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<long> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<long> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    public ulong GetRepeatableHashCode(ReadOnlySpan<AsciiChar> value)
    {
        return GetRepeatableHashCode(ValuesMarshal.AsBytes(value));
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

    public ulong Combine(IRepeatableHash64 obj0, IRepeatableHash64 obj1)
    {
        ArgumentNullException.ThrowIfNull(obj0);
        ArgumentNullException.ThrowIfNull(obj1);
        return CombineHashCodes(obj0.GetRepeatableHashCode(), obj1.GetRepeatableHashCode());
    }

    public ulong Combine(IRepeatableHash64 obj0, IRepeatableHash64 obj1, IRepeatableHash64 obj2)
    {
        ArgumentNullException.ThrowIfNull(obj0);
        ArgumentNullException.ThrowIfNull(obj1);
        ArgumentNullException.ThrowIfNull(obj2);
        return CombineHashCodes(
            obj0.GetRepeatableHashCode(),
            obj1.GetRepeatableHashCode(),
            obj2.GetRepeatableHashCode());
    }

    public ulong Combine(IRepeatableHash64 obj0, IRepeatableHash64 obj1, IRepeatableHash64 obj2, IRepeatableHash64 obj3)
    {
        ArgumentNullException.ThrowIfNull(obj0);
        ArgumentNullException.ThrowIfNull(obj1);
        ArgumentNullException.ThrowIfNull(obj2);
        ArgumentNullException.ThrowIfNull(obj3);
        return CombineHashCodes(
            obj0.GetRepeatableHashCode(),
            obj1.GetRepeatableHashCode(),
            obj2.GetRepeatableHashCode(),
            obj3.GetRepeatableHashCode());
    }

#pragma warning disable CA1822 // Mark members as static

    public ulong CombineHashCodes(ulong h0, ulong h1)
    {
        return h0 ^ h1;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2)
    {
        return h0 ^ h1 ^ h2;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3)
    {
        return h0 ^ h1 ^ h2 ^ h3;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6, ulong h7)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6 ^ h7;
    }

    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6, ulong h7, ulong h8)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6 ^ h7 ^ h8;
    }

    public ulong CombineHashCodes(ReadOnlySpan<ulong> hashes)
    {
        ulong hash = 0;

        foreach (var h in hashes)
        {
            hash ^= h;
        }

        return hash;
    }

#pragma warning restore CA1822 // Mark members as static

    public bool TryGetRepeatableHashCode<T>(T value, out ulong hash)
    {
        if (value is null)
        {
            return Failed(out hash);
        }

        switch (value)
        {
            case IRepeatableHash64:
                hash = ((IRepeatableHash64)value).GetRepeatableHashCode();
                return true;
            case bool boolValue:
                hash = GetRepeatableHashCode(boolValue);
                return true;
            case byte byteValue:
                hash = GetRepeatableHashCode(byteValue);
                return true;
            case short shortValue:
                hash = GetRepeatableHashCode(shortValue);
                return true;
            case ushort ushortValue:
                hash = GetRepeatableHashCode(ushortValue);
                return true;
            case int intValue:
                hash = GetRepeatableHashCode(intValue);
                return true;
            case uint uintValue:
                hash = GetRepeatableHashCode(uintValue);
                return true;
            case long longValue:
                hash = GetRepeatableHashCode(longValue);
                return true;
            case ulong ulongValue:
                hash = GetRepeatableHashCode(ulongValue);
                return true;
            case decimal decimalValue:
                hash = GetRepeatableHashCode(decimalValue);
                return true;
            case float floatValue:
                hash = GetRepeatableHashCode(floatValue);
                return true;
            case double doubleValue:
                hash = GetRepeatableHashCode(doubleValue);
                return true;
            case string stringValue:
                hash = GetRepeatableHashCode(stringValue);
                return true;
            case char[] charArray:
                hash = GetRepeatableHashCode(charArray);
                return true;
            case short[] shortArray:
                hash = GetRepeatableHashCode(shortArray);
                return true;
            case ushort[] ushortArray:
                hash = GetRepeatableHashCode(ushortArray);
                return true;
            case int[] intArray:
                hash = GetRepeatableHashCode(intArray);
                return true;
            case uint[] uintArray:
                hash = GetRepeatableHashCode(uintArray);
                return true;
            case long[] longArray:
                hash = GetRepeatableHashCode(longArray);
                return true;
            case ulong[] ulongArray:
                hash = GetRepeatableHashCode(ulongArray);
                return true;
            case ReadOnlyMemory<char> charMemory:
                hash = GetRepeatableHashCode(charMemory.Span);
                return true;
            case ReadOnlyMemory<short> shortMemory:
                hash = GetRepeatableHashCode(shortMemory.Span);
                return true;
            case ReadOnlyMemory<ushort> ushortMemory:
                hash = GetRepeatableHashCode(ushortMemory.Span);
                return true;
            case ReadOnlyMemory<int> intMemory:
                hash = GetRepeatableHashCode(intMemory.Span);
                return true;
            case ReadOnlyMemory<uint> uintMemory:
                hash = GetRepeatableHashCode(uintMemory.Span);
                return true;
            case ReadOnlyMemory<long> longMemory:
                hash = GetRepeatableHashCode(longMemory.Span);
                return true;
            case ReadOnlyMemory<ulong> ulongMemory:
                hash = GetRepeatableHashCode(ulongMemory.Span);
                return true;
            default:
                return Failed(out hash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool Failed(out ulong hash)
        {
            hash = 0;
            return false;
        }
    }
}
