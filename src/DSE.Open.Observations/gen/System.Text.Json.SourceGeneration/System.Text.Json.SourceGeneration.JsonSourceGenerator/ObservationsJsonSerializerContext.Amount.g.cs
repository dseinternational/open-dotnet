﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Amount>? _Amount;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Amount> Amount
        #nullable enable annotations
        {
            get => _Amount ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Amount>)Options.GetTypeInfo(typeof(global::DSE.Open.Values.Amount));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Amount> Create_Amount(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Values.Amount>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Values.Amount> jsonTypeInfo))
            {
                global::System.Text.Json.Serialization.JsonConverter converter = ExpandConverter(typeof(global::DSE.Open.Values.Amount), new global::DSE.Open.Values.Text.Json.Serialization.JsonDecimalValueConverter<global::DSE.Open.Values.Amount>(), options);
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateValueInfo<global::DSE.Open.Values.Amount> (options, converter);
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }
    }
}