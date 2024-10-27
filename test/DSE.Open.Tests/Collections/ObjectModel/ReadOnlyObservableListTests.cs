// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.ObjectModel;

namespace DSE.Open.Tests.Collections.ObjectModel;

public class ReadOnlyObservableListTests
{
    [Fact]
    public void Initialise()
    {
        var list = new ReadOnlyObservableList<string>();
        Assert.Empty(list);
    }

    [Fact]
    public void InitialiseWithCollection()
    {
        var items = new[] { "one", "two", "three", "four" };

        var list = new ObservableList<string>(items).AsReadOnlyObservableList();

        Assert.Equal(4, list.Count);
        Assert.Equal("one", list[0]);
        Assert.Equal("two", list[1]);
        Assert.Equal("three", list[2]);
        Assert.Equal("four", list[3]);
    }

    [Fact]
    public void AddToWrappedAddsItemAndRaisesCollectionChangedEvent()
    {
        var eventCount = 0;

        var list = new ObservableList<string>();

        var readOnlyList = list.AsReadOnlyObservableList();

        readOnlyList.CollectionChanged += (s, e) => eventCount++;

        list.Add("one");
        list.Add("two");
        list.Add("three");
        list.Add("four");

        Assert.Equal(eventCount, readOnlyList.Count);
        Assert.Equal("one", readOnlyList[0]);
        Assert.Equal("two", readOnlyList[1]);
        Assert.Equal("three", readOnlyList[2]);
        Assert.Equal("four", readOnlyList[3]);
    }
}
