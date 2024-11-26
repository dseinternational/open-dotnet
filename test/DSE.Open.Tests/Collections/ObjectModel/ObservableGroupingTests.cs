// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.Collections.ObjectModel;

public class ObservableGroupingTests
{
    [Fact]
    public void Constructor_Group_Assigned_Empty_Collection()
    {
        var grouping = new ObservableGrouping<string, string>("Test");
        Assert.Equal("Test", grouping.Group);
        Assert.Empty(grouping);
    }

    [Fact]
    public void Constructor_Group_Collection_Assigned()
    {
        var grouping = new ObservableGrouping<string, string>("Test", ["One", "Two"]);
        Assert.Equal("Test", grouping.Group);
        Assert.Equal(2, grouping.Count);
        Assert.Equal("One", grouping[0]);
        Assert.Equal("Two", grouping[1]);
    }

    [Fact]
    public void Group_Set_Raises_PropertyChanged()
    {
        var grouping = new ObservableGrouping<string, string>("Test", ["One", "Two"]);
        var eventCount = 0;
        ((INotifyPropertyChanged)grouping).PropertyChanged += (_, e) =>
        {
            Assert.Equal("Group", e.PropertyName);
            eventCount++;
        };
        grouping.Group = "Test 2";
        Assert.Equal(1, eventCount);
    }
}
