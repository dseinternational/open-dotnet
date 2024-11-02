﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordMeasure>? _BinaryWordMeasure;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordMeasure> BinaryWordMeasure
        #nullable enable annotations
        {
            get => _BinaryWordMeasure ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordMeasure>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.BinaryWordMeasure));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordMeasure> Create_BinaryWordMeasure(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.BinaryWordMeasure>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.BinaryWordMeasure> jsonTypeInfo))
            {
                var objectInfo = new global::System.Text.Json.Serialization.Metadata.JsonObjectInfoValues<global::DSE.Open.Observations.BinaryWordMeasure>
                {
                    ObjectCreator = null,
                    ObjectWithParameterizedConstructorCreator = static args => new global::DSE.Open.Observations.BinaryWordMeasure((global::DSE.Open.Observations.MeasureId)args[0], (global::System.Uri)args[1], (string)args[2], (string)args[3]){ Id = (global::DSE.Open.Observations.MeasureId)args[0], Uri = (global::System.Uri)args[1], MeasurementLevel = (global::DSE.Open.Observations.MeasurementLevel)args[4], Name = (string)args[2], Statement = (string)args[3] },
                    PropertyMetadataInitializer = _ => BinaryWordMeasurePropInit(options),
                    ConstructorParameterMetadataInitializer = BinaryWordMeasureCtorParamInit,
                    ConstructorAttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.BinaryWordMeasure).GetConstructor(InstanceMemberBindingFlags, binder: null, new[] {typeof(global::DSE.Open.Observations.MeasureId), typeof(global::System.Uri), typeof(string), typeof(string)}, modifiers: null),
                    SerializeHandler = BinaryWordMeasureSerializeHandler,
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateObjectInfo<global::DSE.Open.Observations.BinaryWordMeasure>(options, objectInfo);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[] BinaryWordMeasurePropInit(global::System.Text.Json.JsonSerializerOptions options)
        {
            var properties = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[7];

            var info0 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.MeasureId>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Measure),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Measure)obj).Id,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Id",
                JsonPropertyName = "id",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Measure).GetProperty("Id", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.MeasureId), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[0] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.MeasureId>(options, info0);

            var info1 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.Uri>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Measure),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Measure)obj).Uri,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Uri",
                JsonPropertyName = "uri",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Measure).GetProperty("Uri", InstanceMemberBindingFlags, null, typeof(global::System.Uri), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[1] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.Uri>(options, info1);
            properties[1].IsGetNullable = false;
            properties[1].IsSetNullable = false;

            var info2 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Observations.MeasurementLevel>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Measure),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Measure)obj).MeasurementLevel,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "MeasurementLevel",
                JsonPropertyName = "level",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Measure).GetProperty("MeasurementLevel", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Observations.MeasurementLevel), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[2] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Observations.MeasurementLevel>(options, info2);

            var info3 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<string>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Measure),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Measure)obj).Name,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Name",
                JsonPropertyName = "name",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Measure).GetProperty("Name", InstanceMemberBindingFlags, null, typeof(string), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[3] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<string>(options, info3);
            properties[3].IsGetNullable = false;
            properties[3].IsSetNullable = false;

            var info4 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<string>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.Measure),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.Measure)obj).Statement,
                Setter = static (obj, value) => throw new global::System.InvalidOperationException("Setting init-only properties is not supported in source generation mode."),
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Statement",
                JsonPropertyName = "statement",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.Measure).GetProperty("Statement", InstanceMemberBindingFlags, null, typeof(string), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[4] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<string>(options, info4);
            properties[4].IsGetNullable = false;
            properties[4].IsSetNullable = false;

            var info5 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::System.Collections.Generic.IReadOnlyDictionary<string, object>>
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
            
            properties[5] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::System.Collections.Generic.IReadOnlyDictionary<string, object>>(options, info5);
            properties[5].IsGetNullable = false;

            var info6 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<bool>
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
            
            properties[6] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<bool>(options, info6);

            return properties;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void BinaryWordMeasureSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Observations.BinaryWordMeasure? value)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            
            writer.WriteStartObject();

            writer.WritePropertyName(PropName_id);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.Measure)value).Id, MeasureId);
            writer.WritePropertyName(PropName_uri);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.Measure)value).Uri, Uri);
            writer.WritePropertyName(PropName_level);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.Measure)value).MeasurementLevel, MeasurementLevel);
            writer.WriteString(PropName_name, ((global::DSE.Open.Observations.Measure)value).Name);
            writer.WriteString(PropName_statement, ((global::DSE.Open.Observations.Measure)value).Statement);

            writer.WriteEndObject();
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[] BinaryWordMeasureCtorParamInit() => new global::System.Text.Json.Serialization.Metadata.JsonParameterInfoValues[]
        {
            new()
            {
                Name = "id",
                ParameterType = typeof(global::DSE.Open.Observations.MeasureId),
                Position = 0,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "uri",
                ParameterType = typeof(global::System.Uri),
                Position = 1,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "name",
                ParameterType = typeof(string),
                Position = 2,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "statement",
                ParameterType = typeof(string),
                Position = 3,
                HasDefaultValue = false,
                DefaultValue = null,
                IsNullable = false,
            },

            new()
            {
                Name = "MeasurementLevel",
                ParameterType = typeof(global::DSE.Open.Observations.MeasurementLevel),
                Position = 4,
                IsMemberInitializer = true,
            },
        };
    }
}
