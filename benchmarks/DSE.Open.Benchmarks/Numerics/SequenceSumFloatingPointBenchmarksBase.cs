// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using BenchmarkDotNet.Attributes;
using DSE.Open.Numerics;

namespace DSE.Open.Benchmarks.Numerics;

[MemoryDiagnoser]
public abstract class SequenceSumFloatingPointBenchmarksBase<T> : SequenceSumBenchmarksBase<T>
    where T : struct, IFloatingPointIeee754<T>
{
    [Benchmark]
    public T TensorPrimitives_Sum_Array()
    {
        return TensorPrimitives.Sum(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Array()
    {
        return Sequence.SumFloatingPoint(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Collection()
    {
        return Sequence.SumFloatingPoint(ValuesCollection);
    }

    [Benchmark]
    public T Sequence_Sum_Enumerable()
    {
        return Sequence.SumFloatingPoint(ValuesEnumerable);
    }
}
