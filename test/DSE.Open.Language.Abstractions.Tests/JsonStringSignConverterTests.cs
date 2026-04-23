// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Language.Serialization;

namespace DSE.Open.Language;

public sealed class JsonStringSignConverterTests
{
    [Fact]
    public void Default_returns_cached_singleton()
    {
        Assert.Same(JsonStringSignConverter.Default, JsonStringSignConverter.Default);
    }

    [Fact]
    public void Serializes_sign_as_json_string()
    {
        var sign = new Sign(SignModality.Spoken, new WordText("run"));
        var json = JsonSerializer.Serialize(sign);
        Assert.Equal("\"spoken:run\"", json);
    }

    [Fact]
    public void Deserializes_sign_from_json_string()
    {
        var sign = JsonSerializer.Deserialize<Sign>("\"written:book\"");
        Assert.Equal(new Sign(SignModality.Written, new WordText("book")), sign);
    }

    [Fact]
    public void Roundtrips_via_json()
    {
        var sign = new Sign(SignModality.Gestured, new WordText("more"));
        var json = JsonSerializer.Serialize(sign);
        var actual = JsonSerializer.Deserialize<Sign>(json);
        Assert.Equal(sign, actual);
    }

    [Fact]
    public void Throws_on_malformed_json_string()
    {
        _ = Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<Sign>("\"not-a-sign\""));
    }
}
