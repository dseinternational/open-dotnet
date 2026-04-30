// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="SecureToken"/> values as JSON strings.
/// </summary>
public sealed class JsonStringSecureTokenConverter : SpanParsableCharWritingJsonConverter<SecureToken>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringSecureTokenConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(SecureToken value)
    {
        return SecureToken.MaxTokenLength;
    }
}
