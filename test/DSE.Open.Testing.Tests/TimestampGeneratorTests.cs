// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

public class TimestampGeneratorTests
{
    [Fact]
    public void GetNext_ProducesNonEmptyTimestamp()
    {
        var t = TimestampGenerator.GetNext();
        Assert.NotEqual(Timestamp.Empty, t);
    }

    [Fact]
    public void GetNext_ReturnsUniqueValues()
    {
        const int count = 1000;
        var seen = new HashSet<Timestamp>();
        for (var i = 0; i < count; i++)
        {
            Assert.True(seen.Add(TimestampGenerator.GetNext()));
        }
    }

    [Fact]
    public async Task GetNext_IsThreadSafe()
    {
        const int perTask = 500;
        const int taskCount = 8;

        var tasks = new Task<Timestamp[]>[taskCount];
        for (var t = 0; t < taskCount; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                var results = new Timestamp[perTask];
                for (var i = 0; i < perTask; i++)
                {
                    results[i] = TimestampGenerator.GetNext();
                }
                return results;
            });
        }

        var all = (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
        Assert.Equal(taskCount * perTask, all.Distinct().Count());
    }

    [Fact]
    public void GetRandom_ProducesDifferentValuesAcrossCalls()
    {
        var a = TimestampGenerator.GetRandom();
        var b = TimestampGenerator.GetRandom();
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void GetRandom_ProducesEightByteTimestamp()
    {
        var t = TimestampGenerator.GetRandom();
        Assert.Equal(Timestamp.Size, t.GetBytes().Length);
    }
}
