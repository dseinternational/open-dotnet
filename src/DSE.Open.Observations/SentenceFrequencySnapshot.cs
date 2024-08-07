// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SentenceFrequencySnapshot : Snapshot<SentenceFrequencyObservation, BehaviorFrequency, SentenceId>
{
    [JsonConstructor]
    internal SentenceFrequencySnapshot(DateTimeOffset time, SentenceFrequencyObservation observation) : base(time, observation)
    {
    }

    public static SentenceFrequencySnapshot ForUtcNow(SentenceFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SentenceFrequencySnapshot ForUtcNow(SentenceFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SentenceFrequencySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
