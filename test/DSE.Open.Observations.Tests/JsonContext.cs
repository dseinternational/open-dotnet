// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

[JsonSerializable(typeof(IObservation))]
[JsonSerializable(typeof(Measure<Amount>))]
[JsonSerializable(typeof(Measure<BehaviorFrequency, SentenceId>))]
[JsonSerializable(typeof(Measure<BehaviorFrequency, SpeechSound>))]
[JsonSerializable(typeof(Measure<BehaviorFrequency, WordId>))]
[JsonSerializable(typeof(Measure<BehaviorFrequency>))]
[JsonSerializable(typeof(Measure<Binary, SentenceId>))]
[JsonSerializable(typeof(Measure<Binary, SpeechSound>))]
[JsonSerializable(typeof(Measure<Binary, WordId>))]
[JsonSerializable(typeof(Measure<Binary>))]
[JsonSerializable(typeof(Measure<Completeness, SentenceId>))]
[JsonSerializable(typeof(Measure<Completeness>))]
[JsonSerializable(typeof(Measure<Count>))]
[JsonSerializable(typeof(Measure<Ratio>))]
[JsonSerializable(typeof(Measure<SpeechClarity, SpeechSound>))]
[JsonSerializable(typeof(Measure<SpeechClarity, WordId>))]
[JsonSerializable(typeof(Measure<SpeechClarity>))]
[JsonSerializable(typeof(Observation))]
[JsonSerializable(typeof(Observation<BehaviorFrequency, SentenceId>))]
[JsonSerializable(typeof(Observation<BehaviorFrequency, SpeechSound>))]
[JsonSerializable(typeof(Observation<BehaviorFrequency, WordId>))]
[JsonSerializable(typeof(Observation<BehaviorFrequency>))]
[JsonSerializable(typeof(Observation<Binary, SentenceId>))]
[JsonSerializable(typeof(Observation<Binary, SpeechSound>))]
[JsonSerializable(typeof(Observation<Binary, WordId>))]
[JsonSerializable(typeof(Observation<Binary>))]
public sealed partial class JsonContext : JsonSerializerContext;
