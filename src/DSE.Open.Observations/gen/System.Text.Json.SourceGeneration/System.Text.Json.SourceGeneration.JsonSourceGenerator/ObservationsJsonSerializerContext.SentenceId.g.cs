﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Language.SentenceId>? _SentenceId;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Language.SentenceId> SentenceId
        #nullable enable annotations
        {
            get => _SentenceId ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Language.SentenceId>)Options.GetTypeInfo(typeof(global::DSE.Open.Language.SentenceId));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Language.SentenceId> Create_SentenceId(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Language.SentenceId>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Language.SentenceId> jsonTypeInfo))
            {
                global::System.Text.Json.Serialization.JsonConverter converter = ExpandConverter(typeof(global::DSE.Open.Language.SentenceId), new global::DSE.Open.Values.Text.Json.Serialization.JsonUInt64ValueConverter<global::DSE.Open.Language.SentenceId>(), options);
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateValueInfo<global::DSE.Open.Language.SentenceId> (options, converter);
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }
    }
}
