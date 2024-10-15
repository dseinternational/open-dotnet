// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[JsonSerializable(typeof(AmountMeasure))]
[JsonSerializable(typeof(BinaryMeasure))]
[JsonSerializable(typeof(BehaviorFrequencyMeasure))]
[JsonSerializable(typeof(BinaryMeasure))]
[JsonSerializable(typeof(BinarySentenceMeasure))]
[JsonSerializable(typeof(BinarySpeechSoundMeasure))]
[JsonSerializable(typeof(BinaryWordMeasure))]
[JsonSerializable(typeof(CompletenessMeasure))]
[JsonSerializable(typeof(CountMeasure))]
[JsonSerializable(typeof(RatioMeasure))]
[JsonSerializable(typeof(SentenceCompletenessMeasure))]
[JsonSerializable(typeof(SentenceFrequencyMeasure))]
[JsonSerializable(typeof(SpeechClarityMeasure))]
[JsonSerializable(typeof(SpeechSoundClarityMeasure))]
[JsonSerializable(typeof(SpeechSoundFrequencyMeasure))]
[JsonSerializable(typeof(SpokenWordClarityMeasure))]
[JsonSerializable(typeof(WordFrequencyMeasure))]
[JsonSerializable(typeof(MeasurementSnapshotSet<BinarySnapshot, BinaryObservation, bool>))]
[JsonSerializable(typeof(MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>))]
[JsonSerializable(typeof(MeasurementSnapshotSet<AmountSnapshot, AmountObservation, Amount>))]
[JsonSerializable(typeof(ObservationSet))]
public partial class ObservationsJsonSerializerContext : JsonSerializerContext
{
    public static ObservationsJsonSerializerContext RelaxedJsonEscaping { get; } = new ObservationsJsonSerializerContext(JsonSharedOptions.RelaxedJsonEscaping);
}
