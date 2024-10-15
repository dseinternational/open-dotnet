// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record BinaryObservationSet : ObservationSet<BinaryObservation, bool>
{
    private BinaryObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinaryObservation> observations)
        : base(created, trackerReference, observerReference, source, location, observations)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinaryObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinaryObservation> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location, observations)
    {
    }

    public static BinaryObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<BinaryObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static BinaryObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinaryObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static BinaryObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<BinaryObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryObservationSet(timeProvider.GetUtcNow(), trackerReference, observerReference, source, location, observations);
    }
}
