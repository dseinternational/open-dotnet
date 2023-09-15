// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
public class BinaryValueToBase64Benchmarks
{
    private static readonly BinaryValue s_value = BinaryValue.FromEncodedString("Hello, World!");

    [Benchmark]
    public string ToBase64String() => s_value.ToBase64EncodedString();

    [Benchmark]
    public bool TryFormatBase64_Chars()
    {
        Span<char> buffer = stackalloc char[16];
        return s_value.TryFormat(buffer, out _, format: default, provider: default);
    }

    [Benchmark]
    public bool TryFormatBase64_Bytes()
    {
        Span<byte> buffer = stackalloc byte[16];
        return s_value.TryFormat(buffer, out _, format: default, provider: default);
    }
}
