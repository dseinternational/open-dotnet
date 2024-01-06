// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using DSE.Open.Text;

namespace DSE.Open.Benchmarks.Text;

[MemoryDiagnoser]
[SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<Pending>")]
public class StringHelperJoinBenchmarks
{
    [Params(5, 20, 100)]
    public int ValuesCount { get; set; }

    private static List<string> GetValuesCollection(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => WordLists.EnglishEarlyWords[Random.Shared.Next(WordLists.EnglishEarlyWords.Count - 1)])
            .ToList();
    }

    private static IEnumerable<string> GetValuesEnumerable(int count)
    {
        var list = GetValuesCollection(count);

        foreach (var item in list)
        {
            yield return item;
        }
    }

    [Benchmark(Baseline = true)]
    public string Join_Collection()
    {
        return StringConcatenator.Join(", ", GetValuesCollection(ValuesCount), " and ");
    }

    [Benchmark]
    public string StringHelperJoin_Collection()
    {
        return StringHelper.Join(", ", GetValuesCollection(ValuesCount), " and ");
    }

    [Benchmark]
    public string Join_Enumerable()
    {
        return StringConcatenator.Join(", ", GetValuesEnumerable(ValuesCount), " and ");
    }

    [Benchmark]
    public string StringHelperJoin_Enumerable()
    {
        return StringHelper.Join(", ", GetValuesEnumerable(ValuesCount), " and ");
    }
}
