// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using DSE.Open.Text;

namespace DSE.Open.Benchmarks.Text;

[MemoryDiagnoser]
[SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<Pending>")]
public class StringHelperJoinStringBenchmarks
{
    [Params(8, 64, 128)]
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
    public string Join_List()
    {
        return StringHelper.Join(", ", " and ", GetValuesList(ValuesCount));
    }

    [Benchmark]
    public string StringJoin_List()
    {
        return string.Join(", ", GetValuesList(ValuesCount));
    }

    [Benchmark]
    public string Join_Collection()
    {
        return StringHelper.Join(", ", " and ", GetValuesCollection(ValuesCount));
    }

    [Benchmark]
    public string Join_Enumerable()
    {
        return StringHelper.Join(", ", " and ", GetValuesEnumerable(ValuesCount));
    }
}
