// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Observations;

public static class ObservationsValidator
{
    public static readonly DateTimeOffset MinimumObservationTime =
        new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static void EnsureMinimumObservationTime(DateTimeOffset time, [CallerArgumentExpression(nameof(time))] string? paramName = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(time, MinimumObservationTime, paramName);
    }
}
