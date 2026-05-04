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
        const int threadCount = 32;
        const int trials = 200;
        var barrierTimeout = TimeSpan.FromSeconds(2);

        var ct = TestContext.Current.CancellationToken;

        // LongRunning so each worker gets a dedicated thread rather than a pool
        // thread - blocking on the barrier would otherwise gate on thread-pool
        // injection. Workers are reused across trials so each trial only pays for
        // two barrier crossings. SignalAndWait carries a timeout so that a worker
        // exiting early (e.g. an assertion added inside the loop) fails loudly
        // rather than hanging until test-host cancellation.
        using var trialStart = new Barrier(threadCount + 1);
        using var trialDone = new Barrier(threadCount + 1);

        DomainEventFake current = null!;
        var observed = new Identifier[threadCount];

        var workers = new Task[threadCount];
        for (var i = 0; i < threadCount; i++)
        {
            var index = i;
            workers[i] = Task.Factory.StartNew(() =>
            {
                for (var t = 0; t < trials; t++)
                {
                    Assert.True(trialStart.SignalAndWait(barrierTimeout, ct));
                    observed[index] = current.Id;
                    Assert.True(trialDone.SignalAndWait(barrierTimeout, ct));
                }
            }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        for (var t = 0; t < trials; t++)
        {
            current = new DomainEventFake("Test");
            Assert.True(trialStart.SignalAndWait(barrierTimeout, ct));
            Assert.True(trialDone.SignalAndWait(barrierTimeout, ct));

            var expected = observed[0];
            for (var i = 1; i < threadCount; i++)
            {
                Assert.Equal(expected, observed[i]);
            }
        }

        await Task.WhenAll(workers).WaitAsync(ct);
    }
}
