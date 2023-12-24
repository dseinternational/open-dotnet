// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
public class BinaryValueToHexLowerBenchmarks
{
    private static readonly BinaryValue s_value = BinaryValue.FromEncodedString("Hello, World!");
    private const string Format = "x";

    [Benchmark]
    public string ToHexLowerString()
    {
        return s_value.ToString(BinaryStringEncoding.HexLower);
    }

    [Benchmark]
    public bool TryFormatHexLower_Chars()
    {
        Span<char> buffer = stackalloc char[26];
        return s_value.TryFormat(buffer, out _, Format, provider: default);
    }

    [Benchmark]
    public bool TryFormatHexLower_Bytes()
    {
        Span<byte> buffer = stackalloc byte[26];
        return s_value.TryFormat(buffer, out _, Format, provider: default);
    }
}
