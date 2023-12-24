// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class AsciiStringToStringLowerBenchmarks
{
    private static readonly AsciiString s_singleValue = AsciiString.Parse("A", CultureInfo.InvariantCulture);

    private static readonly AsciiString s_eightValue = AsciiString.Parse("ABCDEFGH", CultureInfo.InvariantCulture);

    private static readonly AsciiString s_longValue = AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZ", CultureInfo.InvariantCulture);

    private static readonly AsciiString s_longerValue = AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", CultureInfo.InvariantCulture);

    private static readonly AsciiString s_veryLongValue =
        AsciiString.Parse(
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
            CultureInfo.InvariantCulture);

    [Benchmark]
    public string ToStringLower_Single()
    {
        return s_singleValue.ToStringLower();
    }

    [Benchmark]
    public string ToStringLower_Eight()
    {
        return s_eightValue.ToStringLower();
    }

    [Benchmark]
    public string ToStringLower_Long()
    {
        return s_longValue.ToStringLower();
    }

    [Benchmark]
    public string ToStringLower_Longer()
    {
        return s_longerValue.ToStringLower();
    }

    [Benchmark]
    public string ToStringLower_VeryLong()
    {
        return s_veryLongValue.ToStringLower();
    }
}
