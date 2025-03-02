// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;
using System.Text.Json;

namespace DSE.Open.Numerics;

public partial class CategorialVectorTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        return options;
    });

    public CategorialVectorTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Init()
    {
        var vector = Vector.CreateCategorical(
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
        Assert.True(vector.Span.SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, vector.CategoryData.Length);
        Assert.True(vector.IsValid());
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var vector = Vector.CreateCategorical(
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

        var deserialized = JsonSerializer.Deserialize<CategoricalVector<int>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);

        Assert.Equal(18, deserialized.Length);
        Assert.True(deserialized.Span.SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, deserialized.CategoryData.Length);
        Assert.True(deserialized.IsValid());
    }
}
