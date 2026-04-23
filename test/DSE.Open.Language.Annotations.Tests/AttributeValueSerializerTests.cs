// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class AttributeValueSerializerTests
{
    [Theory]
    [InlineData("Case=Acc")]
    [InlineData("Case=Acc|Gender=Masc")]
    [InlineData("Case=Acc,Dat|Gender=Masc")]
    public void TrySerialize_Roundtrips(string values)
    {
        Assert.True(AttributeValueSerializer.TryDeserialize(values, out var parsed));
        var list = parsed.ToList();

        Span<char> buffer = stackalloc char[256];
        Assert.True(AttributeValueSerializer.TrySerialize(buffer, list, out var charsWritten));

        Assert.Equal(values, buffer[..charsWritten].ToString());
    }

    [Fact]
    public void TrySerialize_BufferTooSmall_ReturnsFalse()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize("Case=Acc|Gender=Masc", out var parsed));
        var list = parsed.ToList();

        Span<char> tooSmall = stackalloc char[5];
        var result = AttributeValueSerializer.TrySerialize(tooSmall, list, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TrySerialize_BufferExactlyFits_ReturnsTrue()
    {
        var input = "Case=Acc|Gender=Masc";
        Assert.True(AttributeValueSerializer.TryDeserialize(input, out var parsed));
        var list = parsed.ToList();

        Span<char> exact = stackalloc char[input.Length];
        Assert.True(AttributeValueSerializer.TrySerialize(exact, list, out var charsWritten));
        Assert.Equal(exact.Length, charsWritten);
    }

    [Fact]
    public void TrySerialize_NoSeparatorRoomBetweenItems_ReturnsFalse()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize("Case=Acc|Gender=Masc", out var parsed));
        var list = parsed.ToList();

        // Buffer fits the first attribute "Case=Acc" but leaves no room for "|".
        Span<char> noRoomForSeparator = stackalloc char["Case=Acc".Length];
        var result = AttributeValueSerializer.TrySerialize(noRoomForSeparator, list, out var charsWritten);

        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryDeserialize_Empty_ReturnsTrueAndEmptyCollection()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize(default(string), out var values));
        Assert.Empty(values);
    }

    [Fact]
    public void TryDeserialize_InvalidValue_ReturnsFalse()
    {
        Assert.False(AttributeValueSerializer.TryDeserialize("Case=Acc|=Bad", out _));
    }

    [Fact]
    public void SerializeToString_RoundtripsMultiValueAttribute()
    {
        var input = "Case=Acc,Dat";
        Assert.True(AttributeValueSerializer.TryDeserialize(input, out var parsed));
        var list = parsed.ToList();

        var roundtrip = AttributeValueSerializer.SerializeToString(list);
        Assert.Equal(input, roundtrip);
    }
}
