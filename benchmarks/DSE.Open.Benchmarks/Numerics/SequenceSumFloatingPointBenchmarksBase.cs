// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

[MemoryDiagnoser]
public abstract class SequenceSumFloatingPointBenchmarksBase<T> : SequenceSumBenchmarksBase<T>
    where T : struct, IFloatingPointIeee754<T>
{
    [Benchmark]
    public T TensorPrimitives_Sum_Array()
    {
        return TensorPrimitives.Sum<T>(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Array()
    {
        return Open.Numerics.Vector.SumFloatingPoint(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Collection()
    {
        return Open.Numerics.Vector.SumFloatingPoint(ValuesCollection);
    }

    [Benchmark]
    public T Sequence_Sum_Enumerable()
    {
        return Open.Numerics.Vector.SumFloatingPoint(ValuesEnumerable);
    }
}
