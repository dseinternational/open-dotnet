﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonSByteValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, sbyte>
{
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (TValue)reader.GetSByte();

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);
        writer.WriteNumberValue((sbyte)value);
    }
}
