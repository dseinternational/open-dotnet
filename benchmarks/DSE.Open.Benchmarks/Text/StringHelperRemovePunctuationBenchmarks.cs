// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Text;

namespace DSE.Open.Benchmarks.Text;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class StringHelperRemovePunctuationBenchmarks
{
    private const string LargeNoPunctuation =
        "The cat is on the bed and the dog is on the floor and the mouse is on the table and the " +
        "horse is in the field and the cow is in the barn";
    private const string LargePunctuation =
        "The cat is on the bed, and the dog is on the floor, and the mouse is on the table, and the " +
        "horse is in the field, and the cow is in the barn.";
    private const string SmallNoPunctuation =
        "The cat is on the bed";
    private const string SmallPunctuation =
        "The cat is on the bed.";

    public StringHelperRemovePunctuationBenchmarks()
    {
        if (LargePunctuation.Length < MemoryThresholds.StackallocCharThreshold)
        {
            throw new InvalidOperationException(
                $"LargePunctuation is {LargePunctuation.Length} characters long, which is less " +
                $"than MemoryThresholds.StackallocCharThreshold");
        }

        if (LargeNoPunctuation.Length < MemoryThresholds.StackallocCharThreshold)
        {
            throw new InvalidOperationException(
                $"LargeNoPunctuation is {LargeNoPunctuation.Length} characters long, which is less " +
                $"than MemoryThresholds.StackallocCharThreshold");
        }

        if (SmallPunctuation.Length > MemoryThresholds.StackallocCharThreshold)
        {
            throw new InvalidOperationException(
                $"SmallPunctuation is {SmallPunctuation.Length} characters long, which is greater " +
                $"than MemoryThresholds.StackallocCharThreshold");
        }

        if (SmallNoPunctuation.Length > MemoryThresholds.StackallocCharThreshold)
        {
            throw new InvalidOperationException(
                $"SmallNoPunctuation is {SmallNoPunctuation.Length} characters long, which is " +
                $"greater than MemoryThresholds.StackallocCharThreshold");
        }
    }

    [Benchmark]
    public string RemovePunctuation_Large_NoPunctuation()
    {
        return StringHelper.RemovePunctuation(LargeNoPunctuation);
    }

    [Benchmark]
    public string RemovePunctuation_Large_Punctuation()
    {
        return StringHelper.RemovePunctuation(LargePunctuation);
    }

    [Benchmark]
    public string RemovePunctuation_Small_NoPunctuation()
    {
        return StringHelper.RemovePunctuation(SmallNoPunctuation);
    }

    [Benchmark]
    public string RemovePunctuation_Small_Punctuation()
    {
        return StringHelper.RemovePunctuation(SmallPunctuation);
    }
}

#pragma warning restore CA1822 // Mark members as static
