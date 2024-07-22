// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Testing;

public static class FakeTimeProviderExtensions
{
    public static FakeTimeProvider AddMinutes(this FakeTimeProvider timeProvider, int minutes)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        timeProvider.SetUtcNow(timeProvider.GetUtcNow().AddMinutes(minutes));
        return timeProvider;
    }

    public static FakeTimeProvider AddSeconds(this FakeTimeProvider timeProvider, int seconds)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        timeProvider.SetUtcNow(timeProvider.GetUtcNow().AddSeconds(seconds));
        return timeProvider;
    }
}
