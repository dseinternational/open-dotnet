﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>>? _ReadOnlyValueCollectionBinarySentenceObservation;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>> ReadOnlyValueCollectionBinarySentenceObservation
        #nullable enable annotations
        {
            get => _ReadOnlyValueCollectionBinarySentenceObservation ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>>)Options.GetTypeInfo(typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>> Create_ReadOnlyValueCollectionBinarySentenceObservation(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>> jsonTypeInfo))
            {
                var info = new global::System.Text.Json.Serialization.Metadata.JsonCollectionInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>>
                {
                    ObjectCreator = () => new global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>(),
                    SerializeHandler = ReadOnlyValueCollectionBinarySentenceObservationSerializeHandler
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateICollectionInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>, global::DSE.Open.Observations.BinarySentenceObservation>(options, info);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void ReadOnlyValueCollectionBinarySentenceObservationSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySentenceObservation>? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();

            foreach (global::DSE.Open.Observations.BinarySentenceObservation element in value)
            {
                BinarySentenceObservationSerializeHandler(writer, element);
            }

            writer.WriteEndArray();
        }
    }
}
