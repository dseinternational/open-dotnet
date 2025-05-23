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
    [InlineData("{\"d\":\"int32\",\"v\":[]}")]
    [InlineData("{\"d\":\"int32\",\"v\":[0] }")]
    [InlineData("{\"d\":\"int32\",\"v\":[0,1] }")]
    [InlineData("{\"d\":\"int32\",\"v\":[0,1,2,5,-9,-283590]}")]
    [InlineData("{\"d\":\"int32\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"int32\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"int32\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"int32\",\"l\":6,\"v\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"d\":\"int32\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"int32\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"int32\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"int32\",\"l\":6,\"v\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Vector<int>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"d\":\"int64\",\"v\":[]}")]
    [InlineData("{\"d\":\"int64\",\"v\":[0]}")]
    [InlineData("{\"d\":\"int64\",\"v\":[0,1]}")]
    [InlineData("{\"d\":\"int64\",\"v\":[0,1,2,5,-9,-283590]}")]
    [InlineData("{\"d\":\"int64\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"int64\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"int64\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"int64\",\"l\":6,\"v\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"d\":\"int64\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"int64\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"int64\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"int64\",\"l\":6,\"v\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Vector<long>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"d\":\"uint32\",\"v\":[]}")]
    [InlineData("{\"d\":\"uint32\",\"v\":[0]}")]
    [InlineData("{\"d\":\"uint32\",\"v\":[0,1]}")]
    [InlineData("{\"d\":\"uint32\",\"v\":[0,1,2,5,9,283590]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":6,\"v\":[0,1,2,5,9,283590]}")]
    public void DeserializeVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"d\":\"uint32\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"uint32\",\"l\":6,\"v\":[0,1,2,5,9,283590]}")]
    public void DeserializeSerializeVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<Vector<uint>>(serialized, s_jsonOptions.Value);
        Assert.NotNull(deserialized2);

        Assert.True(deserialized.AsSpan().SequenceEqual(deserialized2.AsSpan()));
    }

    [Theory]
    [InlineData("{\"d\":\"uint64\",\"v\":[]}")]
    [InlineData("{\"d\":\"uint64\",\"v\":[0]}")]
    [InlineData("{\"d\":\"uint64\",\"v\":[0,1]}")]
    [InlineData("{\"d\":\"uint64\",\"v\":[0,1,2,5,9,283590]}")]
    [InlineData("{\"d\":\"uint64\",\"l\":0,\"v\":[]}")]
    [InlineData("{\"d\":\"uint64\",\"l\":1,\"v\":[0]}")]
    [InlineData("{\"d\":\"uint64\",\"l\":2,\"v\":[0,1]}")]
    [InlineData("{\"d\":\"uint64\",\"l\":6,\"v\":[0,1,2,5,9,283590]}")]
    public void DeserializeVectorOfUInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Vector<ulong>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }
}
