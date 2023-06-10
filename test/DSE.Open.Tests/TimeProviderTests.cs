// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using DSE.Open.Threading;

namespace DSE.Open.Tests;

// To be removed when .NET 8.0 is released.

public class TimeProviderTests
{
    [Fact]
    public void TestUtcSystemTime()
    {
        var dto1 = DateTimeOffset.UtcNow;
        var providerDto = TimeProvider.System.GetUtcNow();
        var dto2 = DateTimeOffset.UtcNow;

        Assert.InRange(providerDto.Ticks, dto1.Ticks, dto2.Ticks);
        Assert.Equal(TimeSpan.Zero, providerDto.Offset);
    }

    [Fact]
    public void TestLocalSystemTime()
    {
        var dto1 = DateTimeOffset.Now;
        var providerDto = TimeProvider.System.GetLocalNow();
        var dto2 = DateTimeOffset.Now;

        // Ensure there was no daylight saving shift during the test execution.
        if (dto1.Offset == dto2.Offset)
        {
            Assert.InRange(providerDto.Ticks, dto1.Ticks, dto2.Ticks);
            Assert.Equal(dto1.Offset, providerDto.Offset);
        }
    }

    private sealed class ZonedTimeProvider : TimeProvider
    {
        private readonly TimeZoneInfo _zoneInfo;

        public ZonedTimeProvider(TimeZoneInfo zoneInfo)
        {
            _zoneInfo = zoneInfo;
        }

        public override TimeZoneInfo LocalTimeZone => _zoneInfo;

        public static TimeProvider FromLocalTimeZone(TimeZoneInfo zoneInfo) => new ZonedTimeProvider(zoneInfo);
    }

    [Fact]
    public void TestSystemProviderWithTimeZone()
    {
        Assert.Equal(TimeZoneInfo.Local.Id, TimeProvider.System.LocalTimeZone.Id);

        var tzi = TimeZoneInfo.FindSystemTimeZoneById(OperatingSystem.IsWindows() ? "Pacific Standard Time" : "America/Los_Angeles");

        var tp = ZonedTimeProvider.FromLocalTimeZone(tzi);
        Assert.Equal(tzi.Id, tp.LocalTimeZone.Id);

        var utcDto1 = DateTimeOffset.UtcNow;
        var localDto = tp.GetLocalNow();
        var utcDto2 = DateTimeOffset.UtcNow;

        var utcConvertedDto = TimeZoneInfo.ConvertTime(localDto, TimeZoneInfo.Utc);
        Assert.InRange(utcConvertedDto.Ticks, utcDto1.Ticks, utcDto2.Ticks);
    }

    public static TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
                    Stopwatch.GetElapsedTime(startingTimestamp, endingTimestamp);

    [Fact]
    public void TestSystemTimestamp()
    {
        var timestamp1 = Stopwatch.GetTimestamp();
        var providerTimestamp1 = TimeProvider.System.GetTimestamp();
        var timestamp2 = Stopwatch.GetTimestamp();
        Thread.Sleep(100);
        var providerTimestamp2 = TimeProvider.System.GetTimestamp();

        Assert.InRange(providerTimestamp1, timestamp1, timestamp2);
        Assert.True(providerTimestamp2 > timestamp2);
        Assert.Equal(GetElapsedTime(providerTimestamp1, providerTimestamp2), TimeProvider.System.GetElapsedTime(providerTimestamp1, providerTimestamp2));

        var timestamp = TimeProvider.System.GetTimestamp();
        var period1 = TimeProvider.System.GetElapsedTime(timestamp);
        var period2 = TimeProvider.System.GetElapsedTime(timestamp);
        Assert.True(period1 <= period2);

        Assert.Equal(Stopwatch.Frequency, TimeProvider.System.TimestampFrequency);
    }

    public static IEnumerable<object[]> TimersProvidersData()
    {
        yield return new object[] { TimeProvider.System, 6000 };
        yield return new object[] { new FastClock(), 3000 };
    }

