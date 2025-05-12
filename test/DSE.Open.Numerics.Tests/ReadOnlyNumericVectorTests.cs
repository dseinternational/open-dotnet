// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics;

public sealed class ReadOnlyNumericVectorTests
{
    [Fact]
    public void Init()
    {
        ReadOnlyVector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = ReadOnlyVector.Create([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.AsReadOnlySpan().SequenceEqual(v2.AsReadOnlySpan()));
    }

    [Fact]
    public void JsonRoundtrip()
    {
        // Arrange
        var vector = ReadOnlyVector.Create([1, 2, 3, 4, 5, 6]);

        // Act
        var json = JsonSerializer.Serialize(vector);
        var deserializedVector = JsonSerializer.Deserialize<ReadOnlyVector<int>>(json);

        // Assert
        Assert.NotNull(deserializedVector);
        Assert.True(vector.SequenceEqual(deserializedVector));
    }
}
