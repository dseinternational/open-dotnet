// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using DSE.Open.Text;

namespace DSE.Open.Benchmarks.Text;

#pragma warning disable CA5394 // Do not use insecure randomness - doesn't matter for benchmark

[MemoryDiagnoser(false)]
public class StringHelperJoinStringVsSpanBenchmarks
{
    [Params(2, 4, 64, 256)]
    public int ValuesCount { get; set; }

    private static List<string> GetValuesList(int count)
    {
        return [.. Enumerable.Range(0, count).Select(i => WordLists.EnglishEarlyWords[Random.Shared.Next(WordLists.EnglishEarlyWords.Count - 1)])];
    }

    public static IEnumerable<string> GetValuesCollection(int count)
    {
        return new Collection<string>(GetValuesList(count));
    }

    private static IEnumerable<string> GetValuesEnumerable(int count)
    {
        var list = GetValuesList(count);

        foreach (var item in list)
        {
            yield return item;
        }
    }

    [Benchmark(Baseline = true)]
    public string JoinString_List()
    {
        return StringHelper.Join(", ", null, GetValuesList(ValuesCount));
    }

    [Benchmark]
    public string JoinSpan_List()
    {
        return StringHelper.Join((ReadOnlySpan<char>)", ", null, GetValuesList(ValuesCount));
    }

    [Benchmark]
    public string JoinString_Collection()
    {
        return StringHelper.Join(", ", " and ", GetValuesCollection(ValuesCount));
    }

    [Benchmark]
    public string JoinSpan_Collection()
    {
        return StringHelper.Join((ReadOnlySpan<char>)", ", " and ", GetValuesCollection(ValuesCount));
    }

    [Benchmark]
    public string JoinString_Enumerable()
    {
        return StringHelper.Join(", ", " and ", GetValuesEnumerable(ValuesCount));
    }

    [Benchmark]
    public string JoinSpan_Enumerable()
    {
        return StringHelper.Join((ReadOnlySpan<char>)", ", " and ", GetValuesEnumerable(ValuesCount));
    }
}
