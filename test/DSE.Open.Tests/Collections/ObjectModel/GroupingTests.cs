// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.ObjectModel;

namespace DSE.Open.Tests.Collections.ObjectModel;

public class GroupingTests
{
    [Fact]
    public void Constructor_Group_Assigned_Empty_Collection()
    {
        var grouping = new Grouping<string, string>("Test");
        Assert.Equal("Test", grouping.Group);
        Assert.Empty(grouping);
    }

    [Fact]
    public void Constructor_Group_Collection_Assigned()
    {
        var grouping = new Grouping<string, string>("Test", new[] { "One", "Two" });
        Assert.Equal("Test", grouping.Group);
        Assert.Equal(2, grouping.Count);
        Assert.Equal("One", grouping[0]);
        Assert.Equal("Two", grouping[1]);
    }
}
