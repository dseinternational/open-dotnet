// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Text;

namespace DSE.Open.Benchmarks.Text;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class StringHelperRemovePunctuationBenchmarks
{
    private const string LargeNoPunctuation = "The cat is on the bed and the dog is on the floor and the mouse is on the table and the horse is in the field and the cow is in the barn";
    private const string LargePunctuation = "The cat is on the bed, and the dog is on the floor, and the mouse is on the table, and the horse is in the field, and the cow is in the barn.";
    private const string SmallNoPunctuation = "The cat is on the bed";
    private const string SmallPunctuation = "The cat is on the bed.";

    public StringHelperRemovePunctuationBenchmarks()
    {
        if (LargePunctuation.Length < StackallocThresholds.MaxCharLength)
        {
            throw new InvalidOperationException($"LargePunctuation is {LargePunctuation.Length} characters long, which is less than StackAllocThreshold");
        }
        
        if (LargeNoPunctuation.Length < StackallocThresholds.MaxCharLength)
        {
            throw new InvalidOperationException($"LargeNoPunctuation is {LargeNoPunctuation.Length} characters long, which is less than StackAllocThreshold");
        }
        
        if (SmallPunctuation.Length > StackallocThresholds.MaxCharLength)
        {
            throw new InvalidOperationException($"SmallPunctuation is {SmallPunctuation.Length} characters long, which is greater than StackAllocThreshold");
        }

        if (SmallNoPunctuation.Length > StackallocThresholds.MaxCharLength)
        {
            throw new InvalidOperationException($"SmallNoPunctuation is {SmallNoPunctuation.Length} characters long, which is greater than StackAllocThreshold");
        }
    }
    
    [Benchmark]
    public string RemovePunctuation_Large_NoPunctuation() => StringHelper.RemovePunctuation(LargeNoPunctuation);
    
    [Benchmark]
    public string RemovePunctuation_Large_Punctuation() => StringHelper.RemovePunctuation(LargePunctuation);
    
    [Benchmark]
    public string RemovePunctuation_Small_NoPunctuation() => StringHelper.RemovePunctuation(SmallNoPunctuation);
    
    [Benchmark]
    public string RemovePunctuation_Small_Punctuation() => StringHelper.RemovePunctuation(SmallPunctuation);
}

#pragma warning restore CA1822 // Mark members as static
