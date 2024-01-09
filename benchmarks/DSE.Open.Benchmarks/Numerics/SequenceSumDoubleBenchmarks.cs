// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Numerics;

public class SequenceSumDoubleBenchmarks : SequenceSumFloatingPointBenchmarksBase<double>
{
    [Benchmark(Baseline = true)]
    public double Linq_Sum_Array()
    {
        return ValuesArray.Sum();
    }
}
