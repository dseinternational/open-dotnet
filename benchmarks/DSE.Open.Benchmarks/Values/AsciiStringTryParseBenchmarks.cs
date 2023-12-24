// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[DisassemblyDiagnoser(printSource: true)]
public class AsciiStringTryParseBenchmarks
{
    private static ReadOnlySpan<char> Short => "abc".AsSpan();

    private static ReadOnlySpan<char> Long => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".AsSpan();

    [Benchmark]
    public bool TryParse_Short()
    {
        return AsciiString.TryParse(Short, out _);
    }

    [Benchmark]
    public bool TryParse_Long()
    {
        return AsciiString.TryParse(Long, out _);
    }
}
