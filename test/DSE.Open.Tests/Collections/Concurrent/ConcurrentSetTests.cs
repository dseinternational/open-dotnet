// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Concurrent;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Concurrent;

public class ConcurrentSetTests
{
    [Fact]
    public void ContainsAddedNotRemoved()
    {
        var set = new ConcurrentSet<int>();

        var values = Enumerable.Range(0, 1000);

        _ = Parallel.ForEach(values, i =>
        {
            _ = set.Add(i);
            Assert.True(set.Contains(i), $"Failed added: {i}");

            _ = set.TryRemove(i);
            Assert.False(set.Contains(i), $"Failed removed: {i}");
        });
    }

    [Fact]
    public void Iterate()
    {
        var set = new ConcurrentSet<int>();

        var values = Enumerable.Range(0, 1000);

        set.AddRange(values);

        _ = Parallel.ForEach(values, i =>
        {
            foreach (var value in set)
            {
                Assert.True(set.Contains(value), $"Failed: {i}");
            }
        });
    }

    [Fact]
    public void AddRange()
    {
        var set = new ConcurrentSet<int>();

        var values = Enumerable.Range(0, 1000);

        set.AddRange(values);

        Assert.Equal(1000, set.Count);
    }
}
