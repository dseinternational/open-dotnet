// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryObservation : Observation<bool>
{
    private BinaryObservation(uint measureId, DateTimeOffset time, bool value)
        : base(measureId, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinaryObservation(ulong id, uint measureId, DateTimeOffset time, bool value)
        : base(id, measureId, time, value)
    {
    }

    public static BinaryObservation Create(uint measureId, bool value)
    {
        return Create(measureId, value, TimeProvider.System);
    }

    public static BinaryObservation Create(
        uint measureId,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryObservation(measureId, timeProvider.GetUtcNow(), value);
    }
}
