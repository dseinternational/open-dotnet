// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// Identifies types of observations.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<MeasureType>))]
public enum MeasureType
{
    [JsonStringEnumMemberName("unknown")]
    Unknown,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="bool"/> value.
    /// </summary>
    [JsonStringEnumMemberName("binary")]
    Binary = 101,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("binary_speech_sound")]
    BinarySpeechSound = 111,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("binary_word")]
    BinaryWord = 112,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="bool"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("binary_sentence")]
    BinarySentence = 113,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.BehaviorFrequency"/> value.
    /// </summary>
    [JsonStringEnumMemberName("behavior_frequency")]
    BehaviorFrequency = 1001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("behavior_frequency_speech_sound")]
    BehaviorFrequencySpeechSound = 1011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("behavior_frequency_word")]
    BehaviorFrequencyWord = 1012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.BehaviorFrequency"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("behavior_frequency_sentence")]
    BehaviorFrequencySentence = 1013,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.Count"/> value.
    /// </summary>
    [JsonStringEnumMemberName("count")]
    Count = 2001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with an <see cref="Observations.Amount"/> value.
    /// </summary>
    [JsonStringEnumMemberName("amount")]
    Amount = 2010,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.SpeechClarity"/> value.
    /// </summary>
    [JsonStringEnumMemberName("speech_clarity")]
    SpeechClarity = 3001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("speech_clarity_speech_sound")]
    SpeechClaritySpeechSound = 3011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("speech_clarity_word")]
    SpeechClarityWord = 3012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.SpeechClarity"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("speech_clarity_sentence")]
    SpeechClaritySentence = 3013,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue}"/> with a <see cref="Observations.Completeness"/> value.
    /// </summary>
    [JsonStringEnumMemberName("completeness")]
    Completeness = 4001,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="SpeechSound"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("completeness_speech_sound")]
    CompletenessSpeechSound = 4011,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="WordId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("completeness_word")]
    CompletenessWord = 4012,

    /// <summary>
    /// Gets the type id for an <see cref="Observation{TValue, TParam}"/> with a <see cref="Observations.Completeness"/> value
    /// and a <see cref="SentenceId"/> parameter.
    /// </summary>
    [JsonStringEnumMemberName("completeness_sentence")]
    CompletenessSentence = 4013,
}
