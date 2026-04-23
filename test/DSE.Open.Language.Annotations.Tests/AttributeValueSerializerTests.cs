// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public sealed class AttributeValueSerializerTests
{
    [Fact]
    public void SerializeToString_joins_values_with_pipe()
    {
        var values = new[]
        {
            AttributeValue.ParseInvariant("SpaceAfter=No"),
            AttributeValue.ParseInvariant("Lang=en"),
        };

        var result = AttributeValueSerializer.SerializeToString(values);
        Assert.Equal("SpaceAfter=No|Lang=en", result);
    }

    [Fact]
    public void SerializeToString_throws_on_null_input()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            AttributeValueSerializer.SerializeToString(null!));
    }

    [Fact]
    public void TryDeserialize_empty_span_returns_empty_sequence()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize(ReadOnlySpan<char>.Empty, out var result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryDeserialize_null_string_returns_empty_sequence()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize((string?)null, out var result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryDeserialize_single_attribute_value()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize("SpaceAfter=No", out var result));
        var list = result.ToList();
        _ = Assert.Single(list);
        Assert.Equal("SpaceAfter", list[0].Name.ToStringInvariant());
        Assert.Equal("No", list[0].Value.ToStringInvariant());
    }

    [Fact]
    public void TryDeserialize_multiple_attribute_values()
    {
        Assert.True(AttributeValueSerializer.TryDeserialize("SpaceAfter=No|Lang=en", out var result));
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("SpaceAfter", list[0].Name.ToStringInvariant());
        Assert.Equal("Lang", list[1].Name.ToStringInvariant());
    }

    [Fact]
    public void Serialize_then_deserialize_roundtrips()
    {
        var original = new[]
        {
            AttributeValue.ParseInvariant("SpaceAfter=No"),
            AttributeValue.ParseInvariant("Lang=en"),
        };

        var serialized = AttributeValueSerializer.SerializeToString(original);
        Assert.True(AttributeValueSerializer.TryDeserialize(serialized, out var deserialized));

        var list = deserialized.ToList();
        Assert.Equal(original.Length, list.Count);
        for (var i = 0; i < original.Length; i++)
        {
            Assert.Equal(original[i], list[i]);
        }
    }

    [Fact]
    public void TrySerialize_returns_false_when_destination_too_small()
    {
        var values = new[]
        {
            AttributeValue.ParseInvariant("SpaceAfter=No"),
            AttributeValue.ParseInvariant("Lang=en"),
        };

        Span<char> destination = stackalloc char[4];
        var success = AttributeValueSerializer.TrySerialize(destination, values, out var charsWritten);

        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }
}
