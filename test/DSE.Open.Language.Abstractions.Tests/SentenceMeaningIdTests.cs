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
}
