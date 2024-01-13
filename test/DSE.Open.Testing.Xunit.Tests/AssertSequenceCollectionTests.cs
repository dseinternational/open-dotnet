// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceCollectionTests
{
    [Fact]
    public void Empty()
    {
        AssertSequence.Empty(Enumerable.Empty<int>().ToList());
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty(Enumerable.Range(1, 10).ToList());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(100)]
    public void CountEqual(int count)
    {
        AssertSequence.CountEqual(count, Enumerable.Range(1, count).ToList());
    }

    [Fact]
    public void SingleElement()
    {
        AssertSequence.SingleElement(Enumerable.Range(1, 1).ToList());
    }
}
