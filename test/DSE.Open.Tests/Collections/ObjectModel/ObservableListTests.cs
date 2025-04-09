// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.ObjectModel;

public sealed class ObservableListTests
{
    [Fact]
    public void Initialise()
    {
        var list = new ObservableList<string>();
        Assert.Empty(list);
    }

    [Fact]
    public void InitialiseWithCollection()
    {
        var items = new[] { "one", "two", "three", "four" };

        var list = new ObservableList<string>(items);

        Assert.Equal(4, list.Count);
        Assert.Equal("one", list[0]);
        Assert.Equal("two", list[1]);
        Assert.Equal("three", list[2]);
        Assert.Equal("four", list[3]);
    }

    [Fact]
    public void AddAddsItemAndRaisesCollectionChangedEvent()
    {
        var eventCount = 0;

        var list = new ObservableList<string>();

        list.CollectionChanged += (s, e) => eventCount++;

        list.Add("one");
        list.Add("two");
        list.Add("three");
        list.Add("four");

        Assert.Equal(eventCount, list.Count);
        Assert.Equal("one", list[0]);
        Assert.Equal("two", list[1]);
        Assert.Equal("three", list[2]);
        Assert.Equal("four", list[3]);
    }

    [Fact]
    public void Sort()
    {
        var items = new[] { "one", "two", "Three", "four" };

        var list = new ObservableList<string>(items);

        list.Sort(StringComparer.OrdinalIgnoreCase);

        Assert.Equal("four", list[0]);
        Assert.Equal("one", list[1]);
        Assert.Equal("Three", list[2]);
        Assert.Equal("two", list[3]);
    }

    [Fact]
    public void AddRange()
    {
        var items = new[] { "one", "two", "three", "four" };

        var list = new ObservableList<string>();

        list.AddRange(items);

        Assert.Equal(4, list.Count);
        Assert.Equal("one", list[0]);
        Assert.Equal("two", list[1]);
        Assert.Equal("three", list[2]);
        Assert.Equal("four", list[3]);
    }

    [Fact]
    public void RemoveRange()
    {
        var items = new[] { "one", "two", "three", "four" };

        var list = new ObservableList<string>();

        list.AddRange(items);

        list.RemoveRange(["one", "two"]);

        Assert.Equal(2, list.Count);
        Assert.Equal("three", list[0]);
        Assert.Equal("four", list[1]);
    }

    [Fact]
    public void SetRange_WithPreviouslyEmpty_ShouldAddAll()
    {
        // Arrange
        string[] items = ["one", "two", "three", "four"];
        var list = new ObservableList<string>();

        // Act
        list.SetRange(items);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.True(list.SequenceEqual(items));
    }

    [Fact]
    public void SetRange_WithPreviousNoOverlap_ShouldReplace()
    {
        // Arrange
        string[] items = ["one", "two", "three", "four"];
        var list = new ObservableList<string>(["five", "six"]);

        // Act
        list.SetRange(items);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.True(list.SequenceEqual(items));
    }

    [Fact]
    public void SetRange_WithPreviousOverlap_ShouldReplace()
    {
        // Arrange
        string[] items = ["one", "two", "three", "four"];
        var list = new ObservableList<string>(["one", "two", "five", "six"]);

        // Act
        list.SetRange(items);

        // Assert
        Assert.Equal(4, list.Count);
        Assert.True(list.SequenceEqual(items));
    }

    [Fact]
    public void SetRange_WithPreviousItemsAndEmpty_ShouldClear()
    {
        // Arrange
        string[] items = [];
        var list = new ObservableList<string>(["one", "two", "five", "six"]);

        // Act
        list.SetRange(items);

        // Assert
        Assert.Empty(list);
    }
}
