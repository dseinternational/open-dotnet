// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SequenceTests
{
    [Fact]
    public void Mean_Span_Int32_Int32()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = Sequence.Mean<int, int>(sequence);
        Assert.Equal(5, mean);
    }

    [Fact]
    public void Mean_Span_Int32_Double()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = Sequence.Mean<int, double>(sequence);
        Assert.Equal(5.5, mean);
    }

    [Fact]
    public void Mean_Enumerable_Int32_Int32()
    {
        IEnumerable<int> sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = Sequence.Mean<int, int>(sequence);
        Assert.Equal(5, mean);
    }

    [Fact]
    public void Mean_Array_Int32_Double()
    {
        int[] sequence = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var mean = Sequence.Mean<int, double>((IList<int>)sequence);
        Assert.Equal(5.5, mean);
    }

    [Fact]
    public void Mean_List_Double()
    {
        var sequence = Enumerable.Range(1,500).Select(i => i * 3.333).ToList();
        var mean = Sequence.Mean(sequence);
        Assert.Equal(834.91650000000004, mean, 0.00000000000001);
    }
}
