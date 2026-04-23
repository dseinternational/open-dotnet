// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Testing;

/// <summary>
/// Convenience helpers that advance a <see cref="FakeTimeProvider"/>'s clock forwards,
/// returning the same provider so calls can be chained.
/// </summary>
/// <remarks>
/// <see cref="FakeTimeProvider.SetUtcNow(DateTimeOffset)"/> refuses to rewind, so these
/// helpers only accept positive increments.
/// </remarks>
public static class FakeTimeProviderExtensions
{
    /// <summary>
    /// Advances the provider's UTC clock forwards by <paramref name="minutes"/> minutes.
    /// </summary>
    /// <param name="timeProvider">The provider to advance.</param>
    /// <param name="minutes">The number of minutes to add. Must be non-negative.</param>
    /// <returns>The same <paramref name="timeProvider"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minutes"/> is negative.</exception>
    public static FakeTimeProvider AddMinutes(this FakeTimeProvider timeProvider, int minutes)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentOutOfRangeException.ThrowIfNegative(minutes);
        timeProvider.SetUtcNow(timeProvider.GetUtcNow().AddMinutes(minutes));
        return timeProvider;
    }

    /// <summary>
    /// Advances the provider's UTC clock forwards by <paramref name="seconds"/> seconds.
    /// </summary>
    /// <param name="timeProvider">The provider to advance.</param>
    /// <param name="seconds">The number of seconds to add. Must be non-negative.</param>
    /// <returns>The same <paramref name="timeProvider"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="seconds"/> is negative.</exception>
    public static FakeTimeProvider AddSeconds(this FakeTimeProvider timeProvider, int seconds)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentOutOfRangeException.ThrowIfNegative(seconds);
        timeProvider.SetUtcNow(timeProvider.GetUtcNow().AddSeconds(seconds));
        return timeProvider;
    }
}
