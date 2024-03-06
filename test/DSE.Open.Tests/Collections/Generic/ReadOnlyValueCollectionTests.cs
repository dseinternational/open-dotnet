// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public class ReadOnlyValueCollectionTests
{
    [Fact]
    public void Equal_returns_true_identical_value_collections()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));

        Assert.Equal(c1, c2);
    }

    [Fact]
    public void Equal_returns_false_nonidentical_value_collections()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var c2 = new ReadOnlyValueCollection<int>(Enumerable.Range(2, 20));

        Assert.NotEqual(c1, c2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var c1 = new ReadOnlyValueCollection<int>(Enumerable.Range(0, 20));
        var json = JsonSerializer.Serialize(c1);
        var c2 = JsonSerializer.Deserialize<ReadOnlyValueCollection<int>>(json);
        Assert.Equal(c1, c2);
    }


    [Fact]
    public void CreateRange_WithIEnumerable_ShouldCreate()
    {
        // Arrange
        IEnumerable<int> enumerable = new[] { 0, 1 };

        // Act
        var collection = ReadOnlyValueCollection.CreateRange(enumerable);

        // Assert
        Assert.Equal(2, collection.Count);
        Assert.Equal(0, collection[0]);
        Assert.Equal(1, collection[1]);
    }

    [Fact]
    public void CreateRange_WithRovc_ShouldCreate()
    {
        // Arrange
        ReadOnlyValueCollection<int> initial = [3, 2];

        // Act
        var collection = ReadOnlyValueCollection.CreateRange(initial);

        // Assert
        Assert.Equal(2, initial.Count);
        Assert.Equal(2, collection.Count);
        Assert.Equal(3, collection[0]);
        Assert.Equal(2, collection[1]);
    }

    [Fact]
    public void Create_WithSpan_ShouldCreate()
    {
        // Act
        var collection = ReadOnlyValueCollection.Create(stackalloc int[] { 0, 2 });

        // Assert
        Assert.Equal(0, collection[0]);
        Assert.Equal(2, collection[1]);
        Assert.Equal(2, collection.Count);
    }

    [Fact]
    public void Create_WithCollectionLiteral_ShouldCreate()
    {
        // Act
        ReadOnlyValueCollection<int> collection = [0, 1];

        // Assert
        Assert.Equal(0, collection[0]);
        Assert.Equal(1, collection[1]);
        Assert.Equal(2, collection.Count);
    }

    [Fact]
    public void Foreach_ShouldIterate()
    {
        // Arrange
        const int lower = 0;
        const int upper = 10;
        ReadOnlyValueCollection<int> collection = [..Enumerable.Range(lower, upper)];

        // Act
        var acc = 0;

        foreach (var item in collection)
        {
            acc += item;
        }

        // Assert
        var expected = Enumerable.Range(lower, upper).Sum();
        Assert.Equal(expected, acc);
        Assert.Equal(expected, collection.Sum());
    }

    [Fact]
    public void IEnumerable_Sum()
    {
        // Arrange
        const int lower = 0;
        const int upper = 10;
        ReadOnlyValueCollection<int> collection = [..Enumerable.Range(lower, upper)];

        // Act
        var sum = collection.Sum();

        // Assert
        Assert.Equal(Enumerable.Range(lower, upper).Sum(), sum);
    }

    [Fact]
    public void GetEnumerator_ShouldReturnStructEnumerator()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [0, 1, 2];

        // Act
        using var enumerator = collection.GetEnumerator();

        // Assert
        _ = Assert.IsType<ReadOnlyValueCollection<int>.Enumerator>(enumerator);
    }

    [Fact]
    public void GetEnumerator_IEnumerable_ShouldReturnStructEnumerator()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [0, 1, 2];

        // Act
        var enumerator = ((IEnumerable)collection).GetEnumerator();

        // Assert
        _ = Assert.IsType<ReadOnlyValueCollection<int>.Enumerator>(enumerator);
    }


    [Fact]
    public void GetEnumerator_IEnumerableT_ShouldReturnStructEnumerator()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [0, 1, 2];

        // Act
        using var enumerator = ((IEnumerable<int>)collection).GetEnumerator();

        // Assert
        _ = Assert.IsType<ReadOnlyValueCollection<int>.Enumerator>(enumerator);
    }

    [Fact]
    public void Reset_ShouldThrowNotSupported()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [0, 1, 2];
        using var enumerator = collection.GetEnumerator();

        // Act
        void Act() => ((IEnumerator)enumerator).Reset();

        // Assert
        _ = Assert.Throws<NotSupportedException>(Act);
    }
}
