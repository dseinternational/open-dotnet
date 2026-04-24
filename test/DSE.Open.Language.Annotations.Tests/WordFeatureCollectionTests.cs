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

    [Fact]
    public void JsonSerialize_WithLongFeatureCollection_DoesNotAssumeFixedItemWidth()
    {
        var feature = string.Join('|', Enumerable.Range(0, 33).Select(i => $"F{i}=A"));
        var col = WordFeatureCollection.ParseInvariant(feature);

        var json = JsonSerializer.Serialize(col);
        var deserialized = JsonSerializer.Deserialize<WordFeatureCollection>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(feature, deserialized.ToStringInvariant());
    }

    [Fact]
    public void Add_DuplicateFeatureName_Throws()
    {
        var col = WordFeatureCollection.ParseInvariant("Voice=Pass");
        var duplicate = WordFeature.ParseInvariant("Voice=Act");
        _ = Assert.Throws<InvalidOperationException>(() => col.Add(duplicate));
    }

    [Fact]
    public void Insert_DuplicateFeatureName_Throws()
    {
        var col = WordFeatureCollection.ParseInvariant("Voice=Pass|Gender=Masc");
        var duplicate = WordFeature.ParseInvariant("Voice=Act");
        _ = Assert.Throws<InvalidOperationException>(() => col.Insert(0, duplicate));
    }

    [Fact]
    public void Insert_NewFeatureName_AddsAtIndex()
    {
        var col = WordFeatureCollection.ParseInvariant("Voice=Pass|Gender=Masc");
        var added = WordFeature.ParseInvariant("Number=Sing");
        col.Insert(1, added);

        Assert.Equal(3, col.Count);
        Assert.Equal(added, col[1]);
    }

    [Fact]
    public void Insert_NullItem_Throws()
    {
        var col = WordFeatureCollection.ParseInvariant("Voice=Pass");
        _ = Assert.Throws<ArgumentNullException>(() => col.Insert(0, null!));
    }
}
