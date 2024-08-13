// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using CollectionExtensions = DSE.Open.Collections.Generic.CollectionExtensions;

namespace DSE.Open.Tests.Collections.Generic;

public sealed partial class CollectionExtensionsTests
{
    [Fact]
    public void Single_WithSingle_ShouldReturnSingle()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [123];

        // Act
        var single = CollectionExtensions.Single(collection);

        // Assert
        Assert.Equal(((IEnumerable<int>)collection).Single(), single);
    }

    [Fact]
    public void Single_WithZero_ShouldThrowInvalidOperationException()
    {
        // Arrange
        ReadOnlyValueCollection<int> emptyCollection = [];

        // Act
        void Act() => _ = emptyCollection.Single();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void Single_WithMultiple_ShouldThrowInvalidOperationException()
    {
        // Arrange
        ReadOnlyValueCollection<int> multipleCollection = [1, 2];

        // Act
        void Act() => _ = multipleCollection.Single();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void SingleOrDefault_WithSingle_ShouldReturnSingle()
    {
        // Arrange
        ReadOnlyValueCollection<int> collection = [123];

        // Act
        var single = collection.SingleOrDefault();

        // Assert
        Assert.Equal(((IEnumerable<int>)collection).Single(), single);
    }

    [Fact]
    public void SingleOrDefault_WithZero_ShouldReturnDefault()
    {
        // Arrange
        ReadOnlyValueCollection<int> empty = [];

        // Act
        var result = empty.SingleOrDefault();

        // Assert
        Assert.Equal(((IEnumerable<int>)empty).SingleOrDefault(), result);
        Assert.Equal(default, result);
    }

    [Fact]
    public void SingleOrDefault_WithMultiple_ShouldThrowInvalidOperationException()
    {
        // Arrange
        ReadOnlyValueCollection<int> multiple = [1, 2];

        // Act
        void Act() => _ = multiple.SingleOrDefault();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }
}
