﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>>? _MeasurementSnapshotSetAmountSnapshotAmountObservationAmount;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>> MeasurementSnapshotSetAmountSnapshotAmountObservationAmount
        #nullable enable annotations
        {
            get => _MeasurementSnapshotSetAmountSnapshotAmountObservationAmount ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>> Create_MeasurementSnapshotSetAmountSnapshotAmountObservationAmount(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>> jsonTypeInfo))
            {
                var info = new global::System.Text.Json.Serialization.Metadata.JsonCollectionInfoValues<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>>
                {
                    ObjectCreator = () => new global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>(),
                    SerializeHandler = MeasurementSnapshotSetAmountSnapshotAmountObservationAmountSerializeHandler
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateISetInfo<global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>, global::DSE.Open.Observations.AmountSnapshot>(options, info);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void MeasurementSnapshotSetAmountSnapshotAmountObservationAmountSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Observations.MeasurementSnapshotSet<global::DSE.Open.Observations.AmountSnapshot, global::DSE.Open.Observations.AmountObservation, global::DSE.Open.Values.Amount>? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();

            foreach (global::DSE.Open.Observations.AmountSnapshot element in value)
            {
                global::System.Text.Json.JsonSerializer.Serialize(writer, element, AmountSnapshot);
            }

            writer.WriteEndArray();
        }
    }
}
