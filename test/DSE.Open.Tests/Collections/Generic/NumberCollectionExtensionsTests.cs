// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class NumberCollectionExtensionsTests
{
    private static readonly int[] s_ints = [1, 2, 3];

    [Fact]
    public void ConvertChecked_InRange_ConvertsElements()
    {
        IEnumerable<int> source = s_ints;

        var result = source.ConvertChecked<int, long>().ToArray();

        Assert.Equal([1L, 2L, 3L], result);
    }

    [Fact]
    public void ConvertChecked_Overflow_Throws()
    {
        IEnumerable<int> source = [int.MaxValue];

        _ = Assert.Throws<OverflowException>(() => source.ConvertChecked<int, short>().ToArray());
    }

    [Fact]
    public void ConvertChecked_NullSource_Throws()
    {
        IEnumerable<int>? source = null;

        _ = Assert.Throws<ArgumentNullException>(() => source!.ConvertChecked<int, long>().ToArray());
    }

    [Fact]
    public void ConvertSaturating_Overflow_Saturates()
    {
        IEnumerable<int> source = [int.MaxValue, int.MinValue, 0];

        var result = source.ConvertSaturating<int, short>().ToArray();

        Assert.Equal([short.MaxValue, short.MinValue, (short)0], result);
    }

    [Fact]
    public void ConvertSaturating_NullSource_Throws()
    {
        IEnumerable<int>? source = null;

        _ = Assert.Throws<ArgumentNullException>(() => source!.ConvertSaturating<int, long>().ToArray());
    }

    [Fact]
    public void ConvertTruncating_Truncates()
    {
        IEnumerable<double> source = [1.9, -1.9, 3.5];

        var result = source.ConvertTruncating<double, int>().ToArray();

        Assert.Equal([1, -1, 3], result);
    }

    [Fact]
    public void ConvertTruncating_NullSource_Throws()
    {
        IEnumerable<int>? source = null;

        _ = Assert.Throws<ArgumentNullException>(() => source!.ConvertTruncating<int, long>().ToArray());
    }
}
