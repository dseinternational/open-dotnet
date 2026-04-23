// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Requests.Tests;

public class RequestMetadataTests
{
    [Fact]
    public void Id_LazilyGenerated_OnFirstRead()
    {
        var metadata = new RequestMetadata();

        var first = metadata.Id;
        var second = metadata.Id;

        Assert.NotEqual(Guid.Empty, first);
        Assert.Equal(first, second);
    }

    [Fact]
    public void Id_TwoInstances_AreUnique()
    {
        var a = new RequestMetadata().Id;
        var b = new RequestMetadata().Id;
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Id_CanBeOverriddenViaInit()
    {
        var explicitId = Guid.NewGuid();
        var metadata = new RequestMetadata { Id = explicitId };
        Assert.Equal(explicitId, metadata.Id);
    }

    [Fact]
    public void Properties_DefaultsToEmpty()
    {
        var metadata = new RequestMetadata();
        Assert.Empty(metadata.Properties);
    }

    [Fact]
    public void Properties_KeysAreCaseInsensitive_Ordinal()
    {
        var metadata = new RequestMetadata();
        _ = metadata.Properties.TryAdd("Tenant", "acme");

        Assert.True(metadata.Properties.ContainsKey("tenant"));
        Assert.True(metadata.Properties.ContainsKey("TENANT"));
    }

    [Fact]
    public void SerializeDeserialize_PreservesIdAndProperties()
    {
        var metadata = new RequestMetadata { Id = Guid.Parse("11111111-2222-3333-4444-555555555555") };
        _ = metadata.Properties.TryAdd("tenant", "acme");

        var json = JsonSerializer.Serialize(metadata);
        var deserialized = JsonSerializer.Deserialize<RequestMetadata>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(metadata.Id, deserialized.Id);
        Assert.True(deserialized.Properties.ContainsKey("tenant"));
    }
}
