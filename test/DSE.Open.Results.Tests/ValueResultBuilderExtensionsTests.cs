// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results.Tests;

public sealed class ValueResultBuilderExtensionsTests
{
    [Fact]
    public void BuildWithValue_ReturnsValue()
    {
        // Arrange
        var builder = new ValueResultBuilder<int>();

        // Act
        var result = builder.BuildWithValue(42);

        // Assert
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void BuildWithValue_WithCollection_ReturnsCollection()
    {
        // Arrange
        var builder = new CollectionValueResultBuilder<int>();

        // Act
        var result = builder.BuildWithValue([4, 2]);

        // Assert
        Assert.Equal([4, 2], result.Value);
    }

    [Fact]
    public void BuildWithValue_WithAsyncCollection_ReturnsCollection()
    {
        // Arrange
        var builder = new CollectionValueAsyncResultBuilder<int>();

        // Act
        List<int> list = [4, 2];
        var result = builder.BuildWithValue(list.ToAsyncEnumerable());

        // Assert
        Assert.Equal([4, 2], result.Value);
    }
}
