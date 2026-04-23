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
        // LossyPayload.Secret is decorated with [JsonIgnore], so it is dropped during
        // serialisation.  AssertJson.Roundtrip must detect the mismatch and throw.
        _ = Assert.ThrowsAny<XunitException>(
            () => AssertJson.Roundtrip(new LossyPayload { Id = 1, Secret = "secret" }));
    }

    internal sealed class LossyPayload
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string Secret { get; set; } = string.Empty;
    }
}
