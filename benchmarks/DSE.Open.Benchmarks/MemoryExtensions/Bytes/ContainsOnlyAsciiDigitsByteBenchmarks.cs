// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.MemoryExtensions.Bytes;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class ContainsOnlyAsciiDigitsByteBenchmarks
{
    private static readonly byte[] Easy = "a"u8.ToArray();

    private static readonly byte[] Medium = "1234567a"u8.ToArray();

    private static readonly byte[] Hard = "1234567891   23456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789a"u8.ToArray();

    public static IEnumerable<object> Params()
    {
        yield return Easy;
        yield return Medium;
        yield return Hard;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiDigits_Original(byte[] arr)
    {
        var value = arr.AsSpan();

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiDigits(byte[] arr)
        => arr.AsSpan().ContainsOnlyAsciiDigits();
}
