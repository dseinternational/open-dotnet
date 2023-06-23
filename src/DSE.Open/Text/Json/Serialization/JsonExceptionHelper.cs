// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

public static class JsonExceptionHelper
{
    public static void ThrowJsonException(string? message)
        => throw new JsonException(message);

    public static void ThrowJsonException(string? message, Exception? innerException)
        => throw new JsonException(message, innerException);

    public static void ThrowJsonException(
        string? message,
        string? path,
        long? lineNumber,
        long? bytePositionInLine,
        Exception? innerException)
        => throw new JsonException(message, path, lineNumber, bytePositionInLine, innerException);
}
