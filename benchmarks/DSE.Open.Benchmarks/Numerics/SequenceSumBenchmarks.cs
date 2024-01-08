// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;
using BenchmarkDotNet.Attributes;
using DSE.Open.Numerics;

namespace DSE.Open.Benchmarks.Numerics;

[MemoryDiagnoser]
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
[SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
public abstract class SequenceSumBenchmarks<T>
    where T : struct, INumber<T>
{
    [Params(10, 50, 500)]
    public int Count { get; set; }

    public T[] ValuesArray { get; protected set; } = null!;

    public Collection<T> ValuesCollection { get; protected set; } = null!;

    public IEnumerable<T> ValuesEnumerable{ get; protected set; } = null!;


    [GlobalSetup]
    public void Setup()
    {
        ValuesArray = GetValuesArray(Count);
        ValuesCollection = new Collection<T>(GetValuesList(Count));
        ValuesEnumerable = GetValuesEnumerable(Count);
    }

    [Benchmark]
    public T TensorPrimitives_Sum_Array()
    {
        return TensorPrimitives.Sum<T>(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Array()
    {
        return Sequence.Sum(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Collection()
    {
        return Sequence.Sum(ValuesCollection);
    }

    [Benchmark]
    public T Sequence_Sum_Enumerable()
    {
        return Sequence.Sum(ValuesEnumerable);
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

#pragma warning disable CA1002 // Do not expose generic lists
    protected List<T> GetValuesList(int count)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        var i = T.AdditiveIdentity;
        return Enumerable.Range(1, count).Select(n => i + T.One).ToList();
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
}


public class SequenceSumBenchmarksInt32 : SequenceSumBenchmarks<int>
{
    [Benchmark(Baseline = true)]
    public int Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}

public class SequenceSumBenchmarksInt64 : SequenceSumBenchmarks<long>
{
    [Benchmark(Baseline = true)]
    public long Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }

    public long Linq_Sum_Collection()
    {
        return ValuesCollection.Sum();
    }
}

public class SequenceSumBenchmarksSingle : SequenceSumBenchmarks<float>
{
    [Benchmark(Baseline = true)]
    public float Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}

public class SequenceSumBenchmarksDouble : SequenceSumBenchmarks<double>
{
    [Benchmark(Baseline = true)]
    public double Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}
