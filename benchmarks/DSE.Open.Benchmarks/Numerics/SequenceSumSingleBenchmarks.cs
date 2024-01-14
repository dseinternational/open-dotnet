// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

public class SequenceSumSingleBenchmarks : SequenceSumFloatingPointBenchmarksBase<float>
{
    [Benchmark(Baseline = true)]
    public float Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}
