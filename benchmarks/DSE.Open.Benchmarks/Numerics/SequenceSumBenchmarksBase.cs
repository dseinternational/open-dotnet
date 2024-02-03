// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

[MemoryDiagnoser]
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
[SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
public abstract class SequenceSumBenchmarksBase<T>
    where T : struct, INumber<T>
{
    [Params(10, 50, 500)]
    public int Count { get; set; }

    public T[] ValuesArray { get; protected set; } = null!;

    public Collection<T> ValuesCollection { get; protected set; } = null!;

    public IEnumerable<T> ValuesEnumerable { get; protected set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        ValuesArray = GetValuesArray(Count);
        ValuesCollection = new Collection<T>(GetValuesList(Count));
        ValuesEnumerable = GetValuesEnumerable(Count);
    }
    protected T[] GetValuesArray(int count)
    {
        var result = new T[count];

        var v = T.AdditiveIdentity;

        for (var i = 0; i < count; i++)
        {
            result[i] = v += T.One;
        }

        return result;
    }

    protected IEnumerable<T> GetValuesCollection(int count)
    {
        return new Collection<T>(GetValuesList(count));
    }

    protected IEnumerable<T> GetValuesEnumerable(int count)
    {
        var list = GetValuesList(count);

        foreach (var item in list)
        {
            yield return item;
        }
    }

#pragma warning disable CA1002 // Do not expose generic lists
    protected List<T> GetValuesList(int count)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        var i = T.AdditiveIdentity;
        return Enumerable.Range(1, count).Select(n => i + T.One).ToList();
    }
}
