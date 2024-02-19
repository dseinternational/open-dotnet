// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open;

public static class TimeProviderExtensions
{
    public static Instant GetCurrentInstant(this TimeProvider timeProvider)
    {
        Guard.IsNotNull(timeProvider);
        return Instant.FromDateTimeOffset(timeProvider.GetUtcNow());
    }

    public static IClock AsClock(this TimeProvider timeProvider)
    {
        Guard.IsNotNull(timeProvider);
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
