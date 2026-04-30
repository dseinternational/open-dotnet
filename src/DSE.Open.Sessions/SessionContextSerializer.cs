// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Sessions;

/// <summary>
/// Serializes and deserializes <see cref="SessionContext"/> instances to and from
/// UTF-8 JSON, including a Base64-encoded form suitable for transport.
/// </summary>
public static class SessionContextSerializer
{
    /// <summary>
    /// Serializes the specified <paramref name="value"/> to UTF-8 encoded JSON.
    /// </summary>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static ReadOnlyMemory<byte> SerializeToUtf8Json(SessionContext value)
    {
        return JsonBinarySerializer.SerializeToUtf8Json(value);
    }

    /// <summary>
    /// Deserializes a <see cref="SessionContext"/> from the specified UTF-8 encoded
    /// JSON <paramref name="json"/>.
    /// </summary>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static SessionContext? DeserializeFromUtf8Json(ReadOnlySpan<byte> json)
    {
        return JsonBinarySerializer.DeserializeFromUtf8Json<SessionContext>(json);
    }

    /// <summary>
    /// Serializes the specified <paramref name="value"/> to UTF-8 encoded JSON and
    /// returns the result as a Base64-encoded string.
    /// </summary>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static string SerializeToBase64Utf8Json(SessionContext value)
    {
        return JsonBinarySerializer.SerializeToBase64Utf8Json(value);
    }

    /// <summary>
    /// Deserializes a <see cref="SessionContext"/> from the specified
    /// Base64-encoded UTF-8 JSON string.
    /// </summary>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static SessionContext? DeserializeFromBase64Utf8Json(string base64)
    {
        return JsonBinarySerializer.DeserializeFromBase64Utf8Json<SessionContext>(base64);
    }

    /// <summary>
    /// Attempts to deserialize a <see cref="SessionContext"/> from the specified
    /// Base64-encoded UTF-8 JSON string.
    /// </summary>
    /// <returns><see langword="true"/> if deserialization succeeded and the result
    /// is non-<see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static bool TryDeserializeFromBase64Utf8Json(
        string base64,
        [NotNullWhen(true)] out SessionContext? sessionContext)
    {
        return JsonBinarySerializer.TryDeserializeFromBase64Utf8Json(base64, out sessionContext)
               && sessionContext is not null;
    }
}
