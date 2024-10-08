// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Helpers for reading/writing Json serialized data to/from memory.
/// </summary>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public static class JsonBinarySerializer
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = JsonSharedOptions.RelaxedJsonEscaping;

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static ReadOnlyMemory<byte> SerializeToUtf8Json<T>(T value, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= s_jsonSerializerOptions;
        return JsonSerializer.SerializeToUtf8Bytes(value, jsonSerializerOptions);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static T? DeserializeFromUtf8Json<T>(ReadOnlySpan<byte> json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= s_jsonSerializerOptions;
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static bool TryDeserializeFromUtf8Json<T>(ReadOnlySpan<byte> json, out T? value)
    {
        return TryDeserializeFromUtf8Json(json, null, out value);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static bool TryDeserializeFromUtf8Json<T>(ReadOnlySpan<byte> json, JsonSerializerOptions? jsonSerializerOptions, out T? value)
    {
        jsonSerializerOptions ??= s_jsonSerializerOptions;

        try
        {
            value = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
            return true;
        }
        catch (JsonException)
        {
            value = default;
            return false;
        }
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static string SerializeToBase64Utf8Json<T>(T value, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= s_jsonSerializerOptions;
        return Convert.ToBase64String(SerializeToUtf8Json(value, jsonSerializerOptions).Span);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static T? DeserializeFromBase64Utf8Json<T>(string base64, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        ArgumentNullException.ThrowIfNull(base64);
        jsonSerializerOptions ??= s_jsonSerializerOptions;
        return DeserializeFromUtf8Json<T>(Convert.FromBase64String(base64), jsonSerializerOptions);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static bool TryDeserializeFromBase64Utf8Json<T>(string base64, out T? value)
    {
        return TryDeserializeFromBase64Utf8Json(base64, null, out value);
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    [SkipLocalsInit]
    public static bool TryDeserializeFromBase64Utf8Json<T>(string base64, JsonSerializerOptions? jsonSerializerOptions, out T? value)
    {
        ArgumentNullException.ThrowIfNull(base64);

        jsonSerializerOptions ??= s_jsonSerializerOptions;

        var byteLength = (int)(3 * Math.Ceiling((double)base64.Length / 4));

        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(byteLength)
            ? stackalloc byte[byteLength]
            : (rented = SpanOwner<byte>.Allocate(byteLength)).Span;

        using (rented)
        {
            if (Convert.TryFromBase64String(base64, buffer, out var bytesWritten))
            {
                return TryDeserializeFromUtf8Json(buffer[..bytesWritten], jsonSerializerOptions, out value);
            }

            value = default;
            return false;
        }
    }
}
