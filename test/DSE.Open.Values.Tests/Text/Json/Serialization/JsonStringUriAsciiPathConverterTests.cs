// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values.Tests.Text.Json.Serialization;

public class JsonStringUriAsciiPathConverterTests
{
    [Fact]
    public void Deserialize_RoundTrip()
    {
        var path = UriAsciiPath.Parse("path/to/page");

        var json = JsonSerializer.Serialize(path);
        var deserialized = JsonSerializer.Deserialize<UriAsciiPath>(json);

        Assert.Equal(path, deserialized);
    }
}
