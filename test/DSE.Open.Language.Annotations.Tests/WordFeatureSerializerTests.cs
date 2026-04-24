// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class WordFeatureSerializerTests
{
    [Theory]
    [InlineData("Voice=Pass")]
    [InlineData("Voice=Pass|Gender=Masc")]
    [InlineData("Voice=Pass|Number=Sing|Gender=Masc")]
    public void TrySerialize_Roundtrips(string features)
    {
        Assert.True(WordFeatureSerializer.TryDeserialize(features, out var parsed));
        var list = parsed.ToList();

        Span<char> buffer = stackalloc char[256];
        Assert.True(WordFeatureSerializer.TrySerialize(buffer, list, out var charsWritten));

        Assert.Equal(features, buffer[..charsWritten].ToString());
    }

    [Fact]
    public void TrySerialize_BufferTooSmall_ReturnsFalse()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize("Voice=Pass|Gender=Masc", out var parsed));
        var list = parsed.ToList();

        Span<char> tooSmall = stackalloc char[5];
        var result = WordFeatureSerializer.TrySerialize(tooSmall, list, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TrySerialize_BufferExactlyFits_ReturnsTrue()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize("Voice=Pass|Gender=Masc", out var parsed));
        var list = parsed.ToList();

        Span<char> exact = stackalloc char["Voice=Pass|Gender=Masc".Length];
        Assert.True(WordFeatureSerializer.TrySerialize(exact, list, out var charsWritten));
        Assert.Equal(exact.Length, charsWritten);
    }

    [Fact]
    public void TrySerialize_NoSeparatorRoomBetweenItems_ReturnsFalse()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize("Voice=Pass|Gender=Masc", out var parsed));
        var list = parsed.ToList();

        // Buffer just large enough for the first feature ("Voice=Pass") but not
        // for the separator that follows.
        Span<char> noRoomForSeparator = stackalloc char["Voice=Pass".Length];
        var result = WordFeatureSerializer.TrySerialize(noRoomForSeparator, list, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryDeserialize_Empty_ReturnsTrueAndEmptyCollection()
    {
        Assert.True(WordFeatureSerializer.TryDeserialize(default(string), out var features));
        Assert.Empty(features);
    }

    [Fact]
    public void TryDeserialize_InvalidValue_ReturnsFalse()
    {
        Assert.False(WordFeatureSerializer.TryDeserialize("Voice=Pass|=Bad", out _));
    }

    [Fact]
    public void SerializeToString_RoundtripsMultiValueFeature()
    {
        var input = "Case=Acc,Dat";
        Assert.True(WordFeatureSerializer.TryDeserialize(input, out var parsed));
        var list = parsed.ToList();

        var roundtrip = WordFeatureSerializer.SerializeToString(list);
        Assert.Equal(input, roundtrip);
    }

    [Fact]
    public void TryDeserialize_WithMoreThanThirtyTwoFeatures_ReturnsAllFeatures()
    {
        var input = string.Join('|', Enumerable.Range(0, 33).Select(i => $"F{i}=A"));

        Assert.True(WordFeatureSerializer.TryDeserialize(input, out var parsed));

        var list = parsed.ToList();
        Assert.Equal(33, list.Count);
        Assert.Equal(input, WordFeatureSerializer.SerializeToString(list));
    }
}
