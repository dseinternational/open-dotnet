// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values.TestValues;

namespace DSE.Open.Values;

public class SerializableValueTests
{
    [Fact]
    public void Serialize_value()
    {
        var v1 = BinaryInt32Value.False;
        var json = JsonSerializer.Serialize(v1);
        var json2 = JsonSerializer.Serialize((int)v1);
        Assert.Equal(json, json2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var v1 = BinaryInt32Value.False;
        var json = JsonSerializer.Serialize(v1);
        var v2 = JsonSerializer.Deserialize<BinaryInt32Value>(json);
        Assert.Equal(v1, v2);
    }
}
