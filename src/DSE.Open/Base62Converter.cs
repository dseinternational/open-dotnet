// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;

namespace DSE.Open;

public static class Base62Converter
{
    private const string CharacterSet =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    // 0 = 48  :  0
    // 9 = 57  :  9
    // A = 65  : 10
    // Z = 90  : 35
    // a = 97  : 36
    // z = 122 : 61

    /// <summary>
    /// Gets the index of the character in the base 62 character set. Returns -1 if the character is not valid.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private static int GetIndex(char c)
    {
        return c switch
        {
            >= '0' and <= '9' => c - 48,
            >= 'A' and <= 'Z' => c - 55,
            >= 'a' and <= 'z' => c - 61,
            _ => -1
        };
    }

    private static bool TryGetIndex(char c, out int value)
    {
        value = GetIndex(c);
        return value >= 0;
    }

    public static string ToBase62String(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
        {
            return string.Empty;
        }

        var arr = new int[data.Length];

        for (var i = 0; i < data.Length; i++)
        {
            arr[i] = data[i];
        }

        var converted = Convert(arr, 256, 62);

        var builder = new StringBuilder(converted.Length);

        foreach (var t in converted)
        {
            _ = builder.Append(CharacterSet[t]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Converts a base 62 string to a byte array.
    /// </summary>
    /// <param name="base62"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException">Thrown if the string is not a valid base 62 string.</exception>
    public static byte[] FromBase62(string base62)
    {
        ArgumentNullException.ThrowIfNull(base62);

        if (!TryFromBase62(base62, out var data))
        {
            ThrowHelper.ThrowInvalidDataException("Invalid base 62 string");
        }

        return data;
    }


    /// <summary>
    /// Converts base62 encoded chars to a byte array.
    /// </summary>
    /// <param name="base62"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool TryFromBase62Chars(ReadOnlySpan<char> base62, out byte[] data)
    {
        if (base62.IsEmpty)
        {
            data = [];
            return true;
        }

        var values = new int[base62.Length];

        for (var i = 0; i < base62.Length; i++)
        {
            if (!TryGetIndex(base62[i], out var value))
            {
                return Failed(out data);
            }

            values[i] = value;
        }

        var converted = Convert(values, 62, 256);

        data = Array.ConvertAll(converted, System.Convert.ToByte);

        return true;

        static bool Failed(out byte[] d)
        {
            d = [];
            return false;
        }
    }

    public static bool TryFromBase62(string base62, out byte[] data)
    {
        ArgumentNullException.ThrowIfNull(base62);
        return TryFromBase62Chars(base62.AsSpan(), out data);
    }

    private static int[] Convert(int[] source, int sourceBase, int targetBase)
    {
        var result = new List<int>(source.Length);

        var leadingZeroCount = Math.Min(source.TakeWhile(x => x == 0).Count(), source.Length - 1);

        int count;

        var quotient = new List<int>(16);

        while ((count = source.Length) > 0)
        {
            var remainder = 0;

            for (var i = 0; i != count; i++)
            {
                var accumulator = source[i] + (remainder * sourceBase);
                var digit = accumulator / targetBase;
                remainder = accumulator % targetBase;

                if (quotient.Count > 0 || digit > 0)
                {
                    quotient.Add(digit);
                }
            }

            result.Insert(0, remainder);

            source = quotient.ToArray();
            quotient.Clear();
        }

        result.InsertRange(0, Enumerable.Repeat(0, leadingZeroCount));

        return result.ToArray();
    }

    public static bool TryEncodeToBase62(ReadOnlySpan<byte> bytes, Span<char> destination, out int charsWritten)
    {
        if (bytes.IsEmpty)
        {
            charsWritten = 0;
            return true;
        }

        var arr = new int[bytes.Length];

        for (var i = 0; i < bytes.Length; i++)
        {
            arr[i] = bytes[i];
        }

        var converted = Convert(arr, 256, 62);

        if (converted.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        for (var i = 0; i < converted.Length; i++)
        {
            destination[i] = CharacterSet[converted[i]];
        }

        charsWritten = converted.Length;
        return true;
    }
}