    [Fact]
    public void FastClockTest()
    {
        var fastClock = new FastClock();

        for (var i = 0; i < 20; i++)
        {
            var fastNow = fastClock.GetUtcNow();
            var now = DateTimeOffset.UtcNow;

            Assert.True(fastNow > now, $"Expected {fastNow} > {now}");

            fastNow = fastClock.GetLocalNow();
            now = DateTimeOffset.Now;

            Assert.True(fastNow > now, $"Expected {fastNow} > {now}");
        }

        Assert.Equal(TimeSpan.TicksPerSecond, fastClock.TimestampFrequency);

        var stamp1 = fastClock.GetTimestamp();
        var stamp2 = fastClock.GetTimestamp();

        Assert.Equal(stamp2 - stamp1, fastClock.GetElapsedTime(stamp1, stamp2).Ticks);
    }

    public static IEnumerable<object[]> TimersProvidersListData()
    {
        yield return new object[] { TimeProvider.System };
        yield return new object[] { new FastClock() };
    }

    [Fact]
    public static void NegativeTests()
    {
        var clock = new FastClock(-1);  // negative frequency
        _ = Assert.Throws<InvalidOperationException>(() => clock.GetElapsedTime(1, 2));
        clock = new FastClock(0); // zero frequency
        _ = Assert.Throws<InvalidOperationException>(() => clock.GetElapsedTime(1, 2));

        _ = Assert.Throws<ArgumentNullException>(() => TimeProvider.System.CreateTimer(null!, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => TimeProvider.System.CreateTimer(obj => { }, null, TimeSpan.FromMilliseconds(-2), Timeout.InfiniteTimeSpan));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => TimeProvider.System.CreateTimer(obj => { }, null, Timeout.InfiniteTimeSpan, TimeSpan.FromMilliseconds(-2)));
    }

    // Clock that speeds up the reported time
    private sealed class FastClock : TimeProvider
    {
        private long _minutesToAdd;
        private readonly TimeZoneInfo _zone;
        private readonly long _timestampFrequency;

        public FastClock(long timestampFrequency = TimeSpan.TicksPerSecond, TimeZoneInfo? zone = null)
        {
            _timestampFrequency = timestampFrequency;
            _zone = zone ?? TimeZoneInfo.Local;
        }

        public override DateTimeOffset GetUtcNow()
        {
            var now = DateTimeOffset.UtcNow;

            _minutesToAdd++;
            var remainingTicks = DateTimeOffset.MaxValue.Ticks - now.Ticks;

            if (_minutesToAdd * TimeSpan.TicksPerMinute > remainingTicks)
            {
                _minutesToAdd = 0;
                return now;
            }

            return now.AddMinutes(_minutesToAdd);
        }

        public override long TimestampFrequency => _timestampFrequency;

        public override TimeZoneInfo LocalTimeZone => _zone;

        public override long GetTimestamp() => GetUtcNow().Ticks;

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period) =>
            new FastTimer(callback, state, dueTime, period);
    }

    // Timer that fire faster
    private sealed class FastTimer : ITimer, IDisposable
    {
        private readonly Timer _timer;

        public FastTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            if (dueTime != Timeout.InfiniteTimeSpan)
            {
                dueTime = new TimeSpan(dueTime.Ticks / 2);
            }

            if (period != Timeout.InfiniteTimeSpan)
            {
                period = new TimeSpan(period.Ticks / 2);
            }

            _timer = new Timer(callback, state, dueTime, period);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            if (dueTime != Timeout.InfiniteTimeSpan)
            {
                dueTime = new TimeSpan(dueTime.Ticks / 2);
            }

            if (period != Timeout.InfiniteTimeSpan)
            {
                period = new TimeSpan(period.Ticks / 2);
            }

            return _timer.Change(dueTime, period);
        }

        public void Dispose() => _timer.Dispose();

        public ValueTask DisposeAsync() => _timer.DisposeAsync();
    }
}
