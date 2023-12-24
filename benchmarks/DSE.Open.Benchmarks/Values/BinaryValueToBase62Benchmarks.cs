// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

public class BinaryValueToBase62Benchmarks
{
    private static readonly BinaryValue s_value = BinaryValue.FromEncodedString("Hello, World!");
    private const string Format = "b";

    [Benchmark]
    public string ToBase62String()
    {
        return s_value.ToBase64EncodedString();
    }

    [Benchmark]
    public bool TryFormatBase62_Chars()
    {
        Span<char> buffer = stackalloc char[16];
        return s_value.TryFormat(buffer, out _, Format, provider: default);
    }

    [Benchmark]
    public bool TryFormatBase62_Bytes()
    {
        Span<byte> buffer = stackalloc byte[16];
        return s_value.TryFormat(buffer, out _, Format, provider: default);
    }
}
