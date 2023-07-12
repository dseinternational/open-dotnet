// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Threading;

#if !NET8_0_OR_GREATER

/// <summary>Represents a timer that can have its due time and period changed.</summary>
/// <remarks>
/// Implementations of <see cref="Change"/>, <see cref="IDisposable.Dispose"/>, and <see cref="IAsyncDisposable.DisposeAsync"/>
/// must all be thread-safe such that the timer instance may be accessed concurrently from multiple threads.
/// <para><b>Note:</b> this interface will be available in .NET 8.0, at which time we will remove our version.</para>
/// </remarks>
public interface ITimer : IDisposable, IAsyncDisposable
{
    /// <summary>Changes the start time and the interval between method invocations for a timer, using <see cref="TimeSpan"/> values to measure time intervals.</summary>
    /// <param name="dueTime">
    /// A <see cref="TimeSpan"/> representing the amount of time to delay before invoking the callback method specified when the <see cref="ITimer"/> was constructed.
    /// Specify <see cref="Timeout.InfiniteTimeSpan"/> to prevent the timer from restarting. Specify <see cref="TimeSpan.Zero"/> to restart the timer immediately.
    /// </param>
    /// <param name="period">
    /// The time interval between invocations of the callback method specified when the Timer was constructed.
    /// Specify <see cref="Timeout.InfiniteTimeSpan"/> to disable periodic signaling.
    /// </param>
    /// <returns><see langword="true"/> if the timer was successfully updated; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dueTime"/> or <paramref name="period"/> parameter, in milliseconds, is less than -1 or greater than 4294967294.</exception>
    /// <remarks>
    /// It is the responsibility of the implementer of the ITimer interface to ensure thread safety.
    /// </remarks>
    bool Change(TimeSpan dueTime, TimeSpan period);
}

#endif
