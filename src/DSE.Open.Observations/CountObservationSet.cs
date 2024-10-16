// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountObservationSet : ObservationSet<CountObservation, Count>
{
    internal CountObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CountObservation> observations)
        : base(created, trackerReference, observerReference, source, location, observations)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal CountObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CountObservation> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location, observations)
    {
    }

    public static CountObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<CountObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static CountObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CountObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static CountObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CountObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CountObservationSet(timeProvider.GetUtcNow(), trackerReference, observerReference, source, location, observations);
    }
}
