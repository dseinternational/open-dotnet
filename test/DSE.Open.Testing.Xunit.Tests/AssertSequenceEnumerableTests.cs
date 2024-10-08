// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceEnumerableTests
{
    [Fact]
    public void TrueForAll()
    {
        AssertSequence.TrueForAll(v => v < 101, Enumerable.Range(1, 100));
    }

    [Fact]
    public void TrueForAny()
    {
        AssertSequence.TrueForAny(v => v > 99, Enumerable.Range(1, 100));
    }

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
    public void AllZero()
    {
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0L));
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0.0));
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0m));
    }

    [Fact]
    public void EachGreaterThanPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanPrevious(sequence);

        int[] sequence2 = [10, 12, 14, 20];
        AssertSequence.EachGreaterThanPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanPrevious_throws_if_not()
    {
        int[] sequence = [1, 2, 3, 4, 4];
        _ = Assert.Throws<SequenceException>(() => AssertSequence.EachGreaterThanPrevious(sequence));
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
