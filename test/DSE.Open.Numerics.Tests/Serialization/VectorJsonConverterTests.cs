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
    public void DeserializeNumericVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"int32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int32\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeNumericVectorOfInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<int>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<NumericVector<int>>(serialized, s_jsonOptions.Value);
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
    public void DeserializeNumericVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"int64\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"int64\",\"length\":6,\"values\":[0,1,2,5,-9,-283590]}")]
    public void DeserializeSerializeNumericVectorOfInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<long>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<NumericVector<long>>(serialized, s_jsonOptions.Value);
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
    public void DeserializeNumericVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }

    [Theory]
    [InlineData("{\"dtype\":\"uint32\",\"length\":0,\"values\":[]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":1,\"values\":[0]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":2,\"values\":[0,1]}")]
    [InlineData("{\"dtype\":\"uint32\",\"length\":6,\"values\":[0,1,2,5,9,283590]}")]
    public void DeserializeSerializeNumericVectorOfUInt32(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<uint>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, s_jsonOptions.Value);
        Assert.NotNull(serialized);
        Assert.Equal(json, serialized);

        var deserialized2 = JsonSerializer.Deserialize<NumericVector<uint>>(serialized, s_jsonOptions.Value);
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
    public void DeserializeNumericVectorOfUInt64(string json)
    {
        var deserialized = JsonSerializer.Deserialize<NumericVector<ulong>>(json, s_jsonOptions.Value);
        Assert.NotNull(deserialized);
    }
}
