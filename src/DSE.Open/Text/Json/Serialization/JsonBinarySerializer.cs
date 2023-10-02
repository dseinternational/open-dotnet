// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Helpers for reading/writing Json serialized data to/from memory.
/// </summary>
public static class JsonBinarySerializer
{
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static ReadOnlyMemory<byte> SerializeToUtf8Json<T>(T value, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var jsonWriter = new Utf8JsonWriter(bufferWriter);
        JsonSerializer.Serialize(jsonWriter, value, jsonSerializerOptions);
        return bufferWriter.WrittenMemory;
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static T? DeserializeFromUtf8Json<T>(ReadOnlySpan<byte> json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static string SerializeToBase64Utf8Json<T>(T value, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;
        return Convert.ToBase64String(SerializeToUtf8Json(value, jsonSerializerOptions).Span);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static T? DeserializeFromBase64Utf8Json<T>(string base64, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Guard.IsNotNull(base64);
        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;
        return DeserializeFromUtf8Json<T>(Convert.FromBase64String(base64), jsonSerializerOptions);
    }
}
