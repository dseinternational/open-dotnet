// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SentenceCompletenessSnapshot : Snapshot<SentenceCompletenessObservation, Completeness, SentenceId>
{
    [JsonConstructor]
    internal SentenceCompletenessSnapshot(DateTimeOffset time, SentenceCompletenessObservation observation) : base(time, observation)
    {
    }

    public static SentenceCompletenessSnapshot ForUtcNow(SentenceCompletenessObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SentenceCompletenessSnapshot ForUtcNow(SentenceCompletenessObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SentenceCompletenessSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
