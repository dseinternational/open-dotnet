// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record BehaviorFrequencyObservationSet : ObservationSet<BehaviorFrequencyObservation, BehaviorFrequency>
{
    private BehaviorFrequencyObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BehaviorFrequencyObservation> observations)
        : base(created, trackerReference, observerReference, source, location, observations)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BehaviorFrequencyObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BehaviorFrequencyObservation> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location, observations)
    {
    }

    public static BehaviorFrequencyObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<BehaviorFrequencyObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static BehaviorFrequencyObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BehaviorFrequencyObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static BehaviorFrequencyObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BehaviorFrequencyObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BehaviorFrequencyObservationSet(timeProvider.GetUtcNow(), trackerReference, observerReference, source, location, observations);
    }
}
