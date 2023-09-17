using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Benchmarks.Collections;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(false)]
public class ReadOnlyValueCollectionCreateBenchmarks
{
    public const int N = 100;

    private static readonly List<int> s_items = new(N);

    [GlobalSetup]
    public void Setup()
    {
        for (var i = 0; i < N; i++)
        {
            s_items.Add(i);
        }
    }

    [Benchmark(Baseline = true)]
    public ReadOnlyValueCollection<int> Create_IEnumerable()
    {
        return ReadOnlyValueCollection.CreateRange(s_items);
    }

    [Benchmark]
    public ReadOnlyValueCollection<int> Create_Span()
    {
        var span = CollectionsMarshal.AsSpan(s_items);
        return ReadOnlyValueCollection.Create(span);
    }

    [Benchmark]
    public ReadOnlyValueCollection<int> Create_Unsafe()
    {
        return ReadOnlyValueCollection.CreateUnsafe(s_items);
    }
}
