﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>>? _ReadOnlyValueCollectionBinarySpeechSoundObservation;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>> ReadOnlyValueCollectionBinarySpeechSoundObservation
        #nullable enable annotations
        {
            get => _ReadOnlyValueCollectionBinarySpeechSoundObservation ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>>)Options.GetTypeInfo(typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>> Create_ReadOnlyValueCollectionBinarySpeechSoundObservation(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>> jsonTypeInfo))
            {
                var info = new global::System.Text.Json.Serialization.Metadata.JsonCollectionInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>>
                {
                    ObjectCreator = () => new global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>(),
                    SerializeHandler = ReadOnlyValueCollectionBinarySpeechSoundObservationSerializeHandler
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateICollectionInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>, global::DSE.Open.Observations.BinarySpeechSoundObservation>(options, info);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void ReadOnlyValueCollectionBinarySpeechSoundObservationSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinarySpeechSoundObservation>? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartArray();

            foreach (global::DSE.Open.Observations.BinarySpeechSoundObservation element in value)
            {
                global::System.Text.Json.JsonSerializer.Serialize(writer, element, BinarySpeechSoundObservation);
            }

            writer.WriteEndArray();
        }
    }
}
