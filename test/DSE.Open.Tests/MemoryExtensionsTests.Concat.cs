// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class MemoryExtensionsTestsConcat
{
    [Fact]
    public void Concat_2()
    {
        Span<int> s1 = new int[] { 1, 2, 3 };
        Span<int> s2 = new int[] { 4, 5, 6 };

        var result = s1.Concat(s2);

        Assert.True(result.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6]));
    }

    [Fact]
    public void Concat_3()
    {
        Span<int> s1 = new int[] { 1, 2, 3 };
        Span<int> s2 = new int[] { 4, 5, 6 };
        Span<int> s3 = new int[] { 7, 8, 9 };

        var result = s1.Concat(s2, s3);

        Assert.True(result.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 7, 8, 9]));
    }

    [Fact]
    public void Concat_Element()
    {
        Span<int> s1 = new int[] { 1, 2, 3 };

        var result = s1.Concat(4);

        Assert.True(result.AsSpan().SequenceEqual([1, 2, 3, 4]));
    }


    [Fact]
    public void Concat_Element_2()
    {
        Span<int> s1 = new int[] { 1, 2, 3 };

        var result = s1.Concat(4, 5);

        Assert.True(result.AsSpan().SequenceEqual([1, 2, 3, 4, 5]));
    }

}
