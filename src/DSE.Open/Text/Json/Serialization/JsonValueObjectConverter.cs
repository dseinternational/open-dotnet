// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Deserializes <see cref="object"/> values by converting Json values to a corresponding .NET type.
/// Serializes <see cref="object"/> values by converting them to a value corresponding to the underlying type.
/// <b>Note</b>: when serializing values using this converter, all properties of the underlying type may be
/// considered for serialization.
/// </summary>
/// <remarks>
/// The main purpose of this converter is to allow deserializing Json values to <see cref="object"/>
/// values, the underlying types of which correspond to basic .NET types - rather than the default
/// behaviour, which is to deserialize such values as <see cref="JsonElement"/> values.
/// </remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class JsonValueObjectConverter : JsonConverter<object>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonValueObjectConverter Default = new();

    /// <inheritdoc/>
    public override object Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt64(out var l) => l,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTime(out var datetime) => datetime,
            JsonTokenType.String => reader.GetString()!,
            JsonTokenType.Null => null!,
            JsonTokenType.StartObject or JsonTokenType.StartArray => JsonDocument.ParseValue(ref reader).RootElement.Clone(),
            JsonTokenType.None
                or JsonTokenType.EndObject
                or JsonTokenType.EndArray
                or JsonTokenType.PropertyName
                or JsonTokenType.Comment => throw new JsonException($"Unexpected JSON token: {reader.TokenType}."),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };
    }

    /// <inheritdoc/>
    public override void Write(
        Utf8JsonWriter writer,
        object value,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(string), options);
    }
}
