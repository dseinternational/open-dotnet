// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Writes and reads <see cref="TimeSpan"/> values as JSON numbers representing
/// the total seconds in the value.
/// </summary>
public class JsonTimeSpanSecondsConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return TimeSpan.FromSeconds(reader.GetInt64());
    }

    public override void Write(
        Utf8JsonWriter writer,
        TimeSpan value,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((long)value.TotalSeconds);
    }
}
