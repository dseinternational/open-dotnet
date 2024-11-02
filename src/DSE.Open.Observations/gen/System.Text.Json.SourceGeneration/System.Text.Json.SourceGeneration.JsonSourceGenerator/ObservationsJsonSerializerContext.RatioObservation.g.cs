﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.RatioObservation>? _RatioObservation;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.RatioObservation> RatioObservation
        #nullable enable annotations
        {
            get => _RatioObservation ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.RatioObservation>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.RatioObservation));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.RatioObservation> Create_RatioObservation(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.RatioObservation>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.RatioObservation> jsonTypeInfo))
            {
                var objectInfo = new global::System.Text.Json.Serialization.Metadata.JsonObjectInfoValues<global::DSE.Open.Observations.RatioObservation>
                {
                    ObjectCreator = null,
                    ObjectWithParameterizedConstructorCreator = static args => new global::DSE.Open.Observations.RatioObservation(){ Value = (global::DSE.Open.Values.Ratio)args[0], Id = (global::DSE.Open.Observations.ObservationId)args[1], Time = (global::System.DateTimeOffset)args[2], MeasureId = (global::DSE.Open.Observations.MeasureId)args[3] },
                    PropertyMetadataInitializer = _ => RatioObservationPropInit(options),
                    ConstructorParameterMetadataInitializer = RatioObservationCtorParamInit,
                    ConstructorAttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.RatioObservation).GetConstructor(InstanceMemberBindingFlags, binder: null, global::System.Array.Empty<global::System.Type>(), modifiers: null),
                    SerializeHandler = null,
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateObjectInfo<global::DSE.Open.Observations.RatioObservation>(options, objectInfo);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[] RatioObservationPropInit(global::System.Text.Json.JsonSerializerOptions options)
        {
            var properties = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[6];

            var info0 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Ratio>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Observation<global::DSE.Open.Values.Ratio>),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Observation<global::DSE.Open.Values.Ratio>)obj).Value,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Value",
                JsonPropertyName = "v",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Observation<global::DSE.Open.Values.Ratio>).GetProperty("Value", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Ratio), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[0] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Ratio>(options, info0);
            properties[0].IsRequired = true;
            properties[0].Order = -1;

            var info1 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.ObservationId>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Observation),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Observation)obj).Id,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Id",
                JsonPropertyName = "i",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Observation).GetProperty("Id", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.ObservationId), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[1] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.ObservationId>(options, info1);
            properties[1].Order = -91000;

            var info2 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.DateTimeOffset>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Observation),
                Converter = (global::System.Text.Json.Serialization.JsonConverter<global::System.DateTimeOffset>)ExpandConverter(typeof(global::System.DateTimeOffset), new global::DSE.Open.Text.Json.Serialization.JsonDateTimeOffsetUnixTimeMillisecondsConverter(), options),
                Getter = static obj => ((global::DSE.Open.Observations.Observation)obj).Time,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Time",
                JsonPropertyName = "t",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Observation).GetProperty("Time", InstanceMemberBindingFlags, null, typeof(global::System.DateTimeOffset), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[2] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.DateTimeOffset>(options, info2);
            properties[2].IsRequired = true;
            properties[2].Order = -89800;

            var info3 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.MeasureId>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Observation),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Observation)obj).MeasureId,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "MeasureId",
                JsonPropertyName = "m",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Observation).GetProperty("MeasureId", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.MeasureId), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[3] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.MeasureId>(options, info3);
            properties[3].IsRequired = true;
            properties[3].Order = -88000;

            var info4 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.Collections.Generic.IReadOnlyDictionary<string, object>>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Serialization.DataTransfer.ImmutableDataTransferObject),
                Converter = null,
                Getter = null,
                Setter = null,
                IgnoreCondition = global::System.Text.Json.Serialization.JsonIgnoreCondition.Always,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "ExtensionData",
                JsonPropertyName = null,
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Serialization.DataTransfer.ImmutableDataTransferObject).GetProperty("ExtensionData", InstanceMemberBindingFlags, null, typeof(global::System.Collections.Generic.IReadOnlyDictionary<string, object>), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[4] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.Collections.Generic.IReadOnlyDictionary<string, object>>(options, info4);
            properties[4].IsGetNullable = false;

            var info5 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<bool>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Serialization.DataTransfer.ImmutableDataTransferObject),
                Converter = null,
                Getter = null,
                Setter = null,
                IgnoreCondition = global::System.Text.Json.Serialization.JsonIgnoreCondition.Always,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "HasExtensionData",
                JsonPropertyName = null,
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Serialization.DataTransfer.ImmutableDataTransferObject).GetProperty("HasExtensionData", InstanceMemberBindingFlags, null, typeof(bool), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[5] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<bool>(options, info5);

            return properties;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[] RatioObservationCtorParamInit() => new global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[]
        {
            new()
            {
                Name = "Value",
                ParameterType = typeof(global::DSE.Open.Values.Ratio),
                Position = 0,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Id",
                ParameterType = typeof(global::DSE.Open.Observations.ObservationId),
                Position = 1,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Time",
                ParameterType = typeof(global::System.DateTimeOffset),
                Position = 2,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "MeasureId",
                ParameterType = typeof(global::DSE.Open.Observations.MeasureId),
                Position = 3,
                IsMemberInitializer = true,
            },
        };
    }
}
