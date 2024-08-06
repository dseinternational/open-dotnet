// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record SpeechClaritySnapshot : Snapshot<SpeechClarityObservation, SpeechClarity>
{
    [JsonConstructor]
    internal SpeechClaritySnapshot(DateTimeOffset time, SpeechClarityObservation observation) : base(time, observation)
    {
    }

    public static SpeechClaritySnapshot ForUtcNow(SpeechClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechClaritySnapshot ForUtcNow(SpeechClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SpeechClaritySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
