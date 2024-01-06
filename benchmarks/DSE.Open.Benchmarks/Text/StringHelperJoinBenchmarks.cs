// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
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

    [ParamsSource(nameof(GetValues))]
    public IEnumerable<string> Values { get; set; } = [];

    public IEnumerable<IEnumerable<string>> GetValues()
    {
        yield return GetValuesList(ValuesCount);
        yield return GetValuesCollection(ValuesCount);
        yield return GetValuesEnumerable(ValuesCount);
    }

    private static List<string> GetValuesList(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => WordLists.EnglishEarlyWords[Random.Shared.Next(WordLists.EnglishEarlyWords.Count - 1)])
            .ToList();
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

    [Benchmark]
    public string StringConcatenatorJoin()
    {
        return StringConcatenator.Join(", ", Values, " and ");
    }

    [Benchmark(Baseline = true)]
    public string StringHelperJoin()
    {
        return StringHelper.Join(", ", Values, " and ");
    }

    [Benchmark]
    public string StringHelperJoin2()
    {
        return StringHelper.Join2(", ", Values, " and ");
    }
}
