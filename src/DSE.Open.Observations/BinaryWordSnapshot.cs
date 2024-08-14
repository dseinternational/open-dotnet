// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;
using MessagePack;

namespace DSE.Open.Observations;

[MessagePackObject]
public sealed record BinaryWordSnapshot : Snapshot<BinaryWordObservation, bool, WordId>
{
    [JsonConstructor]
    [SerializationConstructor]
    public BinaryWordSnapshot(DateTimeOffset time, BinaryWordObservation observation) : base(time, observation)
    {
    }

    public static BinaryWordSnapshot ForUtcNow(BinaryWordObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinaryWordSnapshot ForUtcNow(BinaryWordObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinaryWordSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
