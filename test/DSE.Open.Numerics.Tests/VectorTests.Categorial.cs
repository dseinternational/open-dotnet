// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        return options;
    });

    [Fact]
    public void Init_Categorial()
    {
        var vector = Vector.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [
                KeyValuePair.Create("one", 1),
                KeyValuePair.Create("two", 2),
                KeyValuePair.Create("three", 3),
                KeyValuePair.Create("four", 4),
                KeyValuePair.Create("five", 5),
                KeyValuePair.Create("six", 6)
            ]);

        Assert.Equal(18, vector.Length);
        Assert.True(vector.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, vector.Categories.Count);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var vector = Vector.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [
                KeyValuePair.Create("one", 1),
                KeyValuePair.Create("two", 2),
                KeyValuePair.Create("three", 3),
                KeyValuePair.Create("four", 4),
                KeyValuePair.Create("five", 5),
                KeyValuePair.Create("six", 6)
            ]);

        var json = JsonSerializer.Serialize(vector, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);

        Assert.Equal(18, deserialized.Length);
        Assert.True(deserialized.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, deserialized.Categories.Count);
    }

    [Fact]
    public void AsReadOnly_ShouldReturnReadOnlySeries()
    {
        // Arrange
        var vector = Vector.Create([1, 1, 2, 3, 2],
        [
            KeyValuePair.Create("one", 1),
            KeyValuePair.Create("two", 2),
            KeyValuePair.Create("three", 3),
        ]);

        // Act
        var readOnlyVector = vector.AsReadOnly();

        // Assert
        _ = Assert.IsType<ReadOnlyVector<int>>(readOnlyVector);
    }
}
