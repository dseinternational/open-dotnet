// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.MemoryExtensions.Chars;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class ContainsOnlyAsciiDigitsCharBenchmarks
{
    private const string Easy = "a";

    private const string Medium = "1234567a";

    private const string Hard = "123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789a";

    public static IEnumerable<object> Params()
    {
        yield return Easy;
        yield return Medium;
        yield return Hard;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiDigits_Original(string str)
    {
        var value = str.AsSpan();

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiDigits(string str)
    {
        return str.AsSpan().ContainsOnlyAsciiDigits();
    }
}
