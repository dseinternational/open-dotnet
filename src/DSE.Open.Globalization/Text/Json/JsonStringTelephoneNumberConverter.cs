// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Globalization.Text.Json;

public class JsonStringTelephoneNumberConverter : SpanParsableCharWritingJsonConverter<TelephoneNumber>
{
    public static readonly JsonStringTelephoneNumberConverter Default = new();

    protected override int GetMaxCharCountToWrite(TelephoneNumber value) => TelephoneNumber.MaxFormattedLength;
}
