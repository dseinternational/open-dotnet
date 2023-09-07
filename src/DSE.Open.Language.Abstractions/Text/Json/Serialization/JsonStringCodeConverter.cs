// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Text.Json.Serialization;

public class JsonStringSignConverter : SpanParsableCharWritingJsonConverter<Sign>
{
    public static readonly JsonStringSignConverter Default = new();

    protected override int GetMaxCharCountToWrite(Sign value) => Sign.MaxSerializedCharLength;
}
