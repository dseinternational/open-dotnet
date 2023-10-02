// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Tests.ChangeTracking;

public class ReadOnlySetEqualsValueComparerTests
{
    [Fact]
    public void SequenceEqualReadOnlySetsAreEqual()
    {
        var comparer = new ReadOnlySetEqualsValueComparer<int>();

        HashSet<int> l1 = [1, 2, 3, 4, 5, 6];
        HashSet<int> l2 = [6, 5, 4, 3, 2, 1];

        Assert.True(comparer.Equals(l1, l2));
    }

    [Fact]
    public void SnapshotIsEqual()
    {
        var comparer = new ReadOnlySetEqualsValueComparer<int>();

        HashSet<int> l1 = [1, 2, 3, 4, 5, 6];
        var l2 = (HashSet<int>)comparer.Snapshot(l1);

        Assert.True(comparer.Equals(l1, l2));
    }

    [Fact]
    public void HashCodeIsEqual()
    {
        var comparer = new ReadOnlySetEqualsValueComparer<int>();

        HashSet<int> l1 = [1, 2, 3, 4, 5, 6];
        HashSet<int> l2 = [6, 5, 4, 3, 2, 1];

        var h1 = comparer.GetHashCode(l1);
        var h2 = comparer.GetHashCode(l2);

        Assert.Equal(h1, h2);
    }
}
