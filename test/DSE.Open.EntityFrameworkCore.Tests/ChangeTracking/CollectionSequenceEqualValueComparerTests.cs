// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Tests.ChangeTracking;

public class CollectionSequenceEqualValueComparerTests
{
    [Fact]
    public void SequenceEqualCollectionsAreEqual()
    {
        var comparer = new CollectionSequenceEqualValueComparer<int>();

        List<int> l1 = [1, 2, 3, 4, 5, 6];
        List<int> l2 = [1, 2, 3, 4, 5, 6];

        Assert.True(comparer.Equals(l1, l2));
    }

    [Fact]
    public void SnapshotIsEqual()
    {
        var comparer = new CollectionSequenceEqualValueComparer<int>();

        List<int> l1 = [1, 2, 3, 4, 5, 6];
        var l2 = (List<int>)comparer.Snapshot(l1);

        Assert.True(comparer.Equals(l1, l2));
    }

    [Fact]
    public void HashCodeIsEqual()
    {
        var comparer = new CollectionSequenceEqualValueComparer<int>();

        List<int> l1 = [1, 2, 3, 4, 5, 6];
        List<int> l2 = [1, 2, 3, 4, 5, 6];

        var h1 = comparer.GetHashCode(l1);
        var h2 = comparer.GetHashCode(l2);

        Assert.Equal(h1, h2);
    }
}
