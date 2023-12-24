// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Polly.Contrib.WaitAndRetry;

namespace DSE.Open.Retries;

public static class Delays
{
    /// <summary>
    /// Gets a sequence of 5 <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy. The values provided offer an exponentially backing-off,
    /// jittered retry behaviour with a median first retry delay of 1 second.
    /// </summary>
    /// <returns>A sequence of <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy.</returns>
    public static IEnumerable<TimeSpan> ExponentialBackoffWithJitter()
    {
        return ExponentialBackoffWithJitter(TimeSpan.FromSeconds(1.0));
    }

    /// <summary>
    /// Gets a sequence of 5 <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy. The values provided offer an exponentially backing-off,
    /// jittered retry behaviour.
    /// </summary>
    /// <param name="medianFirstRetryDelay">The median delay to target before the first retry.
    /// Choose this value both to approximate the first delay, and to scale the remainder of
    /// the series.</param>
    /// <returns>A sequence of <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy.</returns>
    public static IEnumerable<TimeSpan> ExponentialBackoffWithJitter(TimeSpan medianFirstRetryDelay)
    {
        return ExponentialBackoffWithJitter(medianFirstRetryDelay, 5);
    }

    /// <summary>
    /// Gets a sequence of <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy. The values provided offer an exponentially backing-off,
    /// jittered retry behaviour.
    /// </summary>
    /// <param name="medianFirstRetryDelay">The median delay to target before the first retry.
    /// Choose this value both to approximate the first delay, and to scale the remainder of
    /// the series.</param>
    /// <param name="retryCount">The maximum number of retries to use, in addition to the
    /// original call.</param>
    /// <returns>A sequence of <see cref="TimeSpan"/> values that can be used as sleep durations
    /// for a wait-and-retry policy.</returns>
    public static IEnumerable<TimeSpan> ExponentialBackoffWithJitter(TimeSpan medianFirstRetryDelay, int retryCount)
    {
        return Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2.0), retryCount: 5);
    }
}
