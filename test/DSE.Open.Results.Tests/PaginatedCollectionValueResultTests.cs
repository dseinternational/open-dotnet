// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Notifications;
using DSE.Open.Text.Json;
using AwesomeAssertions;

namespace DSE.Open.Results.Tests;

public class PaginatedCollectionValueResultTests
{
    public PaginatedCollectionValueResultTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void Serialize_Deserialize()
    {
        var val = new PaginatedCollectionValueResult<string>
        {
            Value = ["Test1", "Test2", "Test3"],
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ],
            Pagination = new(13, 10, 2)
        };

        var json = JsonSerializer.Serialize(val, JsonSharedOptions.RelaxedJsonEscaping);

        Output.WriteLine(json);

        var val2 = JsonSerializer.Deserialize<PaginatedCollectionValueResult<string>>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(val2);

        _ = val2.Should().BeEquivalentTo(val, config => config
            .ComparingByMembers<PaginatedCollectionValueResult<string>>());
    }
}
