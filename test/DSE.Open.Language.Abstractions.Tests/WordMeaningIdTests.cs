// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language;

public sealed class WordMeaningIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (WordMeaningId)258089004501;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void GetRandomId_ReturnsValidValue()
    {
        for (var i = 0; i < 1000; i++)
        {
            var id = WordMeaningId.GetRandomId();
            Assert.True(WordMeaningId.IsValidValue((long)id));
        }
    }
}
