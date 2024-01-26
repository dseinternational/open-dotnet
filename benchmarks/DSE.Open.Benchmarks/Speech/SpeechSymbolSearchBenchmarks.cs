// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using DSE.Open.Speech;

namespace DSE.Open.Benchmarks.Speech;

#pragma warning disable CA1822 // Mark members as static

public class SpeechSymbolSearchBenchmarks
{
    private static readonly ImmutableArray<int> s_sortedArray = SpeechSymbol.StrictIpaSet.Order().ToImmutableArray();

    public IEnumerable<char> Values
    {
        get
        {
            yield return SpeechSymbol.RaisedOpenFrontUnrounded;
            yield return SpeechSymbol.MajorGroupBreakIntonation;
        }
    }

    [ParamsSource(nameof(Values))]
    public char Value { get; set; }

    [Benchmark]
    public bool IsStrictIpaCharFrozenSet()
    {
        return SpeechSymbol.IsStrictIpaSymbol(Value);
    }

    [Benchmark]
    public bool IsStrictIpaCharBinarySearch()
    {
        return s_sortedArray.BinarySearch(Value) >= 0;
    }
}

/*

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22635.3130)
13th Gen Intel Core i9-13900K, 1 CPU, 32 logical and 24 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


| Method                      | Value | Mean      | Error     | StdDev    |
|---------------------------- |------ |----------:|----------:|----------:|
| IsStrictIpaCharFrozenSet    | æ     |  2.409 ns | 0.0076 ns | 0.0068 ns |
| IsStrictIpaCharBinarySearch | æ     | 10.073 ns | 0.0577 ns | 0.0540 ns |
| IsStrictIpaCharFrozenSet    | ?     |  1.393 ns | 0.0073 ns | 0.0069 ns |
| IsStrictIpaCharBinarySearch | ?     |  8.376 ns | 0.0909 ns | 0.0851 ns |

 */
