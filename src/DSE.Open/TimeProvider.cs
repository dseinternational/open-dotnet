// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using DSE.Open.Threading;

namespace DSE.Open;

#if !NET8_0_OR_GREATER

/// <summary>
/// Provides an abstraction for time.
/// </summary>
/// <remarks>
/// <para><b>Note:</b> this class will be available in .NET 8.0, at which time we will remove our version.</para>
/// </remarks>
public abstract class TimeProvider
{
    /// <summary>
    /// Gets a <see cref="TimeProvider"/> that provides a clock based on <see cref="DateTimeOffset.UtcNow"/>,
    /// a time zone based on <see cref="TimeZoneInfo.Local"/>, a high-performance time stamp based on
    /// <see cref="Stopwatch"/>, and a timer based on <see cref="Timer"/>.
    /// </summary>
    /// <remarks>
    /// If the <see cref="TimeZoneInfo.Local"/> changes after the object is returned, the change will be
    /// reflected in any subsequent operations that retrieve <see cref="GetLocalNow"/>.
    /// </remarks>
    public static TimeProvider System { get; } = new SystemTimeProvider();

    /// <summary>
    /// Initializes the <see cref="TimeProvider"/>.
    /// </summary>
    protected TimeProvider()
    {
    }

    /// <summary>
    /// Gets a <see cref="DateTimeOffset"/> value whose date and time are set to the current
    /// Coordinated Universal Time (UTC) date and time and whose offset is Zero,
    /// all according to this <see cref="TimeProvider"/>'s notion of time.
    /// </summary>
    /// <remarks>
    /// The default implementation returns <see cref="DateTimeOffset.UtcNow"/>.
    /// </remarks>
    public virtual DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

    private static readonly long s_minDateTicks = DateTime.MinValue.Ticks;
    private static readonly long s_maxDateTicks = DateTime.MaxValue.Ticks;

    /// <summary>
    /// Gets a <see cref="DateTimeOffset"/> value that is set to the current date and time according to this
    /// <see cref="TimeProvider"/>'s notion of time based on <see cref="GetUtcNow"/>, with the offset set to
    /// the <see cref="LocalTimeZone"/>'s offset from Coordinated Universal Time (UTC).
    /// </summary>
    public DateTimeOffset GetLocalNow()
    {
        var utcDateTime = GetUtcNow();
        var zoneInfo = LocalTimeZone;

        var offset = zoneInfo.GetUtcOffset(utcDateTime);

        var localTicks = utcDateTime.Ticks + offset.Ticks;
        if ((ulong)localTicks > (ulong)s_maxDateTicks)
        {
            localTicks = localTicks < s_minDateTicks ? s_minDateTicks : s_maxDateTicks;
        }

        return new DateTimeOffset(localTicks, offset);
    }

    /// <summary>
    /// Gets a <see cref="TimeZoneInfo"/> object that represents the local time zone according to this
    /// <see cref="TimeProvider"/>'s notion of time.
    /// </summary>
    /// <remarks>
    /// The default implementation returns <see cref="TimeZoneInfo.Local"/>.
    /// </remarks>
    public virtual TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;

    /// <summary>
    /// Gets the frequency of <see cref="GetTimestamp"/> of high-frequency value per second.
    /// </summary>
    /// <remarks>
    /// The default implementation returns <see cref="Stopwatch.Frequency"/>. For a given TimeProvider instance,
    /// the value must be idempotent and remain unchanged.
    /// </remarks>
    public virtual long TimestampFrequency => Stopwatch.Frequency;

    /// <summary>
    /// Gets the current high-frequency value designed to measure small time intervals with high accuracy in the
    /// timer mechanism.
    /// </summary>
    /// <returns>A long integer representing the high-frequency counter value of the underlying timer
    /// mechanism.</returns>
    /// <remarks>
    /// The default implementation returns <see cref="Stopwatch.GetTimestamp"/>.
    /// </remarks>
    public virtual long GetTimestamp() => Stopwatch.GetTimestamp();

    /// <summary>
    /// Gets the elapsed time between two timestamps retrieved using <see cref="GetTimestamp"/>.
    /// </summary>
    /// <param name="startingTimestamp">The timestamp marking the beginning of the time period.</param>
    /// <param name="endingTimestamp">The timestamp marking the end of the time period.</param>
    /// <returns>A <see cref="TimeSpan"/> for the elapsed time between the starting and ending timestamps.</returns>
    public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
    {
        var timestampFrequency = TimestampFrequency;
        if (timestampFrequency <= 0)
        {
            ThrowHelper.ThrowInvalidOperationException("");
        }

        return new TimeSpan((long)((endingTimestamp - startingTimestamp)
            * ((double)TimeSpan.TicksPerSecond / timestampFrequency)));
    }

    /// <summary>
    /// Gets the elapsed time since the <paramref name="startingTimestamp"/> value retrieved using
    /// <see cref="GetTimestamp"/>.
    /// </summary>
    /// <param name="startingTimestamp">The timestamp marking the beginning of the time period.</param>
    /// <returns>A <see cref="TimeSpan"/> for the elapsed time between the starting timestamp and the time of
    /// this call.</returns>
    public TimeSpan GetElapsedTime(long startingTimestamp) => GetElapsedTime(startingTimestamp, GetTimestamp());

