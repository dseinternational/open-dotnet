﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.CountObservationSet>? _CountObservationSet;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.CountObservationSet> CountObservationSet
        #nullable enable annotations
        {
            get => _CountObservationSet ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.CountObservationSet>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.CountObservationSet));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.CountObservationSet> Create_CountObservationSet(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.CountObservationSet>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.CountObservationSet> jsonTypeInfo))
            {
                var objectInfo = new global::System.Text.Json.Serialization.Metadata.JsonObjectInfoValues<global::DSE.Open.Observations.CountObservationSet>
                {
                    ObjectCreator = null,
                    ObjectWithParameterizedConstructorCreator = static args => new global::DSE.Open.Observations.CountObservationSet(){ Observations = (global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>)args[0], Id = (global::DSE.Open.Observations.ObservationSetId)args[1], Created = (global::System.DateTimeOffset)args[2], TrackerReference = (global::DSE.Open.Values.Identifier)args[3], ObserverReference = (global::DSE.Open.Values.Identifier)args[4], Source = (global::System.Uri)args[5], Location = (global::DSE.Open.Observations.GroundPoint?)args[6] },
                    PropertyMetadataInitializer = _ => CountObservationSetPropInit(options),
                    ConstructorParameterMetadataInitializer = CountObservationSetCtorParamInit,
                    ConstructorAttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.CountObservationSet).GetConstructor(InstanceMemberBindingFlags, binder: null, global::System.Array.Empty<global::System.Type>(), modifiers: null),
                    SerializeHandler = null,
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateObjectInfo<global::DSE.Open.Observations.CountObservationSet>(options, objectInfo);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[] CountObservationSetPropInit(global::System.Text.Json.JsonSerializerOptions options)
        {
            var properties = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[7];

            var info0 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.CountObservation, global::DSE.Open.Values.Count>),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.CountObservation, global::DSE.Open.Values.Count>)obj).Observations,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Observations",
                JsonPropertyName = "obs",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.CountObservation, global::DSE.Open.Values.Count>).GetProperty("Observations", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[0] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>>(options, info0);
            properties[0].IsRequired = true;
            properties[0].Order = 900000;
            properties[0].IsGetNullable = false;
            properties[0].IsSetNullable = false;

            var info1 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.ObservationSetId>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Id,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Id",
                JsonPropertyName = "id",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Id", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.ObservationSetId), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[1] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.ObservationSetId>(options, info1);
            properties[1].Order = -98000;

            var info2 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.DateTimeOffset>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = (global::System.Text.Json.Serialization.JsonConverter<global::System.DateTimeOffset>)ExpandConverter(typeof(global::System.DateTimeOffset), new global::DSE.Open.Text.Json.Serialization.JsonDateTimeOffsetUnixTimeMillisecondsConverter(), options),
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Created,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Created",
                JsonPropertyName = "crt",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Created", InstanceMemberBindingFlags, null, typeof(global::System.DateTimeOffset), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[2] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.DateTimeOffset>(options, info2);
            properties[2].IsRequired = true;
            properties[2].Order = -97800;

            var info3 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Identifier>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).TrackerReference,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "TrackerReference",
                JsonPropertyName = "trk",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("TrackerReference", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Identifier), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[3] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Identifier>(options, info3);
            properties[3].IsRequired = true;
            properties[3].Order = -90000;

            var info4 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Identifier>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).ObserverReference,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "ObserverReference",
                JsonPropertyName = "obr",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("ObserverReference", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Identifier), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[4] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Identifier>(options, info4);
            properties[4].IsRequired = true;
            properties[4].Order = -89000;

            var info5 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.Uri>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Source,
                Setter = static (obj, value) => ((global::DSE.Open.Observations.ObservationSet)obj).Source = value!,
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Source",
                JsonPropertyName = "src",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Source", InstanceMemberBindingFlags, null, typeof(global::System.Uri), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[5] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.Uri>(options, info5);
            properties[5].IsRequired = true;
            properties[5].Order = -60000;
            properties[5].IsGetNullable = false;
            properties[5].IsSetNullable = false;

            var info6 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.GroundPoint?>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Location,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Location",
                JsonPropertyName = "loc",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Location", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.GroundPoint?), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[6] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.GroundPoint?>(options, info6);
            properties[6].IsRequired = true;
            properties[6].Order = -50000;

            return properties;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[] CountObservationSetCtorParamInit() => new global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[]
        {
            new()
            {
                Name = "Observations",
                ParameterType = typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.CountObservation>),
                Position = 0,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Id",
                ParameterType = typeof(global::DSE.Open.Observations.ObservationSetId),
                Position = 1,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Created",
                ParameterType = typeof(global::System.DateTimeOffset),
                Position = 2,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "TrackerReference",
                ParameterType = typeof(global::DSE.Open.Values.Identifier),
                Position = 3,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "ObserverReference",
                ParameterType = typeof(global::DSE.Open.Values.Identifier),
                Position = 4,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Source",
                ParameterType = typeof(global::System.Uri),
                Position = 5,
                IsMemberInitializer = true,
            },

            new()
            {
                Name = "Location",
                ParameterType = typeof(global::DSE.Open.Observations.GroundPoint?),
                Position = 6,
                IsMemberInitializer = true,
            },
        };
    }
}
