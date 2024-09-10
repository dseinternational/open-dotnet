// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

public class SequenceSumIntBenchmarks : SequenceSumIntegerBenchmarksBase<int>
{
    [Benchmark(Baseline = true)]
    public int Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}
