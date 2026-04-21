// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainEventTests
{
    [Fact]
    public void Id_IsAssignedAtConstruction()
    {
        var ev = new DomainEventFake("Test");
        Assert.NotEqual(default, ev.Id);
    }

    [Fact]
    public void Id_IsStableAcrossReads()
    {
        var ev = new DomainEventFake("Test");
        var first = ev.Id;

        for (var i = 0; i < 100; i++)
        {
            Assert.Equal(first, ev.Id);
        }
    }

    [Fact]
    public async Task Id_IsStableAcrossConcurrentReads()
    {
        const int threadCount = 64;
        const int trials = 200;

        var ct = TestContext.Current.CancellationToken;

        for (var t = 0; t < trials; t++)
        {
            var ev = new DomainEventFake("Test");

            using var start = new Barrier(threadCount);
            var ids = new Identifier[threadCount];

            var tasks = new Task[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                var index = i;
                tasks[i] = Task.Run(() =>
                {
                    _ = start.SignalAndWait(TimeSpan.FromSeconds(5));
                    ids[index] = ev.Id;
                }, ct);
            }

            await Task.WhenAll(tasks).WaitAsync(ct);

            var expected = ids[0];
            for (var i = 1; i < threadCount; i++)
            {
                Assert.Equal(expected, ids[i]);
            }
        }
    }
}
