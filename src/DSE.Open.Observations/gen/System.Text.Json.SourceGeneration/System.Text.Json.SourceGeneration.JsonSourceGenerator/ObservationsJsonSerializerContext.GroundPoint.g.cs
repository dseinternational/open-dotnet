﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0612, CS0618

namespace DSE.Open.Observations
{
    public partial class ObservationsJsonSerializerContext
    {
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.GroundPoint>? _GroundPoint;
        
        /// <summary>
        /// Defines the source generated JSON serialization contract metadata for a given type.
        /// </summary>
        #nullable disable annotations // Marking the property type as nullable-oblivious.
        public global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.GroundPoint> GroundPoint
        #nullable enable annotations
        {
            get => _GroundPoint ??= (global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.GroundPoint>)Options.GetTypeInfo(typeof(global::DSE.Open.Observations.GroundPoint));
        }
        
        private global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.GroundPoint> Create_GroundPoint(global::System.Text.Json.JsonSerializerOptions options)
        {
            if (!TryGetTypeInfoForRuntimeCustomConverter<global::DSE.Open.Observations.GroundPoint>(options, out global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<global::DSE.Open.Observations.GroundPoint> jsonTypeInfo))
            {
                var objectInfo = new global::System.Text.Json.Serialization.Metadata.JsonObjectInfoValues<global::DSE.Open.Observations.GroundPoint>
                {
                    ObjectCreator = () => new global::DSE.Open.Observations.GroundPoint(),
                    ObjectWithParameterizedConstructorCreator = null,
                    PropertyMetadataInitializer = _ => GroundPointPropInit(options),
                    ConstructorParameterMetadataInitializer = null,
                    ConstructorAttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.GroundPoint).GetConstructor(InstanceMemberBindingFlags, binder: null, global::System.Array.Empty<global::System.Type>(), modifiers: null),
                    SerializeHandler = GroundPointSerializeHandler,
                };
                
                jsonTypeInfo = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreateObjectInfo<global::DSE.Open.Observations.GroundPoint>(options, objectInfo);
                jsonTypeInfo.NumberHandling = null;
            }
        
            jsonTypeInfo.OriginatingResolver = this;
            return jsonTypeInfo;
        }

        private static global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[] GroundPointPropInit(global::System.Text.Json.JsonSerializerOptions options)
        {
            var properties = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfo[3];

            var info0 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<double>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.GroundPoint),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.GroundPoint)obj).Latitude,
                Setter = null,
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Latitude",
                JsonPropertyName = "latitude",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.GroundPoint).GetProperty("Latitude", InstanceMemberBindingFlags, null, typeof(double), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[0] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<double>(options, info0);

            var info1 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<double>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.GroundPoint),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.GroundPoint)obj).Longitude,
                Setter = null,
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Longitude",
                JsonPropertyName = "longitude",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.GroundPoint).GetProperty("Longitude", InstanceMemberBindingFlags, null, typeof(double), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[1] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<double>(options, info1);

            var info2 = new global::System.Text.Json.Serialization.Metadata.JsonPropertyInfoValues<global::DSE.Open.Values.Units.Length>
            {
                IsProperty = true,
                IsPublic = true,
                IsVirtual = false,
                DeclaringType = typeof(global::DSE.Open.Observations.GroundPoint),
                Converter = null,
                Getter = static obj => ((global::DSE.Open.Observations.GroundPoint)obj).Accuracy,
                Setter = null,
                IgnoreCondition = null,
                HasJsonInclude = false,
                IsExtensionData = false,
                NumberHandling = null,
                PropertyName = "Accuracy",
                JsonPropertyName = "accuracy",
                AttributeProviderFactory = static () => typeof(global::DSE.Open.Observations.GroundPoint).GetProperty("Accuracy", InstanceMemberBindingFlags, null, typeof(global::DSE.Open.Values.Units.Length), global::System.Array.Empty<global::System.Type>(), null),
            };
            
            properties[2] = global::System.Text.Json.Serialization.Metadata.JsonMetadataServices.CreatePropertyInfo<global::DSE.Open.Values.Units.Length>(options, info2);

            return properties;
        }

        // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
        // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
        private void GroundPointSerializeHandler(global::System.Text.Json.Utf8JsonWriter writer, global::DSE.Open.Observations.GroundPoint value)
        {
            writer.WriteStartObject();

            writer.WriteNumber(PropName_latitude, ((global::DSE.Open.Observations.GroundPoint)value).Latitude);
            writer.WriteNumber(PropName_longitude, ((global::DSE.Open.Observations.GroundPoint)value).Longitude);
            writer.WritePropertyName(PropName_accuracy);
            global::System.Text.Json.JsonSerializer.Serialize(writer, ((global::DSE.Open.Observations.GroundPoint)value).Accuracy, Length);

            writer.WriteEndObject();
        }
    }
}
