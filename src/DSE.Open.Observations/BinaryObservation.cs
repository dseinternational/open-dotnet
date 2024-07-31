// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public record BinaryObservation : Observation<bool>
{
    protected BinaryObservation(Measure measure, DateTimeOffset time, bool value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinaryObservation(ObservationId id, MeasureId measureId, long timestamp, bool value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static BinaryObservation Create(Measure measure, bool value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static BinaryObservation Create(
        Measure measure,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
