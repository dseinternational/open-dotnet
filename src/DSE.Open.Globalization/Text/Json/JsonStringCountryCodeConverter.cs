// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json;

namespace DSE.Open.Globalization.Text.Json;

public class JsonStringCountryCodeConverter : SpanParsableCharWritingJsonConverter<CountryCode>
{
    public static readonly JsonStringCountryCodeConverter Default = new();

    protected override int GetMaxCharCountToWrite(CountryCode value) => CountryCode.Length;
}
