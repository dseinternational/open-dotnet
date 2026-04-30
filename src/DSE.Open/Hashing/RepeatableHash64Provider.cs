// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Runtime.InteropServices;

namespace DSE.Open.Hashing;

/// <summary>
/// Computes 64-bit hash codes that are stable across processes and platforms. Multi-byte values
/// are normalised to little-endian before hashing so that results are independent of host endianness.
/// </summary>
public abstract class RepeatableHash64Provider
{
    /// <summary>
    /// The default <see cref="RepeatableHash64Provider"/> instance, backed by XxHash3.
    /// </summary>
    public static readonly RepeatableHash64Provider Default = new XxHash3RepeatableHash64Provider();

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified byte value.
    /// </summary>
    public ulong GetRepeatableHashCode(byte value)
    {
        return GetRepeatableHashCodeCore([value]);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified boolean value.
    /// </summary>
    public ulong GetRepeatableHashCode(bool value)
    {
        return GetRepeatableHashCode(value ? 1 : 0);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified character.
    /// </summary>
    public ulong GetRepeatableHashCode(char value)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        BinaryPrimitives.WriteUInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 16-bit signed integer.
    /// </summary>
    public ulong GetRepeatableHashCode(short value)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 16-bit unsigned integer.
    /// </summary>
    public ulong GetRepeatableHashCode(ushort value)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 32-bit signed integer.
    /// </summary>
    public ulong GetRepeatableHashCode(int value)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 32-bit unsigned integer.
    /// </summary>
    public ulong GetRepeatableHashCode(uint value)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 64-bit signed integer.
    /// </summary>
    public ulong GetRepeatableHashCode(long value)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified 64-bit unsigned integer.
    /// </summary>
    public ulong GetRepeatableHashCode(ulong value)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash from the tick count of the specified <see cref="DateTime"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(DateTime value)
    {
        return GetRepeatableHashCode(value.Ticks);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash from the tick count of the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(DateTimeOffset value)
    {
        return GetRepeatableHashCode(value.Ticks);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash from the binary representation of the specified decimal.
    /// </summary>
    public ulong GetRepeatableHashCode(decimal value)
    {
        Span<int> bits = stackalloc int[4];
        _ = decimal.GetBits(value, bits);
        return GetRepeatableHashCode(bits);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified single-precision floating-point value.
    /// </summary>
    public ulong GetRepeatableHashCode(float value)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified double-precision floating-point value.
    /// </summary>
    public ulong GetRepeatableHashCode(double value)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(span, value);
        return GetRepeatableHashCode(span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified <see cref="Timestamp"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(Timestamp value)
    {
        return GetRepeatableHashCodeCore(value.AsSpan());
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified string.
    /// </summary>
    public ulong GetRepeatableHashCode(string value)
    {
        return GetRepeatableHashCode(value.AsSpan());
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified <see cref="CharSequence"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(CharSequence value)
    {
        return GetRepeatableHashCode(value.Span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified <see cref="AsciiString"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(AsciiString value)
    {
        return GetRepeatableHashCode(value.AsSpan());
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified <see cref="Utf8String"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(Utf8String value)
    {
        return GetRepeatableHashCode(value.Span);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of characters.
    /// </summary>
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
            ? stackalloc T[MemoryThresholds.StackallocByteThreshold / Unsafe.SizeOf<T>()]
            : (rented = ArrayPool<T>.Shared.Rent(value.Length));

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

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 16-bit signed integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<short> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<short> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 16-bit unsigned integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<ushort> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 32-bit signed integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<int> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<int> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 32-bit unsigned integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<uint> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<uint> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 64-bit signed integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<long> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<long> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of 64-bit unsigned integers.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value)
    {
        return GetRepeatableHashCode(value, !BitConverter.IsLittleEndian);
    }

    internal ulong GetRepeatableHashCode(ReadOnlySpan<ulong> value, bool reverseEndianness)
    {
        return GetRepeatableHashCodeSpan(value, reverseEndianness);
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of <see cref="AsciiChar"/> values.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<AsciiChar> value)
    {
        return GetRepeatableHashCode(ValuesMarshal.AsBytes(value));
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified <see cref="BinaryValue"/>.
    /// </summary>
    public ulong GetRepeatableHashCode(BinaryValue value)
    {
        return GetRepeatableHashCodeCore(value.AsSpan());
    }

    /// <summary>
    /// Computes a repeatable 64-bit hash for the specified span of bytes.
    /// </summary>
    public ulong GetRepeatableHashCode(ReadOnlySpan<byte> value)
    {
        return GetRepeatableHashCodeCore(value);
    }

    /// <summary>
    /// When overridden in a derived class, computes the underlying 64-bit hash for the supplied bytes.
    /// </summary>
    protected abstract ulong GetRepeatableHashCodeCore(ReadOnlySpan<byte> value);

    /// <summary>
    /// Combines the repeatable hash codes of two objects.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
    public ulong Combine(IRepeatableHash64 obj0, IRepeatableHash64 obj1)
    {
        ArgumentNullException.ThrowIfNull(obj0);
        ArgumentNullException.ThrowIfNull(obj1);
        return CombineHashCodes(obj0.GetRepeatableHashCode(), obj1.GetRepeatableHashCode());
    }

    /// <summary>
    /// Combines the repeatable hash codes of three objects.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
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

    /// <summary>
    /// Combines the repeatable hash codes of four objects.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
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

    /// <summary>
    /// Combines two 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1)
    {
        return h0 ^ h1;
    }

    /// <summary>
    /// Combines three 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2)
    {
        return h0 ^ h1 ^ h2;
    }

    /// <summary>
    /// Combines four 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3)
    {
        return h0 ^ h1 ^ h2 ^ h3;
    }

    /// <summary>
    /// Combines five 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4;
    }

    /// <summary>
    /// Combines six 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5;
    }

    /// <summary>
    /// Combines seven 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6;
    }

    /// <summary>
    /// Combines eight 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6, ulong h7)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6 ^ h7;
    }

    /// <summary>
    /// Combines nine 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
    public ulong CombineHashCodes(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6, ulong h7, ulong h8)
    {
        return h0 ^ h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6 ^ h7 ^ h8;
    }

    /// <summary>
    /// Combines an arbitrary number of 64-bit hash codes into a single repeatable 64-bit hash code.
    /// </summary>
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

    /// <summary>
    /// Attempts to compute a repeatable 64-bit hash for <paramref name="value"/> by dispatching on
    /// its runtime type. Returns <see langword="false"/> when the runtime type is not supported.
    /// </summary>
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
