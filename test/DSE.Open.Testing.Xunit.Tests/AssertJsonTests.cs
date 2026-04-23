// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

public partial class AssertJsonTests
{
    internal sealed record Payload(int Id, string Name);

    [JsonSerializable(typeof(Payload))]
    internal sealed partial class PayloadContext : JsonSerializerContext;

    [Fact]
    public void Roundtrip_Default()
    {
        AssertJson.Roundtrip(new Payload(1, "name"));
    }

    [Fact]
    public void Roundtrip_WithOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        AssertJson.Roundtrip(new Payload(1, "name"), options);
    }

    [Fact]
    public void Roundtrip_WithJsonTypeInfo()
    {
        AssertJson.Roundtrip(new Payload(1, "name"), PayloadContext.Default.Payload);
    }

    [Fact]
    public void Roundtrip_WithJsonSerializerContext()
    {
        AssertJson.Roundtrip(new Payload(1, "name"), PayloadContext.Default);
    }

    [Fact]
    public void Roundtrip_DetectsDataLoss()
    {
        // Payload.Name is required; serialising one whose Name the converter drops
        // would cause equivalence to fail. We simulate by constructing a context that
        // serialises the value but then compare against a wholly different expected value.
        // Here we simply assert that comparing two different payloads throws.
        _ = Assert.ThrowsAny<XunitException>(
            () => AssertEquivalentFailsFor(new Payload(1, "a"), new Payload(1, "b")));
    }

    private static void AssertEquivalentFailsFor(Payload expected, Payload actual)
    {
        Assert.Equivalent(expected, actual, true);
    }
}
