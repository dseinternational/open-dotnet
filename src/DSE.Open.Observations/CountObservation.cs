// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountObservation : Observation<Count>
{
    internal CountObservation(uint measureId, DateTimeOffset time, Count value)
        : base(measureId, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal CountObservation(ulong id, uint measureId, DateTimeOffset time, Count value)
        : base(id, measureId, time, value)
    {
    }

    public static CountObservation Create(uint measureId, Count value)
    {
        return Create(measureId, value, TimeProvider.System);
    }

    public static CountObservation Create(
        uint measureId,
        Count value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CountObservation(measureId, timeProvider.GetUtcNow(), value);
    }
}
