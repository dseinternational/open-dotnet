// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record SpeechSoundClarityObservationSet : ObservationSet<SpeechSoundClarityObservation, SpeechClarity>
{
    public static SpeechSoundClarityObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        ReadOnlyValueCollection<SpeechSoundClarityObservation> observations)
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static SpeechSoundClarityObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<SpeechSoundClarityObservation> observations)
    {
        return Create(trackerReference, observerReference, source, location, observations, TimeProvider.System);
    }

    public static SpeechSoundClarityObservationSet Create(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<SpeechSoundClarityObservation> observations,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundClarityObservationSet
        {
            Created = timeProvider.GetUtcNow(),
            TrackerReference = trackerReference,
            ObserverReference = observerReference,
            Source = source,
            Location = location,
            Observations = observations
        };
    }
}
