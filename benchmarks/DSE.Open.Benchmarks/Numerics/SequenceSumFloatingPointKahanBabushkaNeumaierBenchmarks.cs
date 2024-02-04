// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Numerics;

namespace DSE.Open.Benchmarks.Numerics;

public class SequenceSumFloatingPointKahanBabushkaNeumaierBenchmarks
{
    [Params(10, 50, 500)]
    public int Count { get; set; }

    [Params(SummationCompensation.None, SummationCompensation.KahanBabushkaNeumaier)]
    public SummationCompensation SummationCompensation { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
    public double[] ValuesArray { get; protected set; } = null!;
#pragma warning restore CA1819 // Properties should not return arrays

    public IEnumerable<double> ValuesEnumerable { get; protected set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        ValuesArray = GetValuesArray(Count);
        ValuesEnumerable = GetValuesEnumerable(Count);
    }

    private static double[] GetValuesArray(int count)
    {
        var result = new double[count];

        var v = 0.0;

        for (var i = 0; i < count; i++)
        {
            result[i] = v += 1.0;
        }

        return result;
    }

    private static IEnumerable<double> GetValuesEnumerable(int count)
    {
        var list = GetValuesArray(count);

        foreach (var item in list)
        {
            yield return item;
        }
    }

    [Benchmark(Baseline = true)]
    public double SumFloatingPoint_Array_SummationCompensationNone()
    {
        return Sequence.SumFloatingPoint(ValuesArray, SummationCompensation.None);
    }

    [Benchmark]
    public double SumFloatingPoint_Array_SummationCompensationKahanBabushkaNeumaier()
    {
        return Sequence.SumFloatingPoint(ValuesArray, SummationCompensation.KahanBabushkaNeumaier);
    }
}
