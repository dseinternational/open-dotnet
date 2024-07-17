// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;

namespace DSE.Open.Results.Tests;

public class ValueResultBuilderTests
{
    [Fact]
    public void GetResult()
    {
        var builder = new ValueResultBuilder<string>();
        builder.Notifications.AddError((Diagnostics.DiagnosticCode)"TEST123456", "Test");
        var result = builder.Build();
        Assert.True(result.HasNotifications);
        Assert.True(result.Notifications.Count == 1);
    }
}
