// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public class DateTimeOffsetUnixTimeMillisecondsJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset value,
        JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);
        writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
    }
}
