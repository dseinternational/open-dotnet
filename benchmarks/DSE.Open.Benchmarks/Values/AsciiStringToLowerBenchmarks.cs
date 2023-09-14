// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class AsciiStringToLowerBenchmarks
{
    private static readonly AsciiString s_value = AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

    [Benchmark(Baseline = true)]
    public AsciiString ToLower_Main()
    {
        var result = new AsciiChar[s_value.Length];

        for (var i = 0; i < s_value.Length; i++)
        {
            result[i] = s_value.Span[i].ToLower();
        }

        return new AsciiString(result);
    }

    [Benchmark]
    public AsciiString ToLower_WithSpan() => s_value.ToLower();
}
