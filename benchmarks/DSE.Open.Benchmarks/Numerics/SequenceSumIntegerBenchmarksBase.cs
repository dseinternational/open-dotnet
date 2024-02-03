// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

[MemoryDiagnoser]
public abstract class SequenceSumIntegerBenchmarksBase<T> : SequenceSumBenchmarksBase<T>
    where T : struct, IBinaryInteger<T>
{
    [Benchmark]
    public T TensorPrimitives_Sum_Array()
    {
        return TensorPrimitives.Sum<T>(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Array()
    {
        return Open.Numerics.Vector.Sum(ValuesArray);
    }

    [Benchmark]
    public T Sequence_Sum_Collection()
    {
        return Open.Numerics.Vector.Sum(ValuesCollection);
    }

    [Benchmark]
    public T Sequence_Sum_Enumerable()
    {
        return Open.Numerics.Vector.Sum(ValuesEnumerable);
    }
}
