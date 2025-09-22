// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Benchmarks.Collections;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(false)]
public class ReadOnlyValueSetCreateBenchmarks
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

    [Benchmark]
    public ReadOnlyValueSet<int> Create_Span()
    {
        var span = CollectionsMarshal.AsSpan(s_items);
        return ReadOnlyValueSet.Create(span);
    }
}
