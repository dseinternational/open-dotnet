// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Values.Units;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <see cref="Length"/> as a JSON string
/// using its parse/invariant string format.
/// </summary>
public class JsonStringLengthConverter : JsonConverter<Length>
{
    /// <summary>
    /// A shared default instance of the converter.
    /// </summary>
    public static readonly JsonStringLengthConverter Default = new();

    /// <inheritdoc/>
    public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (Length.TryParse(value, out var result))
            {
                return result;
            }
        }

        throw new JsonException();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Length value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        Guard.IsNotDefault(value);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
