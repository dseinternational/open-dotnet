// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Helpers for throwing <see cref="JsonException"/> instances.
/// </summary>
public static class JsonExceptionHelper
{
    /// <summary>
    /// Throws a <see cref="JsonException"/> with the specified message.
    /// </summary>
    public static void ThrowJsonException(string? message)
    {
        throw new JsonException(message);
    }

    /// <summary>
    /// Throws a <see cref="JsonException"/> with the specified message and inner exception.
    /// </summary>
    public static void ThrowJsonException(string? message, Exception? innerException)
    {
        throw new JsonException(message, innerException);
    }

    /// <summary>
    /// Throws a <see cref="JsonException"/> with the specified message, path, line number,
    /// byte position in line, and inner exception.
    /// </summary>
    public static void ThrowJsonException(
        string? message,
        string? path,
        long? lineNumber,
        long? bytePositionInLine,
        Exception? innerException)
    {
        throw new JsonException(message, path, lineNumber, bytePositionInLine, innerException);
    }
}
