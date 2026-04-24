// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Benchmarks.Collections;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(false)]
public class ReadOnlyValueCollectionCreateBenchmarks
{
    public const int N = 100;

#pragma warning disable IDE0028 // Simplify collection initialization

    private static readonly List<int> s_items = new(N);

#pragma warning restore IDE0028 // Simplify collection initialization

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
#pragma warning disable IDE0303 // Simplify collection initialization
        return ReadOnlyValueCollection.CreateRange(s_items);
#pragma warning restore IDE0303 // Simplify collection initialization

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
