// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Results.Tests;

public class ResultMetadataTests
{
    [Fact]
    public void Id_LazilyGenerated_OnFirstRead()
    {
        var metadata = new ResultMetadata();

        var first = metadata.Id;
        var second = metadata.Id;

        Assert.NotEqual(Guid.Empty, first);
        Assert.Equal(first, second);
    }

    [Fact]
    public void Id_TwoInstances_AreUnique()
    {
        var a = new ResultMetadata().Id;
        var b = new ResultMetadata().Id;
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Id_CanBeOverriddenViaInit()
    {
        var explicitId = Guid.NewGuid();
        var metadata = new ResultMetadata { Id = explicitId };
        Assert.Equal(explicitId, metadata.Id);
    }

    [Fact]
    public void Properties_DefaultsToEmpty()
    {
        var metadata = new ResultMetadata();
        Assert.Empty(metadata.Properties);
    }

    [Fact]
    public void Properties_KeysAreCaseInsensitive_Ordinal()
    {
        var metadata = new ResultMetadata();
        _ = metadata.Properties.TryAdd("Tenant", "acme");

        Assert.True(metadata.Properties.ContainsKey("tenant"));
        Assert.True(metadata.Properties.ContainsKey("TENANT"));
    }

    [Fact]
    public void SerializeDeserialize_PreservesId()
    {
        var metadata = new ResultMetadata { Id = Guid.Parse("11111111-2222-3333-4444-555555555555") };

        var json = JsonSerializer.Serialize(metadata);
        var deserialized = JsonSerializer.Deserialize<ResultMetadata>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(metadata.Id, deserialized.Id);
    }
}
