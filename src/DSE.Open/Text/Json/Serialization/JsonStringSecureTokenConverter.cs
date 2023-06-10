// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringSecureTokenConverter : SpanParsableCharWritingJsonConverter<SecureToken>
{
    public static readonly JsonStringSecureTokenConverter Default = new();

    protected override int GetMaxCharCountToWrite(SecureToken value) => SecureToken.MaxTokenLength;
}
