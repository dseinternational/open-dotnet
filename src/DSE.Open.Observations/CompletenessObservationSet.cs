// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CompletenessObservationSet : ObservationSet<CompletenessObservation, Completeness>
{
    private CompletenessObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CompletenessObservation> observations)
        : base(created, trackerReference, observerReference, source, location, observations)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private CompletenessObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CompletenessObservation> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location, observations)
    {
    }

    public static CompletenessObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<CompletenessObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static CompletenessObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CompletenessObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static CompletenessObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<CompletenessObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CompletenessObservationSet(timeProvider.GetUtcNow(), trackerReference, observerReference, source, location, observations);
    }
}
