// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class AsciiStringToStringLowerBenchmarks
{
    private static readonly AsciiString s_singleValue = AsciiString.Parse("A");

    private static readonly AsciiString s_eightValue = AsciiString.Parse("ABCDEFGH");

    private static readonly AsciiString s_longValue = AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

    private static readonly AsciiString s_longerValue = AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

    private static readonly AsciiString s_veryLongValue =
        AsciiString.Parse(
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

    [Benchmark]
    public string ToStringLower_Single() => s_singleValue.ToStringLower();

    [Benchmark]
    public string ToStringLower_Eight() => s_eightValue.ToStringLower();

    [Benchmark]
    public string ToStringLower_Long() => s_longValue.ToStringLower();

    [Benchmark]
    public string ToStringLower_Longer() => s_longerValue.ToStringLower();

    [Benchmark]
    public string ToStringLower_VeryLong() => s_veryLongValue.ToStringLower();
}
