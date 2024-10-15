// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record BinarySentenceObservationSet : ObservationSet<BinarySentenceObservation, bool>
{
    internal BinarySentenceObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinarySentenceObservation> observations)
        : base(created, trackerReference, observerReference, source, location, observations)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinarySentenceObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinarySentenceObservation> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location, observations)
    {
    }

    public static BinarySentenceObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<BinarySentenceObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static BinarySentenceObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinarySentenceObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static BinarySentenceObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinarySentenceObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySentenceObservationSet(timeProvider.GetUtcNow(), trackerReference, observerReference, source, location, observations);
    }
}
