// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonStringCodeConverter : SpanParsableCharWritingJsonConverter<Code>
{
    public static readonly JsonStringCodeConverter Default = new();

    protected override int GetMaxCharCountToWrite(Code value) => Code.MaxLength;
}
