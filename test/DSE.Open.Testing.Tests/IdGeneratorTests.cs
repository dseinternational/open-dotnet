// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

public class IdGeneratorTests
{
    [Fact]
    public void GetInt32Id_ReturnsValueAtLeastMinimum()
    {
        var id = IdGenerator.GetInt32Id();
        Assert.True(id >= 1);
    }

    [Fact]
    public void GetInt32Id_ReturnsMonotonicallyIncreasingValues()
    {
        const int count = 100;
        var ids = new int[count];
        for (var i = 0; i < count; i++)
        {
            ids[i] = IdGenerator.GetInt32Id();
        }

        for (var i = 1; i < count; i++)
        {
            Assert.True(ids[i] > ids[i - 1], $"ids[{i}]={ids[i]} should be greater than ids[{i - 1}]={ids[i - 1]}");
        }
    }

    [Fact]
    public void GetInt32Id_ReturnsUniqueValues()
    {
        const int count = 1000;
        var ids = new HashSet<int>();
        for (var i = 0; i < count; i++)
        {
            Assert.True(ids.Add(IdGenerator.GetInt32Id()));
        }
    }

    [Fact]
    public void GetInt32Id_RespectsMinimum()
    {
        var id = IdGenerator.GetInt32Id(1_000_000_000);
        Assert.True(id >= 1_000_000_000);
    }

    [Fact]
    public void GetInt32Id_NegativeMinimum_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => IdGenerator.GetInt32Id(-1));
    }

    [Fact]
    public void GetInt64Id_ReturnsValueAtLeastMinimum()
    {
        var id = IdGenerator.GetInt64Id();
        Assert.True(id >= 1);
    }

    [Fact]
    public void GetInt64Id_ReturnsMonotonicallyIncreasingValues()
    {
        const int count = 100;
        var ids = new long[count];
        for (var i = 0; i < count; i++)
        {
            ids[i] = IdGenerator.GetInt64Id();
        }

        for (var i = 1; i < count; i++)
        {
            Assert.True(ids[i] > ids[i - 1]);
        }
    }

    [Fact]
    public void GetInt64Id_ReturnsUniqueValues()
    {
        const int count = 1000;
        var ids = new HashSet<long>();
        for (var i = 0; i < count; i++)
        {
            Assert.True(ids.Add(IdGenerator.GetInt64Id()));
        }
    }

    [Fact]
    public void GetInt64Id_RespectsMinimum()
    {
        var id = IdGenerator.GetInt64Id(100_000_000_000L);
        Assert.True(id >= 100_000_000_000L);
    }

    [Fact]
    public void GetInt64Id_NegativeMinimum_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => IdGenerator.GetInt64Id(-1));
    }

    [Fact]
    public void GetRandomHexString_DefaultLength_Returns32LowercaseHexChars()
    {
        var hex = IdGenerator.GetRandomHexString();
        Assert.Equal(32, hex.Length);
        Assert.All(hex, c => Assert.True(IsLowerHexChar(c)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(8)]
    [InlineData(64)]
    public void GetRandomHexString_HonoursLengthInBytes(int lengthInBytes)
    {
        var hex = IdGenerator.GetRandomHexString(lengthInBytes);
        Assert.Equal(lengthInBytes * 2, hex.Length);
        Assert.All(hex, c => Assert.True(IsLowerHexChar(c)));
    }

    [Fact]
    public void GetRandomHexString_ProducesDifferentValues()
    {
        var a = IdGenerator.GetRandomHexString();
        var b = IdGenerator.GetRandomHexString();
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void GetRandomHexString_NegativeLength_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => IdGenerator.GetRandomHexString(-1));
    }

    [Fact]
    public async Task GetInt32Id_IsThreadSafe()
    {
        const int perTask = 500;
        const int taskCount = 8;

        var tasks = new Task<int[]>[taskCount];
        for (var t = 0; t < taskCount; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                var results = new int[perTask];
                for (var i = 0; i < perTask; i++)
                {
                    results[i] = IdGenerator.GetInt32Id();
                }
                return results;
            });
        }

        var all = (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
        Assert.Equal(taskCount * perTask, all.Distinct().Count());
    }

    private static bool IsLowerHexChar(char c)
    {
        return c is (>= '0' and <= '9') or (>= 'a' and <= 'f');
    }
}
