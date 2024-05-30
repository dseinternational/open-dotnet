// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.ObjectModel;

namespace DSE.Open.Tests.Collections.ObjectModel;

public class ObservableKeyedCollectionTests
{
    [Fact]
    public void CountEqualsNumberOfItemsAdded()
    {
        var collection = CreateAndInitializeCollection(50);
        Assert.Equal(50, collection.Count);
    }

    private static TestObservableKeyedCollection CreateAndInitializeCollection(int count)
    {
        var collection = new TestObservableKeyedCollection();
        var items = CreateItems(count);
        foreach (var testItem in items)
        {
            collection.Add(testItem);
        }

        return collection;
    }

    private static List<TestItem> CreateItems(int count)
    {
        var items = new List<TestItem>();
        for (var i = 0; i < count; i++)
        {
            items.Add(new());
        }

        return items;
    }
}

internal sealed class TestObservableKeyedCollection : ObservableKeyedCollection<string, TestItem>
{
    protected override string GetKeyForItem(TestItem item)
    {
        return item.Key;
    }
}

internal sealed class TestItem
{
    public string Key { get; } = Guid.NewGuid().ToString();
}
