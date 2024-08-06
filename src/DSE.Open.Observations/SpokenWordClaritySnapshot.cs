// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SpokenWordClaritySnapshot : Snapshot<SpokenWordClarityObservation, SpeechClarity, WordId>
{
    [JsonConstructor]
    internal SpokenWordClaritySnapshot(DateTimeOffset time, SpokenWordClarityObservation observation) : base(time, observation)
    {
    }

    public static SpokenWordClaritySnapshot ForUtcNow(SpokenWordClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpokenWordClaritySnapshot ForUtcNow(SpokenWordClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SpokenWordClaritySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
