// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;
using DSE.Open;

namespace DSE.Open.Collections.ObjectModel;

// Original from: https://github.com/dotnet/efcore/blob/main/test/EFCore.Tests/ChangeTracking/ObservableHashSetTest.cs
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

public class ObservableSetTest
{
    private static readonly Random s_random = new();

    [Fact]
    public void Can_construct()
    {
        Assert.Same(
            new HashSet<int>().Comparer,
            new ObservableSet<int>().Comparer);

        Assert.Same(
            ReferenceEqualityComparer.Instance,
            new ObservableSet<object>(ReferenceEqualityComparer.Instance).Comparer);

        var testData1 = CreateTestData();

        var rh1 = new HashSet<int>(testData1);
        var ohs1 = new ObservableSet<int>(testData1);
        Assert.Equal(rh1.OrderBy(i => i), ohs1.OrderBy(i => i));
        Assert.Same(rh1.Comparer, ohs1.Comparer);

        var testData2 = CreateTestData().Cast<object>();

        var rh2 = new HashSet<object>(testData2, ReferenceEqualityComparer.Instance);
        var ohs2 = new ObservableSet<object>(testData2, ReferenceEqualityComparer.Instance);
        Assert.Equal(rh2.OrderBy(i => i), ohs2.OrderBy(i => i));
        Assert.Same(rh2.Comparer, ohs2.Comparer);
    }

