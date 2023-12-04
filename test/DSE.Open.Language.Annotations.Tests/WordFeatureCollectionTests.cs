// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public class WordFeatureCollectionTests
{
    [Theory]
    [InlineData("Voice=Pass", 1)]
    [InlineData("Voice=Pass|Gender=Masc", 2)]
    [InlineData("Voice=Pass|Number=Sing|Gender=Masc", 3)]
    public void ParseAndFormat(string feature, int count)
    {
        var col = WordFeatureCollection.ParseInvariant(feature);
        Assert.Equal(count, col.Count);
        var s = col.ToStringInvariant();
        Assert.Equal(feature, s);
    }

    [Theory]
    [InlineData("Voice=Pass", 1)]
    [InlineData("Voice=Pass|Gender=Masc", 2)]
    [InlineData("Voice=Pass|Number=Sing|Gender=Masc", 3)]
    public void JsonSerializeDeserialize(string feature, int count)
    {
        var col = WordFeatureCollection.ParseInvariant(feature);
        Assert.Equal(count, col.Count);
        var json = JsonSerializer.Serialize(col);
        var deserialized = JsonSerializer.Deserialize<WordFeatureCollection>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(count, deserialized.Count);

        for (var i = 0; i < deserialized.Count; i++)
        {
            Assert.Equal(col[i], deserialized[i]);
        }
    }
}
