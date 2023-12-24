// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.MemoryExtensions.Bytes;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class ContainsOnlyAsciiLettersByteBenchmarks
{
    private static readonly byte[] s_easy = "a"u8.ToArray();

    private static readonly byte[] s_medium = "abcdEFG1"u8.ToArray();

    private static readonly byte[] s_hard = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1"u8
        .ToArray();

    public static IEnumerable<object> Params()
    {
        yield return s_easy;
        yield return s_medium;
        yield return s_hard;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiLetters_Original(byte[] arr)
    {
        var value = arr.AsSpan();

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetter(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Params))]
    public bool ContainsOnlyAsciiLetters(byte[] arr)
    {
        return arr.AsSpan().ContainsOnlyAsciiLetters();
    }
}
