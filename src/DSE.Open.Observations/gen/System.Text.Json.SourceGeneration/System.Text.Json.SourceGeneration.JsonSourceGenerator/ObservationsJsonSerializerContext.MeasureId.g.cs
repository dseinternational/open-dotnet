﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasureId>? _MeasureId;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasureId> MeasureId
        #nullable enable annotations
        {
            get => _MeasureId ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasureId>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.MeasureId));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasureId> Create_MeasureId(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.MeasureId>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.MeasureId> jsonTypeInfo))
            {
                global::System.Text.Json.Serialization.JsonConverter converter = ExpandConverter(typeof(global::DSE.Open.Observations.MeasureId), new global::DSE.Open.Values.Text.Json.Serialization.JsonUInt64ValueConverter<global::DSE.Open.Observations.MeasureId>(), options);
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateValueInfo<global::DSE.Open.Observations.MeasureId> (options, converter);
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }
    }
}
