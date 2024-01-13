// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceMemoryTests
{
    [Fact]
    public void TrueForAll()
    {
        AssertSequence.TrueForAll<int>(v => v < 101, Enumerable.Range(1, 100).ToArray().AsMemory());
        AssertSequence.TrueForAll<int>(v => v < 101, Enumerable.Range(1, 100).ToArray().AsSpan());
    }

    [Fact]
    public void TrueForAny()
    {
        AssertSequence.TrueForAny<int>(v => v > 99, Enumerable.Range(1, 100).ToArray().AsMemory());
        AssertSequence.TrueForAny<int>(v => v > 99, Enumerable.Range(1, 100).ToArray().AsSpan());
    }

    [Fact]
    public void Empty()
    {
        AssertSequence.Empty<int>(Enumerable.Empty<int>().ToArray().AsMemory());
        AssertSequence.Empty<int>(Enumerable.Empty<int>().ToArray().AsSpan());
    }

    [Fact]
    public void Empty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.Empty<int>(Enumerable.Range(1, 10).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.Empty<int>(Enumerable.Range(1, 10).ToArray().AsSpan()));
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty<int>(Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.NotEmpty<int>(Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void CountEqual()
    {
        AssertSequence.CountEqual<int>(10, Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.CountEqual<int>(10, Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void CountGreaterThan()
    {
        AssertSequence.CountGreaterThan<int>(9, Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.CountGreaterThan<int>(9, Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void AllZero()
    {
        AssertSequence.AllZero<long>(Enumerable.Range(1, 10).Select(i => 0L).ToArray().AsMemory());
        AssertSequence.AllZero<double>(Enumerable.Range(1, 10).Select(i => 0.0).ToArray().AsMemory());
        AssertSequence.AllZero<decimal>(Enumerable.Range(1, 10).Select(i => 0m).ToArray().AsMemory());
        AssertSequence.AllZero<long>(Enumerable.Range(1, 10).Select(i => 0L).ToArray().AsSpan());
        AssertSequence.AllZero<double>(Enumerable.Range(1, 10).Select(i => 0.0).ToArray().AsSpan());
        AssertSequence.AllZero<decimal>(Enumerable.Range(1, 10).Select(i => 0m).ToArray().AsSpan());
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);

        ReadOnlySpan<int> sequence2 = [5, 5, 5, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
        {
            ReadOnlySpan<int> sequence = [1, 1, 3, 4, 3];
            AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);
        });
    }
}