    [Fact]
    public void Can_add()
    {
        var hashSet = new ObservableSet<string>();
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 0;
        var countChange = 1;
        string[] adding = [];

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Add, a.Action);
            Assert.Null(a.OldItems);
            Assert.Equal(adding, a.NewItems?.OfType<string>());
            collectionChanged++;
        };

        adding = ["Palmer"];
        Assert.True(hashSet.Add("Palmer"));

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(new[] { "Palmer" }, hashSet);

        adding = ["Carmack"];
        Assert.True(hashSet.Add("Carmack"));

        Assert.Equal(2, countChanging);
        Assert.Equal(2, countChanged);
        Assert.Equal(2, collectionChanged);
        Assert.Equal(["Carmack", "Palmer"], hashSet.OrderBy(i => i));

        Assert.False(hashSet.Add("Palmer"));

        Assert.Equal(2, countChanging);
        Assert.Equal(2, countChanged);
        Assert.Equal(2, collectionChanged);
        Assert.Equal(["Carmack", "Palmer"], hashSet.OrderBy(i => i));
    }

    [Fact]
    public void Can_clear()
    {
        var testData = new HashSet<int>(CreateTestData());

        var hashSet = new ObservableSet<int>(testData);
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = testData.Count;
        var countChange = -testData.Count;

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Equal(testData.OrderBy(i => i), a.OldItems?.OfType<int>().OrderBy(i => i));
            Assert.Empty(a.NewItems!);
            collectionChanged++;
        };

        hashSet.Clear();

        Assert.Equal(testData.Count == 0 ? 0 : 1, countChanging);
        Assert.Equal(testData.Count == 0 ? 0 : 1, countChanged);
        Assert.Equal(testData.Count == 0 ? 0 : 1, collectionChanged);
        Assert.Empty(hashSet);

        hashSet.Clear();

        Assert.Equal(testData.Count == 0 ? 0 : 1, countChanging);
        Assert.Equal(testData.Count == 0 ? 0 : 1, countChanged);
        Assert.Equal(testData.Count == 0 ? 0 : 1, collectionChanged);
        Assert.Empty(hashSet);
    }

    [Fact]
    public void Contains_works()
    {
        var testData = CreateTestData();
        var hashSet = new ObservableSet<int>(testData);

        foreach (var item in testData)
        {
            Assert.Contains(item, hashSet);
        }

        foreach (var item in CreateTestData(1000, 10000).Except(testData))
        {
            Assert.DoesNotContain(item, hashSet);
        }
    }

    [Fact]
    public void Can_copy_to_array()
    {
        var testData = CreateTestData();
        var orderedDistinct = testData.Distinct().OrderBy(i => i).ToList();

        var hashSet = new ObservableSet<int>(testData);

        Assert.Equal(orderedDistinct.Count, hashSet.Count);

        var array = new int[hashSet.Count];
        hashSet.CopyTo(array);

        Assert.Equal(orderedDistinct, array.OrderBy(i => i));

        array = new int[hashSet.Count + 100];
        hashSet.CopyTo(array, 100);

        Assert.Equal(orderedDistinct, array.Skip(100).OrderBy(i => i));

        var toTake = Math.Min(10, hashSet.Count);
        array = new int[100 + toTake];
        hashSet.CopyTo(array, 100, toTake);

        foreach (var value in array.Skip(100).Take(toTake))
        {
            Assert.Contains(value, hashSet);
        }
    }

    [Fact]
    public void Can_remove()
    {
        var hashSet = new ObservableSet<string> { "Palmer", "Carmack" };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 2;
        var countChange = -1;
        string[] removing = [];

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Remove, a.Action);
            Assert.Equal(removing, a.OldItems!.OfType<string>());
            Assert.Null(a.NewItems);
            collectionChanged++;
        };

        removing = ["Palmer"];
        Assert.True(hashSet.Remove("Palmer"));

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(new[] { "Carmack" }, hashSet);

        removing = ["Carmack"];
        Assert.True(hashSet.Remove("Carmack"));

        Assert.Equal(2, countChanging);
        Assert.Equal(2, countChanged);
        Assert.Equal(2, collectionChanged);
        Assert.Empty(hashSet);

        Assert.False(hashSet.Remove("Palmer"));

        Assert.Equal(2, countChanging);
        Assert.Equal(2, countChanged);
        Assert.Equal(2, collectionChanged);
        Assert.Empty(hashSet);
    }

    [Fact]
    public void Not_read_only()
    {
        Assert.False(new ObservableSet<Random>().IsReadOnly);
    }

    [Fact]
    public void Can_union_with()
    {
        var hashSet = new ObservableSet<string> { "Palmer", "Carmack" };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 2;
        var countChange = 2;
        var adding = new[] { "Brendan", "Nate" };

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Empty(a.OldItems!);
            Assert.Equal(adding, a.NewItems!.OfType<string>().OrderBy(i => i));
            collectionChanged++;
        };

        hashSet.UnionWith(["Carmack", "Nate", "Brendan"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Carmack", "Nate", "Palmer"], hashSet.OrderBy(i => i));

        hashSet.UnionWith(["Brendan"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Carmack", "Nate", "Palmer"], hashSet.OrderBy(i => i));
    }

    [Fact]
    public void Can_intersect_with()
    {
        var hashSet = new ObservableSet<string>
        {
            "Brendan",
            "Carmack",
            "Nate",
            "Palmer"
        };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 4;
        var countChange = -2;
        var removing = new[] { "Brendan", "Nate" };

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Equal(removing, a.OldItems!.OfType<string>().OrderBy(i => i));
            Assert.Empty(a.NewItems!);
            collectionChanged++;
        };

        hashSet.IntersectWith(["Carmack", "Palmer", "Abrash"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Carmack", "Palmer"], hashSet.OrderBy(i => i));

        hashSet.IntersectWith(["Carmack", "Palmer", "Abrash"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Carmack", "Palmer"], hashSet.OrderBy(i => i));
    }

    [Fact]
    public void Can_except_with()
    {
        var hashSet = new ObservableSet<string>
        {
            "Brendan",
            "Carmack",
            "Nate",
            "Palmer"
        };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 4;
        var countChange = -2;
        var removing = new[] { "Carmack", "Palmer" };

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Equal(removing, a.OldItems!.OfType<string>().OrderBy(i => i));
            Assert.Empty(a.NewItems!);
            collectionChanged++;
        };

        hashSet.ExceptWith(["Carmack", "Palmer", "Abrash"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Nate"], hashSet.OrderBy(i => i));

        hashSet.ExceptWith(["Abrash", "Carmack", "Palmer"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Nate"], hashSet.OrderBy(i => i));
    }

    [Fact]
    public void Can_symmetrical_except_with()
    {
        var hashSet = new ObservableSet<string>
        {
            "Brendan",
            "Carmack",
            "Nate",
            "Palmer"
        };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 4;
        var countChange = -1;
        var removing = new[] { "Carmack", "Palmer" };
        var adding = new[] { "Abrash" };

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Equal(removing, a.OldItems!.OfType<string>().OrderBy(i => i));
            Assert.Equal(adding, a.NewItems!.OfType<string>().OrderBy(i => i));
            collectionChanged++;
        };

        hashSet.SymmetricExceptWith(["Carmack", "Palmer", "Abrash"]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Abrash", "Brendan", "Nate"], hashSet.OrderBy(i => i));

        hashSet.SymmetricExceptWith([]);

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Abrash", "Brendan", "Nate"], hashSet.OrderBy(i => i));
    }

    [Fact]
    public void IsSubsetOf_works_like_normal_hashset()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        Assert.Equal(
            new HashSet<int>(smallData).IsSubsetOf(bigData),
            new ObservableSet<int>(smallData).IsSubsetOf(bigData));
    }

    [Fact]
    public void IsProperSubsetOf_works_like_normal_hashset()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        Assert.Equal(
            new HashSet<int>(smallData).IsProperSubsetOf(bigData),
            new ObservableSet<int>(smallData).IsProperSubsetOf(bigData));
    }

    [Fact]
    public void IsSupersetOf_works_like_normal_hashset()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        Assert.Equal(
            new HashSet<int>(bigData).IsSupersetOf(smallData),
            new ObservableSet<int>(bigData).IsSupersetOf(smallData));
    }

    [Fact]
    public void IsProperSupersetOf_works_like_normal_hashset()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        Assert.Equal(
            new HashSet<int>(bigData).IsProperSupersetOf(smallData),
            new ObservableSet<int>(bigData).IsProperSupersetOf(smallData));
    }

    [Fact]
    public void Overlaps_works_like_normal_hashset()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        Assert.Equal(
            new HashSet<int>(bigData).Overlaps(smallData),
            new ObservableSet<int>(bigData).Overlaps(smallData));
    }

    [Fact]
    public void SetEquals_works_like_normal_hashset()
    {
        var data1 = CreateTestData(5);
        var data2 = CreateTestData(5);

        Assert.Equal(
            new HashSet<int>(data1).SetEquals(data2),
            new ObservableSet<int>(data1).SetEquals(data2));
    }

    [Fact]
    public void TrimExcess_doesnt_throw()
    {
        var bigData = CreateTestData();
        var smallData = CreateTestData(10);

        var hashSet = new ObservableSet<int>(bigData.Concat(smallData));
        foreach (var item in bigData)
        {
            _ = hashSet.Remove(item);
        }

        hashSet.TrimExcess();
    }

    [Fact]
    public void Can_remove_with_predicate()
    {
        var hashSet = new ObservableSet<string>
        {
            "Brendan",
            "Carmack",
            "Nate",
            "Palmer"
        };
        var countChanging = 0;
        var countChanged = 0;
        var collectionChanged = 0;
        var currentCount = 4;
        var countChange = -2;
        var removing = new[] { "Carmack", "Palmer" };

        hashSet.PropertyChanging += (s, a) => AssertCountChanging(hashSet, s!, a, currentCount, ref countChanging);
        hashSet.PropertyChanged += (s, a) => AssertCountChanged(hashSet, s!, a, ref currentCount, countChange, ref countChanged);
        hashSet.CollectionChanged += (s, a) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, a.Action);
            Assert.Equal(removing, a.OldItems!.OfType<string>().OrderBy(i => i));
            Assert.Empty(a.NewItems!);
            collectionChanged++;
        };

        Assert.Equal(2, hashSet.RemoveWhere(i => i.Contains('m', StringComparison.Ordinal)));

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Nate"], hashSet.OrderBy(i => i));

        Assert.Equal(0, hashSet.RemoveWhere(i => i.Contains('m', StringComparison.Ordinal)));

        Assert.Equal(1, countChanging);
        Assert.Equal(1, countChanged);
        Assert.Equal(1, collectionChanged);
        Assert.Equal(["Brendan", "Nate"], hashSet.OrderBy(i => i));
    }

    private static void AssertCountChanging<T>(
        ObservableSet<T> hashSet,
        object sender,
        PropertyChangingEventArgs eventArgs,
        int expectedCount,
        ref int changingCount)
    {
        Assert.Same(hashSet, sender);
        Assert.Equal("Count", eventArgs.PropertyName);
        Assert.Equal(expectedCount, hashSet.Count);
        changingCount++;
    }

    private static void AssertCountChanged<T>(
        ObservableSet<T> hashSet,
        object sender,
        PropertyChangedEventArgs eventArgs,
        ref int expectedCount,
        int countDelta,
        ref int changedCount)
    {
        Assert.Same(hashSet, sender);
        Assert.Equal("Count", eventArgs.PropertyName);
        Assert.Equal(expectedCount + countDelta, hashSet.Count);
        changedCount++;
        expectedCount += countDelta;
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    private static List<int> CreateTestData(int minSize = 0, int maxLength = 1000)
    {
        var length = s_random.Next(minSize, maxLength);
        var data = new List<int>();
        for (var i = 0; i < length; i++)
        {
            data.Add(s_random.Next(int.MinValue, int.MaxValue));
        }

        return data;
    }
#pragma warning restore CA5394 // Do not use insecure randomness
}
