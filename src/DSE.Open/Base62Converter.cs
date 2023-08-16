// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
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
    private static int GetIndex(char c)
    {
        switch (c)
        {
            case >= '0' and <= '9':
                return c - 48;
            case >= 'A' and <= 'Z':
                return c - 55;
            case >= 'a' and <= 'z':
                return c - 61;
            default:
                ThrowHelper.ThrowFormatException("Invalid character in base 62 encoding: " + c);

                return -1;
        }
    }

    private static bool TryGetIndex(char c, out int value)
    {
        switch (c)
        {
            case >= '0' and <= '9':
                value = c - 48;
                return true;
            case >= 'A' and <= 'Z':
                value = c - 55;
                return true;
            case >= 'a' and <= 'z':
                value = c - 61;
                return true;
            default:
                value = -1;
                return false;
        }
    }

    public static string ToBase62String(ReadOnlySpan<byte> data)
    {
        var arr = Array.ConvertAll(data.ToArray(), t => (int)t);

        var converted = Convert(arr, 256, 62);

        var builder = new StringBuilder(converted.Length);

        foreach (var t in converted)
        {
            _ = builder.Append(CharacterSet[t]);
        }

        return builder.ToString();
    }

    public static byte[] FromBase62(string base62Encoded)
    {
        Guard.IsNotNull(base62Encoded);

        var values = new int[base62Encoded.Length];

        for (var i = 0; i < base62Encoded.Length; i++)
        {
            values[i] = GetIndex(base62Encoded[i]);
        }

        var converted = Convert(values, 62, 256);

        return Array.ConvertAll(converted, System.Convert.ToByte);
    }

    public static bool TryFromBase62(string base62Encoded, [NotNullWhen(true)] out byte[]? data)
    {
        if (string.IsNullOrWhiteSpace(base62Encoded))
        {
            return Failed(out data);
        }

        var values = new int[base62Encoded.Length];

        for (var i = 0; i < base62Encoded.Length; i++)
        {
            if (!TryGetIndex(base62Encoded[i], out var value))
            {
                return Failed(out data);
            }

            values[i] = value;
        }

        var converted = Convert(values, 62, 256);

        data = Array.ConvertAll(converted, System.Convert.ToByte);

        return true;

        static bool Failed(out byte[]? d)
        {
            d = null;
            return false;
        }
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
}
