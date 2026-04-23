// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceCollectionTests
{
    [Fact]
    public void True_Success()
    {
        ICollection<int> sequence = [1, 2];
        AssertSequence.True<int>(s => s.Count > 0, sequence);
    }

    [Fact]
    public void True_Throws_When_Predicate_False()
    {
        ICollection<int> sequence = [1, 2];
        _ = Assert.Throws<SequenceException>(() => AssertSequence.True<int>(s => s.Count > 10, sequence));
    }

    [Fact]
    public void True_NullAssertion_Throws()
    {
        ICollection<int> sequence = [1, 2];
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.True(((Func<ICollection<int>, bool>)null!), sequence));
    }

    [Fact]
    public void True_NullSequence_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.True<int>(s => true, (ICollection<int>)null!));
    }

    [Fact]
    public void Empty()
    {
        AssertSequence.Empty(Enumerable.Empty<int>().ToList());
    }

    [Fact]
    public void Empty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.Empty(Enumerable.Range(1, 3).ToList()));
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty(Enumerable.Range(1, 10).ToList());
    }

    [Fact]
    public void NotEmpty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.NotEmpty(Enumerable.Empty<int>().ToList()));
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
    public void CountEqual_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.CountEqual(5, Enumerable.Range(1, 3).ToList()));
    }

    [Fact]
    public void CountGreaterThan()
    {
        AssertSequence.CountGreaterThan(9, Enumerable.Range(1, 10).ToList());
    }

    [Fact]
    public void CountGreaterThan_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.CountGreaterThan(5, Enumerable.Range(1, 3).ToList()));
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.CountGreaterThan(3, Enumerable.Range(1, 3).ToList()));
    }

    [Fact]
    public void SingleElement()
    {
        AssertSequence.SingleElement(Enumerable.Range(1, 1).ToList());
    }

    [Fact]
    public void SingleElement_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.SingleElement(Enumerable.Range(1, 2).ToList()));
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.SingleElement(Enumerable.Empty<int>().ToList()));
    }
}
