﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Count>? _Count;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Count> Count
        #nullable enable annotations
        {
            get => _Count ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Count>)Options.GetTypeInfo(typeof(global::DSE.Open.Values.Count));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Count> Create_Count(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Values.Count>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Count> jsonTypeInfo))
            {
                global::System.Text.Json.Serialization.JsonConverter converter = ExpandConverter(typeof(global::DSE.Open.Values.Count), new global::DSE.Open.Values.Text.Json.Serialization.JsonInt64ValueConverter<global::DSE.Open.Values.Count>(), options);
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateValueInfo<global::DSE.Open.Values.Count> (options, converter);
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }
    }
}
