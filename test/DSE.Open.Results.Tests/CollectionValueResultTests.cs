// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Notifications;
using DSE.Open.Text.Json;
using FluentAssertions;

namespace DSE.Open.Results.Tests;

public class CollectionValueResultTests
{
    public CollectionValueResultTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void Serialize_Deserialize()
    {
        var val = new CollectionValueResult<string>
        {
            Value = new[] { "Test1", "Test2", "Test3" },
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        var json = JsonSerializer.Serialize(val, JsonSharedOptions.RelaxedJsonEscaping);

        Output.WriteLine(json);

        var val2 = JsonSerializer.Deserialize<CollectionValueResult<string>>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(val2);

        _ = val2.Should().BeEquivalentTo(val, config => config
            .ComparingByMembers<CollectionValueResult<string>>());
    }

    [Fact]
    public void Serialize_Deserialize_empty()
    {
        var val = CollectionValueResult<string>.Empty;

        var json = JsonSerializer.Serialize(val, JsonSharedOptions.RelaxedJsonEscaping);

        Output.WriteLine(json);

        var val2 = JsonSerializer.Deserialize<CollectionValueResult<string>>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(val2);

        _ = val2.Should().BeEquivalentTo(val, config => config
            .ComparingByMembers<CollectionValueResult<string>>());
    }
}
