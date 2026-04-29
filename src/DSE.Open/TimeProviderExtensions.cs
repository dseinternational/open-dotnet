// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open;

/// <summary>
/// Extensions over <see cref="TimeProvider"/> integrating with NodaTime.
/// </summary>
public static class TimeProviderExtensions
{
    /// <summary>
    /// Gets the current UTC time from <paramref name="timeProvider"/> as a NodaTime <see cref="Instant"/>.
    /// </summary>
    public static Instant GetCurrentInstant(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return Instant.FromDateTimeOffset(timeProvider.GetUtcNow());
    }

    /// <summary>
    /// Returns an <see cref="IClock"/> that delegates to <paramref name="timeProvider"/>.
    /// </summary>
    public static IClock AsClock(this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new TimeProviderClock(timeProvider);
    }

    private sealed class TimeProviderClock : IClock
    {
        private readonly TimeProvider _timeProvider;

        public TimeProviderClock(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public Instant GetCurrentInstant()
        {
            return _timeProvider.GetCurrentInstant();
        }
    }
}
