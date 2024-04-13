// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public class CollectionExtensionsTests
{
    private static readonly int[] s_integersInOrder = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

    [Fact]
    public void ShuffledListHasNewOrder()
    {
        var list = new List<int>(s_integersInOrder);

        list.Shuffle();
        var inOrder = false;

        for (var i = 1; i < list.Count; i++)
        {
            inOrder = list[i - 1] < list[i];

            if (!inOrder)
            {
                break;
            }
        }

        Assert.False(inOrder);
    }

    [Fact]
    public void FindIndex_IList()
    {
        IList<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 15];

        Assert.Equal(13, list.FindIndex(i => i == 14));
    }

    [Fact]
    public void FindIndex_IReadOnlyList()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 15];

        Assert.Equal(13, list.FindIndex(i => i == 14));
    }

    [Fact]
    public void FindLastIndex_IList()
    {
        IList<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 15];

        Assert.Equal(14, list.FindLastIndex(i => i == 14));
    }

    [Fact]
    public void FindLastIndex_IReadOnlyList()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 15];

        Assert.Equal(14, list.FindLastIndex(i => i == 14));
    }

    [Fact]
    public void FindIndex_WithIListStartAndCount_ShouldReturnCorrectIndex()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        const int target = 6;

        // Act
        var index = ((IList<int>)list).FindIndex(startIndex: 5, count: 2, i => i == target);

        // Assert
        Assert.Equal(target, index);
    }

    [Fact]
    public void FindIndex_WithIReadOnlyListStartAndCount_ShouldReturnCorrectIndex()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        const int target = 6;

        // Act
        var index = ((IReadOnlyList<int>)list).FindIndex(startIndex: 5, count: 2, i => i == target);

        // Assert
        Assert.Equal(target, index);
    }

    [Fact]
    public void CountUnsigned_ShouldReturnCorrectCount()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        var count = list.CountUnsigned();

        // Assert
        Assert.Equal(10u, count);
    }

    [Fact]
    public void CountUnsigned_WithPredicate_ShouldReturnCorrectCount()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        var count = list.CountUnsigned(i => i % 2 == 0);

        // Assert
        Assert.Equal(5u, count);

    }

    [Fact]
    public void CountUnsignedUnchecked_ShouldReturnCorrectCount()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        var count = list.CountUnsignedUnchecked();

        // Assert
        Assert.Equal(10u, count);
    }

    [Fact]
    public void CountUnsignedUnchecked_WithPredicate_ShouldReturnCorrectCount()
    {
        // Arrange
        List<int> list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        var count = list.CountUnsignedUnchecked(i => i % 2 == 0);

        // Assert
        Assert.Equal(5u, count);
    }
}
