// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public class VectorJsonConverterTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        // options.AddDefaultNumericsJsonConverters();
        return options;
    });

    public VectorJsonConverterTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("{\"dtype\":\"int32\",\"values\":[]}")]
    [InlineData("{\"dtype\":\"int32\",\"values\":[0] }")]
    [InlineData("{\"dtype\":\"int32\",\"values\":[0,1] }")]
    [InlineData("{\"dtype\":\"int32\",\"values\":[0,1,2,5,-9,-283590]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"int32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Series<int>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"dtype\":\"int64\",\"values\":[]}")]
    [InlineData("{\"dtype\":\"int64\",\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int64\",\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int64\",\"values\":[0,1,2,5,-9,-283590]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"int64\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Series<long>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"dtype\":\"uint32\",\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint32\",\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint32\",\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint32\",\"values\":[0,1,2,5,9,283590]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":6,\"values\":[0,1,2,5,9,283590]}")]
    public void DeserializeVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"uint32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":6,\"values\":[0,1,2,5,9,283590]}")]
    public void DeserializeSerializeVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Series<uint>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"dtype\":\"uint64\",\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint64\",\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint64\",\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint64\",\"values\":[0,1,2,5,9,283590]}")]
    [InlineData("{\"dtype\":\"uint64\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint64\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint64\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint64\",\"length\":6,\"values\":[0,1,2,5,9,283590]}")]
    public void DeserializeVectorOfUInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Series<ulong>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }
}
