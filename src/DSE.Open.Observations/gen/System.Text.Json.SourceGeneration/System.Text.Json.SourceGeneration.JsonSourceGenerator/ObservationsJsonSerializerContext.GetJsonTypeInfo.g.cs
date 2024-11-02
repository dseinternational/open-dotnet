﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext : global::System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver
    {
        /// <inheritdoc/>
        public override global::System.Text.Json.Serialization.Metadata.JsonTypeInfo? GetTypeInfo(global::System.Type type)
        {
            Options.TryGetTypeInfo(type, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo? typeInfo);
            return typeInfo;
        }

        global::System.Text.Json.Serialization.Metadata.JsonTypeInfo? global::System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver.GetTypeInfo(global::System.Type type, global::System.Text.Json.JsonSerializerOptions options)
        {
            if (type == typeof(bool))
            {
                return Create_Boolean(options);
            }
            if (type == typeof(double))
            {
                return Create_Double(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>))
            {
                return Create_ReadOnlyValueCollectionAmountObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>))
            {
                return Create_ReadOnlyValueCollectionBinaryObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>))
            {
                return Create_ReadOnlyValueCollectionBinarySentenceObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>))
            {
                return Create_ReadOnlyValueCollectionBinarySpeechSoundObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>))
            {
                return Create_ReadOnlyValueCollectionBinaryWordObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>))
            {
                return Create_ReadOnlyValueCollectionCountObservation(options);
            }
            if (type == typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.RatioObservation>))
            {
                return Create_ReadOnlyValueCollectionRatioObservation(options);
            }
            if (type == typeof(global::DSE.Open.Language.SentenceId))
            {
                return Create_SentenceId(options);
            }
            if (type == typeof(global::DSE.Open.Language.WordId))
            {
                return Create_WordId(options);
            }
            if (type == typeof(global::DSE.Open.Observations.AmountMeasure))
            {
                return Create_AmountMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.AmountObservation))
            {
                return Create_AmountObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.AmountObservationSet))
            {
                return Create_AmountObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.AmountSnapshot))
            {
                return Create_AmountSnapshot(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BehaviorFrequencyMeasure))
            {
                return Create_BehaviorFrequencyMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryMeasure))
            {
                return Create_BinaryMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryObservation))
            {
                return Create_BinaryObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryObservationSet))
            {
                return Create_BinaryObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySentenceMeasure))
            {
                return Create_BinarySentenceMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySentenceObservation))
            {
                return Create_BinarySentenceObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySentenceObservationSet))
            {
                return Create_BinarySentenceObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySnapshot))
            {
                return Create_BinarySnapshot(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySpeechSoundMeasure))
            {
                return Create_BinarySpeechSoundMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySpeechSoundObservation))
            {
                return Create_BinarySpeechSoundObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinarySpeechSoundObservationSet))
            {
                return Create_BinarySpeechSoundObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryWordMeasure))
            {
                return Create_BinaryWordMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryWordObservation))
            {
                return Create_BinaryWordObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.BinaryWordObservationSet))
            {
                return Create_BinaryWordObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.CompletenessMeasure))
            {
                return Create_CompletenessMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.CountMeasure))
            {
                return Create_CountMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.CountObservation))
            {
                return Create_CountObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.CountObservationSet))
            {
                return Create_CountObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.CountSnapshot))
            {
                return Create_CountSnapshot(options);
            }
            if (type == typeof(global::DSE.Open.Observations.GroundPoint))
            {
                return Create_GroundPoint(options);
            }
            if (type == typeof(global::DSE.Open.Observations.GroundPoint?))
            {
                return Create_NullableGroundPoint(options);
            }
            if (type == typeof(global::DSE.Open.Observations.MeasureId))
            {
                return Create_MeasureId(options);
            }
            if (type == typeof(global::DSE.Open.Observations.MeasurementLevel))
            {
                return Create_MeasurementLevel(options);
            }
            if (type == typeof(global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>))
            {
                return Create_MeasurementSnapshotSetAmountSnapshotAmountObservationAmount(options);
            }
            if (type == typeof(global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.BinarySnapshot, global::DSE.Open.Observations.BinaryObservation, bool>))
            {
                return Create_MeasurementSnapshotSetBinarySnapshotBinaryObservationBoolean(options);
            }
            if (type == typeof(global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.CountSnapshot, global::DSE.Open.Observations.CountObservation, global::DSE.Open.Values.Count>))
            {
                return Create_MeasurementSnapshotSetCountSnapshotCountObservationCount(options);
            }
            if (type == typeof(global::DSE.Open.Observations.ObservationId))
            {
                return Create_ObservationId(options);
            }
            if (type == typeof(global::DSE.Open.Observations.ObservationSet))
            {
                return Create_ObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.ObservationSetId))
            {
                return Create_ObservationSetId(options);
            }
            if (type == typeof(global::DSE.Open.Observations.RatioMeasure))
            {
                return Create_RatioMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.RatioObservation))
            {
                return Create_RatioObservation(options);
            }
            if (type == typeof(global::DSE.Open.Observations.RatioObservationSet))
            {
                return Create_RatioObservationSet(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SentenceCompletenessMeasure))
            {
                return Create_SentenceCompletenessMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SentenceFrequencyMeasure))
            {
                return Create_SentenceFrequencyMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SpeechClarityMeasure))
            {
                return Create_SpeechClarityMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SpeechSoundClarityMeasure))
            {
                return Create_SpeechSoundClarityMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SpeechSoundFrequencyMeasure))
            {
                return Create_SpeechSoundFrequencyMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.SpokenWordClarityMeasure))
            {
                return Create_SpokenWordClarityMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Observations.WordFrequencyMeasure))
            {
                return Create_WordFrequencyMeasure(options);
            }
            if (type == typeof(global::DSE.Open.Speech.SpeechSound))
            {
                return Create_SpeechSound(options);
            }
            if (type == typeof(global::DSE.Open.Values.Amount))
            {
                return Create_Amount(options);
            }
            if (type == typeof(global::DSE.Open.Values.Count))
            {
                return Create_Count(options);
            }
            if (type == typeof(global::DSE.Open.Values.Identifier))
            {
                return Create_Identifier(options);
            }
            if (type == typeof(global::DSE.Open.Values.Ratio))
            {
                return Create_Ratio(options);
            }
            if (type == typeof(global::DSE.Open.Values.Units.Length))
            {
                return Create_Length(options);
            }
            if (type == typeof(global::System.DateTimeOffset))
            {
                return Create_DateTimeOffset(options);
            }
            if (type == typeof(global::System.Uri))
            {
                return Create_Uri(options);
            }
            if (type == typeof(string))
            {
                return Create_String(options);
            }
            return null;
        }
    }
}
