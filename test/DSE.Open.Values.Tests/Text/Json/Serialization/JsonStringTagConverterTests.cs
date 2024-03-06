// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Values.Tests.Text.Json.Serialization;

public class JsonStringTagConverterTests
{
    [Theory]
    [InlineData("12")]
    [InlineData("ab")]
    [InlineData("a-longer-tag-with-more-parts-to-it")]
    public void SerializeDeserialize(string tagStr)
    {
        var tag = Tag.ParseInvariant(tagStr);

        var json = JsonSerializer.Serialize(tag, JsonSharedOptions.RelaxedJsonEscaping);

        var result = JsonSerializer.Deserialize<Tag>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.Equal(tag, result);
    }
}