    /// <summary>Creates a new <see cref="ITimer"/> instance, using <see cref="TimeSpan"/> values to measure
    /// time intervals.</summary>
    /// <param name="callback">
    /// A delegate representing a method to be executed when the timer fires. The method specified for callback
    /// should be reentrant, as it may be invoked simultaneously on two threads if the timer fires again before
    /// or while a previous callback is still being handled.
    /// </param>
    /// <param name="state">An object to be passed to the <paramref name="callback"/>. This may be null.</param>
    /// <param name="dueTime">The amount of time to delay before <paramref name="callback"/> is invoked. Specify
    /// <see cref="Timeout.InfiniteTimeSpan"/> to prevent the timer from starting. Specify
    /// <see cref="TimeSpan.Zero"/> to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of <paramref name="callback"/>. Specify
    /// <see cref="Timeout.InfiniteTimeSpan"/> to disable periodic signaling.</param>
    /// <returns>
    /// The newly created <see cref="ITimer"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="callback"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The number of milliseconds in the value of
    /// <paramref name="dueTime"/> or <paramref name="period"/> is negative and not equal to
    /// <see cref="Timeout.Infinite"/>, or is greater than <see cref="int.MaxValue"/>.</exception>
    /// <remarks>
    /// <para>
    /// The delegate specified by the callback parameter is invoked once after <paramref name="dueTime"/> elapses,
    /// and thereafter each time the <paramref name="period"/> time interval elapses.
    /// </para>
    /// <para>
    /// If <paramref name="dueTime"/> is zero, the callback is invoked immediately. If <paramref name="dueTime"/>
    /// is -1 milliseconds, <paramref name="callback"/> is not invoked; the timer is disabled,
    /// but can be re-enabled by calling the <see cref="ITimer.Change"/> method.
    /// </para>
    /// <para>
    /// If <paramref name="period"/> is 0 or -1 milliseconds and <paramref name="dueTime"/> is positive,
    /// <paramref name="callback"/> is invoked once; the periodic behavior of the timer is disabled,
    /// but can be re-enabled using the <see cref="ITimer.Change"/> method.
    /// </para>
    /// <para>
    /// The return <see cref="ITimer"/> instance will be implicitly rooted while the timer is still scheduled.
    /// </para>
    /// <para>
    /// <see cref="CreateTimer"/> captures the <see cref="ExecutionContext"/> and stores that with the
    /// <see cref="ITimer"/> for use in invoking <paramref name="callback"/>
    /// each time it's called. That capture can be suppressed with <see cref="ExecutionContext.SuppressFlow"/>.
    /// </para>
    /// </remarks>
    public virtual ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        ArgumentNullException.ThrowIfNull(callback);

        return new SystemTimeProviderTimer(dueTime, period, callback, state);
    }

    /// <summary>Thin wrapper for a <see cref="Timer"/>.</summary>
    /// <remarks>
    /// We don't return a TimerQueueTimer directly as it implements IThreadPoolWorkItem and we don't
    /// want it exposed in a way that user code could directly queue the timer to the thread pool.
    /// We also use this instead of Timer because CreateTimer needs to return a timer that's implicitly
    /// rooted while scheduled.
    /// </remarks>
    private sealed class SystemTimeProviderTimer : ITimer, IDisposable
    {
        private const uint MaxSupportedTimeout = 0xfffffffe;

        private readonly Timer _timer;

        public SystemTimeProviderTimer(TimeSpan dueTime, TimeSpan period, TimerCallback callback, object? state)
        {
            (var duration, var periodTime) = CheckAndGetValues(dueTime, period);

            _timer = new Timer(callback, state, duration, periodTime);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            (var duration, var periodTime) = CheckAndGetValues(dueTime, period);
            try
            {
                return _timer.Change(duration, periodTime);
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public void Dispose() => _timer.Dispose();

        public ValueTask DisposeAsync() => _timer.DisposeAsync();

        private static (uint duration, uint periodTime) CheckAndGetValues(TimeSpan dueTime, TimeSpan periodTime)
        {
            var dueTm = (long)dueTime.TotalMilliseconds;
            var periodTm = (long)periodTime.TotalMilliseconds;

            Guard.IsGreaterThanOrEqualTo(dueTm, -1, nameof(dueTime));
            Guard.IsLessThanOrEqualTo(dueTm, MaxSupportedTimeout, nameof(dueTime));

            Guard.IsGreaterThanOrEqualTo(periodTm, -1, nameof(periodTime));
            Guard.IsLessThanOrEqualTo(periodTm, MaxSupportedTimeout, nameof(periodTime));

            return ((uint)dueTm, (uint)periodTm);
        }
    }

    /// <summary>
    /// Used to create a <see cref="TimeProvider"/> instance returned from <see cref="System"/> and
    /// uses the default implementation
    /// provided by <see cref="TimeProvider"/> which uses <see cref="DateTimeOffset.UtcNow"/>,
    /// <see cref="TimeZoneInfo.Local"/>, <see cref="Stopwatch"/>, and <see cref="Timer"/>.
    /// </summary>
    private sealed class SystemTimeProvider : TimeProvider
    {
        /// <summary>Initializes the instance.</summary>
        internal SystemTimeProvider()
        {
        }
    }
}

#endif
