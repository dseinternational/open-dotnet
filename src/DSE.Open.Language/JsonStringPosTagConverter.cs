// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json;

namespace DSE.Open.Language;

public class JsonStringPosTagConverter : SpanParsableCharWritingJsonConverter<PosTag>
{
    public static readonly JsonStringPosTagConverter Default = new();

    protected override int GetMaxCharCountToWrite(PosTag value) => PosTag.MaxLength;
}
