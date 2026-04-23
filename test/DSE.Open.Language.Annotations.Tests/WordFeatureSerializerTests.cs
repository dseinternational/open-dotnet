// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public sealed class WordFeatureSerializerTests
{
    [Fact]
    public void SerializeToString_joins_features_with_pipe()
    {
        var features = new[]
        {
            WordFeature.ParseInvariant("Number=Sing"),
            WordFeature.ParseInvariant("Tense=Past"),
        };

        var result = WordFeatureSerializer.SerializeToString(features);
        Assert.Equal("Number=Sing|Tense=Past", result);
    }

    [Fact]
    public void SerializeToString_throws_on_null_input()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            WordFeatureSerializer.SerializeToString(null!));
    }

    [Fact]
    public void TryDeserialize_empty_span_returns_empty_sequence()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize(ReadOnlySpan<char>.Empty, out var result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryDeserialize_null_string_returns_empty_sequence()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize((string?)null, out var result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryDeserialize_single_feature()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize("Number=Sing", out var result));
        var list = result.ToList();
        _ = Assert.Single(list);
        Assert.Equal("Number", list[0].Name.ToStringInvariant());
    }

    [Fact]
    public void TryDeserialize_multiple_features()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize("Number=Sing|Tense=Past", out var result));
        var list = result.ToList();
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void Serialize_then_deserialize_roundtrips()
    {
        var original = new[]
        {
            WordFeature.ParseInvariant("Number=Sing"),
            WordFeature.ParseInvariant("Tense=Past"),
        };

        var serialized = WordFeatureSerializer.SerializeToString(original);
        Assert.True(WordFeatureSerializer.TryDeserialize(serialized, out var deserialized));

        var list = deserialized.ToList();
        Assert.Equal(original.Length, list.Count);
    }

    [Fact]
    public void TrySerialize_returns_false_when_destination_too_small()
    {
        var features = new[]
        {
            WordFeature.ParseInvariant("Number=Sing"),
            WordFeature.ParseInvariant("Tense=Past"),
        };

        Span<char> destination = stackalloc char[4];
        var success = WordFeatureSerializer.TrySerialize(destination, features, out var charsWritten);

        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }
}
