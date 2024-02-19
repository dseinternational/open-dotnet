// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.Data.SqlClient;

namespace DSE.Open.Data.SqlClient;

public static class SqlDataReaderExtensions
{
    public static bool? GetNullableBoolean(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
    }

    public static byte? GetNullableByte(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetByte(ordinal);
    }

    public static DateTime? GetNullableDateTime(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }

    public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTimeOffset(ordinal);
    }

    public static decimal? GetNullableDecimal(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
    }

    public static double? GetNullableDouble(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDouble(ordinal);
    }

    public static float? GetNullableFloat(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetFloat(ordinal);
    }

    public static short? GetNullableInt16(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt16(ordinal);
    }

    public static int? GetNullableInt32(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }

    public static long? GetNullableInt64(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt64(ordinal);
    }

    public static string? GetNullableString(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    public static Utf8String GetUtf8String(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);

        var data = reader.GetSqlBytes(ordinal);

        if (data.IsNull)
        {
            SqlExceptionHelper.ThrowSqlNullValueException();
        }

        return data.Storage != StorageState.Buffer
            ? new Utf8String(data.Buffer.AsMemory()[..(int)data.Length])
            : new Utf8String(data.Value);
    }

    public static Utf8String? GetNullableUtf8String(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);

        var data = reader.GetSqlBytes(ordinal);

        if (data.IsNull)
        {
            return null;
        }

        return data.Storage != StorageState.Buffer
            ? new Utf8String(data.Buffer.AsMemory()[..(int)data.Length])
            : new Utf8String(data.Value);
    }

    public static Guid? GetNullableGuid(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetGuid(ordinal);
    }

    public static Uri GetUri(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);
        return new Uri(reader.GetString(ordinal));
    }

    public static Uri? GetNullableUri(this SqlDataReader reader, int ordinal)
    {
        Guard.IsNotNull(reader);

        return reader.IsDBNull(ordinal) ? null : new Uri(reader.GetString(ordinal));
    }

    public static T GetParseableValue<T>(this SqlDataReader reader, int ordinal, IFormatProvider? provider = null)
        where T : IParsable<T>
    {
        Guard.IsNotNull(reader);

        provider ??= CultureInfo.InvariantCulture;

        var value = reader.GetString(ordinal);

        return T.Parse(value, provider);
    }

    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    public static T? GetFromJson<T>(this SqlDataReader reader, int ordinal, JsonSerializerOptions? jsonOptions = default)
    {
        Guard.IsNotNull(reader);

        if (reader.IsDBNull(ordinal))
        {
            return default;
        }

        jsonOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        var json = reader.GetString(ordinal);

        return JsonSerializer.Deserialize<T>(json, jsonOptions);
    }
}
