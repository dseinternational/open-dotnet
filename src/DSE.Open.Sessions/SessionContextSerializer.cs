// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Sessions;

public static class SessionContextSerializer
{
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static ReadOnlyMemory<byte> SerializeToUtf8Json(SessionContext value)
        => JsonBinarySerializer.SerializeToUtf8Json(value);

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static SessionContext? DeserializeFromUtf8Json(ReadOnlySpan<byte> json)
        => JsonBinarySerializer.DeserializeFromUtf8Json<SessionContext>(json);

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static string SerializeToBase64Utf8Json(SessionContext value)
        => JsonBinarySerializer.SerializeToBase64Utf8Json(value);

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static SessionContext? DeserializeFromBase64Utf8Json(string base64)
        => JsonBinarySerializer.DeserializeFromBase64Utf8Json<SessionContext>(base64);

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static bool TryDeserializeFromBase64Utf8Json(
        string base64,
        [NotNullWhen(true)] out SessionContext? sessionContext)
        => JsonBinarySerializer.TryDeserializeFromBase64Utf8Json(base64, out sessionContext)
            && sessionContext is not null;
}
