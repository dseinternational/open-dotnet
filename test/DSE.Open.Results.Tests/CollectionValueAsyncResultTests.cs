// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;
using AwesomeAssertions;

namespace DSE.Open.Results.Tests;

public class CollectionValueAsyncResultTests
{
    public CollectionValueAsyncResultTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    private static readonly string[] s_sourceArray = ["Test1", "Test2", "Test3"];

    [Fact]
    public void Equality()
    {
        var val1 = new CollectionValueAsyncResult<string>
        {
            Value = s_sourceArray.ToAsyncEnumerable(),
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        var val2 = new CollectionValueAsyncResult<string>
        {
            Value = s_sourceArray.ToAsyncEnumerable(),
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        _ = val2.Should().BeEquivalentTo(val1, config => config
            .ComparingByMembers<CollectionValueAsyncResult<string>>()
            .Excluding(e => e.ResultId));
    }
}
