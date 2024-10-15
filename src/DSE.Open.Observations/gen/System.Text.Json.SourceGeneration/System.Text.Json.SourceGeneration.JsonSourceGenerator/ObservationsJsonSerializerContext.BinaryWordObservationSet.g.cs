﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordObservationSet>? _BinaryWordObservationSet;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordObservationSet> BinaryWordObservationSet
        #nullable enable annotations
        {
            get => _BinaryWordObservationSet ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordObservationSet>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.BinaryWordObservationSet));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordObservationSet> Create_BinaryWordObservationSet(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.BinaryWordObservationSet>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordObservationSet> jsonTypeInfo))
            {
                var objectInfo = new global::System.Text.Json.Serialization.Metadata.JsonObjectInfoValues<global::DSE.Open.Observations.BinaryWordObservationSet>
                {
                    ObjectCreator = null,
                    ObjectWithParameterizedConstructorCreator = static args => new global::DSE.Open.Observations.BinaryWordObservationSet((global::DSE.Open.Observations.ObservationSetId)args[0], (long)args[1], (global::DSE.Open.Values.Identifier)args[2], (global::DSE.Open.Values.Identifier)args[3], (global::System.Uri)args[4], (global::DSE.Open.Observations.GroundPoint?)args[5], (global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>)args[6]),
                    PropertyMetadataInitializer = _ => BinaryWordObservationSetPropInit(options),
                    ConstructorParameterMetadataInitializer = BinaryWordObservationSetCtorParamInit,
                    ConstructorAttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.BinaryWordObservationSet).GetConstructor(InstanceMemberBindingFlags, binder: null, new[] {typeof(global::DSE.Open.Observations.ObservationSetId), typeof(long), typeof(global::DSE.Open.Values.Identifier), typeof(global::DSE.Open.Values.Identifier), typeof(global::System.Uri), typeof(global::DSE.Open.Observations.GroundPoint?), typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>)}, modifiers: null),
                    SerializeHandler = BinaryWordObservationSetSerializeHandler,
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateObjectInfo<global::DSE.Open.Observations.BinaryWordObservationSet>(options, objectInfo);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[] BinaryWordObservationSetPropInit(global::System.Text.Json.JsonSerializerOptions options)
        {
            var properties = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[8];

            var info0 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.BinaryWordObservation, bool>),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.BinaryWordObservation, bool>)obj).Observations,
                Setter = null,
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Observations",
                JsonPropertyName = "obs",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.BinaryWordObservation, bool>).GetProperty("Observations", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[0] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>>(options, info0);
            properties[0].Order = 900000;
            properties[0].IsGetNullable = false;

            var info1 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.ObservationSetId>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Id,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.Id' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
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
                Converter = null,
                Getter = null,
                Setter = null,
                IgnoreCondition = global::System.Text.Json.Serialization.JsonIgnoreCondition.Always,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Created",
                JsonPropertyName = null,
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Created", InstanceMemberBindingFlags, null, typeof(global::System.DateTimeOffset), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[2] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.DateTimeOffset>(options, info2);

            var info3 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<long>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).CreatedTimestamp,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.CreatedTimestamp' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "CreatedTimestamp",
                JsonPropertyName = "crt",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("CreatedTimestamp", InstanceMemberBindingFlags, null, typeof(long), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[3] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<long>(options, info3);
            properties[3].Order = -97800;

            var info4 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Identifier>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).TrackerReference,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.TrackerReference' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "TrackerReference",
                JsonPropertyName = "trk",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("TrackerReference", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Identifier), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[4] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Identifier>(options, info4);
            properties[4].Order = -90000;

            var info5 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Identifier>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).ObserverReference,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.ObserverReference' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "ObserverReference",
                JsonPropertyName = "obr",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("ObserverReference", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Identifier), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[5] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Identifier>(options, info5);
            properties[5].Order = -89000;

            var info6 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.Uri>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Source,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.Source' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
                IgnoreCondition = null,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Source",
                JsonPropertyName = "src",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Source", InstanceMemberBindingFlags, null, typeof(global::System.Uri), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[6] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.Uri>(options, info6);
            properties[6].Order = -60000;
            properties[6].IsGetNullable = false;

            var info7 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.GroundPoint?>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.ObservationSet),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.ObservationSet)obj).Location,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("The member 'BinaryWordObservationSet.Location' has been annotated with the JsonIncludeAttribute but is not visible to the source generator."),
                IgnoreCondition = global::System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
                HasJsonInclude = true,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Location",
                JsonPropertyName = "loc",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.ObservationSet).GetProperty("Location", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.GroundPoint?), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[7] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.GroundPoint?>(options, info7);
            properties[7].Order = -50000;

            return properties;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void BinaryWordObservationSetSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Observations.BinaryWordObservationSet? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartObject();

            writer.WritePropertyName(PropName_id);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.ObservationSet)value).Id, ObservationSetId);
            writer.WriteNumber(PropName_crt, ((global::DSE.Open.Observations.ObservationSet)value).CreatedTimestamp);
            writer.WritePropertyName(PropName_trk);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.ObservationSet)value).TrackerReference, Identifier);
            writer.WritePropertyName(PropName_obr);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.ObservationSet)value).ObserverReference, Identifier);
            writer.WritePropertyName(PropName_src);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.ObservationSet)value).Source, Uri);
            global::DSE.Open.Observations.GroundPoint? __value_Location = ((global::DSE.Open.Observations.ObservationSet)value).Location;
            if (__value_Location is not null)
            {
                writer.WritePropertyName(PropName_loc);
                global::System.Text.Json.JsonSerializer.Serialize(writer, __value_Location, NullableGroundPoint);
            }
            writer.WritePropertyName(PropName_obs);
            ReadOnlyValueCollectionBinaryWordObservationSerializeHandler(writer, ((global::DSE.Open.Observations.ObservationSet<global::DSE.Open.Observations.BinaryWordObservation, bool>)value).Observations);

            writer.WriteEndObject();
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[] BinaryWordObservationSetCtorParamInit() => new global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[]
        {
            new()
            {
                Name = "id",
                ParameterType = typeof(global::DSE.Open.Observations.ObservationSetId),
                Position = 0,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "createdTimestamp",
                ParameterType = typeof(long),
                Position = 1,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "trackerReference",
                ParameterType = typeof(global::DSE.Open.Values.Identifier),
                Position = 2,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "observerReference",
                ParameterType = typeof(global::DSE.Open.Values.Identifier),
                Position = 3,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "source",
                ParameterType = typeof(global::System.Uri),
                Position = 4,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "location",
                ParameterType = typeof(global::DSE.Open.Observations.GroundPoint?),
                Position = 5,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = true,
            },

            new()
            {
                Name = "observations",
                ParameterType = typeof(global::DSE.Open.Collections.Generic.ReadOnlyValueCollection<global::DSE.Open.Observations.BinaryWordObservation>),
                Position = 6,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },
        };
    }
}
