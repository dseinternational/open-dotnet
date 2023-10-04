// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;
using FluentAssertions;

namespace DSE.Open.Results.Tests;

public class PaginatedCollectionValueResultBuilderTests
{
    [Fact]
    public void Build()
    {
        var builder = new PaginatedCollectionValueResultBuilder<string>();

        builder.Items.Add("Test1");
        builder.Items.Add("Test2");
        builder.Items.Add("Test3");
        builder.Notifications.Add(Notification.Information("NTF123456", "Information"));
        builder.Notifications.Add(Notification.Warning("NTF123456", "Warning"));
        builder.Notifications.Add(Notification.Error("NTF123456", "Error"));
        builder.Pagination = new Pagination(13, 10, 2);

        var val1 = builder.GetResult();

        var val2 = new PaginatedCollectionValueResult<string>
        {
            Value = new[] { "Test1", "Test2", "Test3" },
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ],
            Pagination = new Pagination(13, 10, 2)
        };

        _ = val1.Should().BeEquivalentTo(val2, config => config
            .ComparingByMembers<PaginatedCollectionValueResult<string>>()
            .Excluding(e => e.Id));
    }
}
