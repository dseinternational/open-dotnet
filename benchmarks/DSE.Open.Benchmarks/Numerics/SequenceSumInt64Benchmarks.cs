// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

public class SequenceSumInt64Benchmarks : SequenceSumIntegerBenchmarksBase<long>
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
