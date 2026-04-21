// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language;

public sealed class SentenceMeaningIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (SentenceMeaningId)258089004501;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void GetRandomId_ReturnsValidValue()
    {
        for (var i = 0; i < 1000; i++)
        {
            var id = SentenceMeaningId.GetRandomId();
            Assert.True(SentenceMeaningId.IsValidValue((long)id));
        }
    }

    [Fact]
    public void ToInt64_round_trips_via_FromInt64()
    {
        var id = SentenceMeaningId.FromUInt64(258089004501ul);
        var i = id.ToInt64();
        Assert.Equal(id, SentenceMeaningId.FromInt64(i));
    }

    [Fact]
    public void ToUInt64_round_trips_via_FromUInt64()
    {
        var id = SentenceMeaningId.FromUInt64(258089004501ul);
        var u = id.ToUInt64();
        Assert.Equal(id, SentenceMeaningId.FromUInt64(u));
    }

    [Fact]
    public void FromUInt64_throws_for_out_of_range_value()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => SentenceMeaningId.FromUInt64(1ul));
    }

    // Pins a specific hash output so any future change to the serialization
    // format is caught on every platform. Regression guard for #354 item 4.
    [Theory]
    [InlineData("The cat sat on the mat.", 461905491987ul)]
    [InlineData("To be, or not to be, that is the question.", 389213881906ul)]
    public void FromDefinition_produces_platform_pinned_id(string definition, ulong expectedId)
    {
        var actual = SentenceMeaningId.FromDefinition(definition.AsSpan());
        Assert.Equal(expectedId, actual.ToUInt64());
    }
}
