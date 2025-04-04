// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class ObservationHashValueBenchmarks
{
    private static readonly AsciiString s_value =
        AsciiString.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", CultureInfo.InvariantCulture);

    [Benchmark(Baseline = true)]
    public AsciiString ToLower_Main()
    {
        var result = new AsciiChar[s_value.Length];

        for (var i = 0; i < s_value.Length; i++)
        {
            result[i] = s_value.AsSpan()[i].ToLower();
        }

        return new(result);
    }

    [Benchmark]
    public AsciiString ToLower_WithSpan()
    {
        return s_value.ToLower();
    }
}
