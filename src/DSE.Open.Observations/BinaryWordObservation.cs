// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryWordObservation : Observation<bool, uint>
{
    internal BinaryWordObservation(uint measureId, uint discriminator, DateTimeOffset time, bool value)
        : base(measureId, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinaryWordObservation(ulong id, uint measureId, uint discriminator, DateTimeOffset time, bool value)
        : base(id, measureId, discriminator, time, value)
    {
    }

    [JsonIgnore]
    public uint WordId => Discriminator;

    public static BinaryWordObservation Create(uint measureId, uint wordId, bool value)
    {
        return Create(measureId, wordId, value, TimeProvider.System);
    }

    public static BinaryWordObservation Create(
        uint measureId,
        uint wordId,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryWordObservation(measureId, wordId, timeProvider.GetUtcNow(), value);
    }
}
