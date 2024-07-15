// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public interface ISpeechSoundObservation : IObservation
{
    /// <summary>
    /// Identifies the speech sound that was involved in the observation.
    /// </summary>
    SpeechSound SpeechSound { get; init; }
}
