// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public partial class AssertSequenceTests
{
    [Fact]
    public void Empty()
    {
        AssertSequence.Empty(Enumerable.Empty<int>());
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty(Enumerable.Range(1, 10));
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);

        int[] sequence2 = [5, 5, 5, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_throws_if_not()
    {
        int[] sequence = [1, 1, 3, 4, 3];
        _ = Assert.Throws<SequenceException>(() => AssertSequence.EachGreaterThanOrEqualToPrevious(sequence));
    }
}
