// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Globalization;

namespace DSE.Open.Requests.Tests;

public class LocalizedRequestTests
{
    [Fact]
    public void SerializeDeserialize_RoundtripsLanguages()
    {
        var request = new LocalizedRequest
        {
            Language = LanguageTag.EnglishUk,
            FormatLanguage = LanguageTag.EnglishUs,
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<LocalizedRequest>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(LanguageTag.EnglishUk, deserialized.Language);
        Assert.Equal(LanguageTag.EnglishUs, deserialized.FormatLanguage);
    }

    [Fact]
    public void SerializeDeserialize_PreservesRequestId()
    {
        var request = new LocalizedRequest
        {
            RequestId = new RequestId("req_abc"),
            Language = LanguageTag.EnglishUk,
            FormatLanguage = LanguageTag.EnglishUk,
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<LocalizedRequest>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(request.RequestId, deserialized.RequestId);
    }
}
