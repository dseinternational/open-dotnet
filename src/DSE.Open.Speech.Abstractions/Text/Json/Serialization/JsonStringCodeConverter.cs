// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Text.Json.Serialization;

public class JsonStringPhonemeConverter : SpanParsableCharWritingJsonConverter<Phoneme>
{
    public static readonly JsonStringPhonemeConverter Default = new();

    protected override int GetMaxCharCountToWrite(Phoneme value) => Phoneme.MaxLength;
}
