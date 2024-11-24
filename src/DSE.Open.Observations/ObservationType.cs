// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// Identifies types of observations.
/// </summary>
public enum ObservationType
{
    Unknown,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="bool"/> value.
    /// </summary>
    Binary = 101,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    BinarySpeechSound = 111,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    BinaryWord = 112,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    BinarySentence = 113,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.BehaviorFrequency"/> value.
    /// </summary>
    BehaviorFrequency = 1001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    BehaviorFrequencySpeechSound = 1011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    BehaviorFrequencyWord = 1012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    BehaviorFrequencySentence = 1013,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Values.Count"/> value.
    /// </summary>
    Count = 2001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.SpeechClarity"/> value.
    /// </summary>
    SpeechClarity = 3001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    SpeechClaritySpeechSound = 3011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    SpeechClarityWord = 3012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    SpeechClaritySentence = 3013,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.Completeness"/> value.
    /// </summary>
    Completeness = 4001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    CompletenessSpeechSound = 4011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    CompletenessWord = 4012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    CompletenessSentence = 4013,
}
