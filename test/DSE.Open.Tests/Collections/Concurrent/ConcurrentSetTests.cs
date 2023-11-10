// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Concurrent;

namespace DSE.Open.Tests.Collections.Concurrent;

public class ConcurrentSetTests
{
    [Fact]
    public void ContainsAddedNotRemoved()
    {
        var set = new ConcurrentSet<int>();

        var values = Enumerable.Range(0, 1000);

        _ = Parallel.ForEach(values, (i) =>
        {
            _ = set.Add(i);
            Assert.True(set.Contains(i), $"Failed added: {i}");

            set.TryRemove(i);
            Assert.False(set.Contains(i), $"Failed removed: {i}");
        });
    }
}
