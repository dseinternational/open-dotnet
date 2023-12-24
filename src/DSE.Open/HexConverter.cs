// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DSE.Open;

// Uses some of the internals from
// https://github.com/dotnet/runtime/blob/cfbd1816a02391e5dde739f9f353bd3fbda79cff/src/libraries/Common/src/System/HexConverter.cs#L17

internal static class HexConverter
{
    internal enum Casing : uint
    {
        // Output [ '0' .. '9' ] and [ 'A' .. 'F' ].
        Upper = 0,

        // Output [ '0' .. '9' ] and [ 'a' .. 'f' ].
        // This works because values in the range [ 0x30 .. 0x39 ] ([ '0' .. '9' ])
        // already have the 0x20 bit set, so ORing them with 0x20 is a no-op,
        // while outputs in the range [ 0x41 .. 0x46 ] ([ 'A' .. 'F' ])
        // don't have the 0x20 bit set, so ORing them maps to
        // [ 0x61 .. 0x66 ] ([ 'a' .. 'f' ]), which is what we want.
        Lower = 0x2020U,
    }

    public static bool TryEncodeToHexLower(ReadOnlySpan<byte> data, Span<byte> utf8Destination, out int bytesWritten)
    {
        return TryEncodeToHex(data, utf8Destination, out bytesWritten, Casing.Lower);
    }

    public static bool TryEncodeToHexUpper(ReadOnlySpan<byte> data, Span<byte> utf8Destination, out int bytesWritten)
    {
        return TryEncodeToHex(data, utf8Destination, out bytesWritten, Casing.Upper);
    }

    private static bool TryEncodeToHex(ReadOnlySpan<byte> data, Span<byte> utf8Destination, out int bytesWritten, Casing casing)
    {
        for (var i = 0; i < data.Length; i++)
        {
            ToBytesBuffer(data[i], utf8Destination, i * 2, casing);
        }

        bytesWritten = data.Length * 2;
        return true;
    }

    public static bool TryEncodeToHexLower(ReadOnlySpan<byte> data, Span<char> destination, out int charsWritten)
    {
        return TryEncodeToHex(data, destination, out charsWritten, Casing.Lower);
    }

    public static bool TryEncodeToHexUpper(ReadOnlySpan<byte> data, Span<char> destination, out int charsWritten)
    {
        return TryEncodeToHex(data, destination, out charsWritten, Casing.Upper);
    }

    private static bool TryEncodeToHex(ReadOnlySpan<byte> data, Span<char> destination, out int charsWritten, Casing casing)
    {
        Debug.Assert(destination.Length >= data.Length * 2);

        for (var pos = 0; pos < data.Length; pos++)
        {
            ToCharsBuffer(data[pos], destination, pos * 2, casing);
        }

        charsWritten = data.Length * 2;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ToBytesBuffer(byte value, Span<byte> buffer, int startingIndex = 0, Casing casing = Casing.Upper)
    {
        var difference = (((uint)value & 0xF0U) << 4) + ((uint)value & 0x0FU) - 0x8989U;
        var packedResult = ((((uint)(-(int)difference) & 0x7070U) >> 4) + difference + 0xB9B9U) | (uint)casing;

        buffer[startingIndex + 1] = (byte)packedResult;
        buffer[startingIndex] = (byte)(packedResult >> 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ToCharsBuffer(byte value, Span<char> buffer, int startingIndex = 0, Casing casing = Casing.Upper)
    {
        var difference = (((uint)value & 0xF0U) << 4) + ((uint)value & 0x0FU) - 0x8989U;
        var packedResult = ((((uint)(-(int)difference) & 0x7070U) >> 4) + difference + 0xB9B9U) | (uint)casing;

        buffer[startingIndex + 1] = (char)(packedResult & 0xFF);
        buffer[startingIndex] = (char)(packedResult >> 8);
    }

    internal static class SearchValuesCache
    {
        internal static readonly SearchValues<char> s_lowerCaseHexChars = SearchValues.Create("0123456789abcdef");

        internal static readonly SearchValues<char> s_upperCaseHexChars = SearchValues.Create("0123456789ABCDEF");

        internal static readonly SearchValues<byte> s_lowerCaseHexBytes = SearchValues.Create("0123456789abcdef"u8);

        internal static readonly SearchValues<byte> s_upperCaseHexBytes = SearchValues.Create("0123456789ABCDEF"u8);
    }

    internal static bool IsValidLowerHex(ReadOnlySpan<char> source)
    {
        return !source.ContainsAnyExcept(SearchValuesCache.s_lowerCaseHexChars);
    }

    internal static bool IsValidUpperHex(ReadOnlySpan<char> source)
    {
        return !source.ContainsAnyExcept(SearchValuesCache.s_upperCaseHexChars);
    }

    internal static bool IsValidLowerHex(ReadOnlySpan<byte> source)
    {
        return !source.ContainsAnyExcept(SearchValuesCache.s_lowerCaseHexBytes);
    }

    internal static bool IsValidUpperHex(ReadOnlySpan<byte> source)
    {
        return !source.ContainsAnyExcept(SearchValuesCache.s_upperCaseHexBytes);
    }

    public static bool TryConvertFromUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
    {
        if (source.Length % 2 != 0)
        {
            bytesWritten = 0;
            return false;
        }

        var i = 0;
        var j = 0;

        while (i < source.Length)
        {
            var hi = CharToHexLookup[source[i++]];
            var lo = CharToHexLookup[source[i++]];

            if (hi == 0xFF || lo == 0xFF)
            {
                bytesWritten = 0;
                return false;
            }

            destination[j++] = (byte)((hi << 4) | lo);
        }

        bytesWritten = j;
        return true;
    }


    /// <summary>Map from an ASCII char to its hex value, e.g. arr['b'] == 11. 0xFF means it's not a hex digit.</summary>
    public static ReadOnlySpan<byte> CharToHexLookup =>
    [
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 15
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 31
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 47
        0x0,
        0x1,
        0x2,
        0x3,
        0x4,
        0x5,
        0x6,
        0x7,
        0x8,
        0x9,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 63
        0xFF,
        0xA,
        0xB,
        0xC,
        0xD,
        0xE,
        0xF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 79
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 95
        0xFF,
        0xa,
        0xb,
        0xc,
        0xd,
        0xe,
        0xf,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 111
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 127
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 143
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 159
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 175
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 191
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 207
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 223
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 239
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF, // 255
    ];
}
