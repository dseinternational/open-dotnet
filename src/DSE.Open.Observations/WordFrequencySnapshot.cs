// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record WordFrequencySnapshot : Snapshot<WordFrequencyObservation, BehaviorFrequency, WordId>
{
    [JsonConstructor]
    internal WordFrequencySnapshot(DateTimeOffset time, WordFrequencyObservation observation) : base(time, observation)
    {
    }

    public static WordFrequencySnapshot ForUtcNow(WordFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static WordFrequencySnapshot ForUtcNow(WordFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new WordFrequencySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
