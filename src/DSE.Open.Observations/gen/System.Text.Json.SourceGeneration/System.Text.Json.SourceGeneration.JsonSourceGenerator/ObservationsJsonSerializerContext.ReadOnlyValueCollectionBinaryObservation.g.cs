﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>>? _ReadOnlyValueCollectionBinaryObservation;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>> ReadOnlyValueCollectionBinaryObservation
        #nullable enable annotations
        {
            get => _ReadOnlyValueCollectionBinaryObservation ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>>)Options.GetTypeInfo(typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>> Create_ReadOnlyValueCollectionBinaryObservation(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>> jsonTypeInfo))
            {
                var info = new global::System.Text.Json.Serialization.Metadata.JsonCollectionInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>>
                {
                    ObjectCreator = () => new global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>(),
                    SerializeHandler = ReadOnlyValueCollectionBinaryObservationSerializeHandler
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateICollectionInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>, global::DSE.Open.Observations.BinaryObservation>(options, info);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void ReadOnlyValueCollectionBinaryObservationSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryObservation>? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();

            foreach (global::DSE.Open.Observations.BinaryObservation element in value)
            {
                BinaryObservationSerializeHandler(writer, element);
            }

            writer.WriteEndArray();
        }
    }
}