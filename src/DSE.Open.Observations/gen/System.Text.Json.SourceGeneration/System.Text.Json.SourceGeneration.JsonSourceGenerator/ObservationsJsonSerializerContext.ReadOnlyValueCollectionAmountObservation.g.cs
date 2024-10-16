﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>>? _ReadOnlyValueCollectionAmountObservation;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>> ReadOnlyValueCollectionAmountObservation
        #nullable enable annotations
        {
            get => _ReadOnlyValueCollectionAmountObservation ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>>)Options.GetTypeInfo(typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>> Create_ReadOnlyValueCollectionAmountObservation(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>> jsonTypeInfo))
            {
                var info = new global::System.Text.Json.Serialization.Metadata.JsonCollectionInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>>
                {
                    ObjectCreator = () => new global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>(),
                    SerializeHandler = ReadOnlyValueCollectionAmountObservationSerializeHandler
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateICollectionInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>, global::DSE.Open.Observations.AmountObservation>(options, info);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void ReadOnlyValueCollectionAmountObservationSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.AmountObservation>? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();

            foreach (global::DSE.Open.Observations.AmountObservation element in value)
            {
                AmountObservationSerializeHandler(writer, element);
            }

            writer.WriteEndArray();
        }
    }
}
