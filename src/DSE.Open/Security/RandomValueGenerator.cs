// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;

namespace DSE.Open.Security;

public static class RandomValueGenerator
{
    private const string DefaultStringValueCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";

    public static string GetStringValue(int? length = null) => GetStringValue(length ?? 16, DefaultStringValueCharacters);

    public static string GetStringValue(int length, string validCharacters)
    {
        Guard.IsGreaterThan(length, 0);
        Guard.IsNotNullOrWhiteSpace(validCharacters);
        Guard.IsGreaterThan(validCharacters.Length, 8);

        var data = RandomNumberGenerator.GetBytes(length * 2);
        var sb = new StringBuilder(length);
        for (var i = 0; i < data.Length; i += 2)
        {
            var f = BitConverter.ToUInt16(data, i);
            var r = (double)f / ushort.MaxValue;
            var c = (int)(r * (validCharacters.Length - 1));
            _ = sb.Append(validCharacters[c]);
        }

        return sb.ToString();
    }

    public static int GetInt32Value() => RandomNumberGenerator.GetInt32(int.MaxValue);

    public static int GetInt32Value(int minimum, int maximum)
    {
        return minimum >= maximum
            ? throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be smaller than maximum.")
            : RandomNumberGenerator.GetInt32(minimum, maximum);
    }

    public static int GetPositiveInt32Value() => Math.Abs(GetInt32Value());

    public static long GetInt64Value()
    {
        var data = RandomNumberGenerator.GetBytes(8);
        return BitConverter.ToInt64(data, 0);
    }

    public static long GetPositiveInt64Value() => Math.Abs(GetInt64Value());
}
