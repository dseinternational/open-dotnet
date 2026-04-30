// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.Data.SqlClient;

namespace DSE.Open.Data.SqlClient;

/// <summary>
/// Provides extension methods for reading values from a <see cref="SqlDataReader"/>.
/// </summary>
public static class SqlDataReaderExtensions
{
    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="bool"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static bool? GetNullableBoolean(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="byte"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static byte? GetNullableByte(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetByte(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="DateTime"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static DateTime? GetNullableDateTime(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="DateTimeOffset"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static DateTimeOffset? GetNullableDateTimeOffset(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTimeOffset(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="decimal"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static decimal? GetNullableDecimal(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="double"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static double? GetNullableDouble(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetDouble(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="float"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static float? GetNullableFloat(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetFloat(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="short"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static short? GetNullableInt16(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt16(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="int"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static int? GetNullableInt32(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="long"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static long? GetNullableInt64(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt64(ordinal);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="string"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static string? GetNullableString(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    /// <summary>
    /// Reads the binary value at the specified column ordinal as a <see cref="Utf8String"/>.
    /// </summary>
    /// <exception cref="SqlNullValueException">The column value is <c>DBNULL</c>.</exception>
    public static Utf8String GetUtf8String(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var data = reader.GetSqlBytes(ordinal);

        if (data.IsNull)
        {
            SqlExceptionHelper.ThrowSqlNullValueException();
        }

        return data.Storage == StorageState.Buffer
            ? new(data.Buffer.AsMemory()[..(int)data.Length])
            : new Utf8String(data.Value);
    }

    /// <summary>
    /// Reads the binary value at the specified column ordinal as a nullable <see cref="Utf8String"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static Utf8String? GetNullableUtf8String(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var data = reader.GetSqlBytes(ordinal);

        if (data.IsNull)
        {
            return null;
        }

        return data.Storage == StorageState.Buffer
            ? new(data.Buffer.AsMemory()[..(int)data.Length])
            : new Utf8String(data.Value);
    }

    /// <summary>
    /// Returns the value at the specified column ordinal as a nullable <see cref="Guid"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static Guid? GetNullableGuid(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return reader.IsDBNull(ordinal) ? null : reader.GetGuid(ordinal);
    }

    /// <summary>
    /// Returns the string value at the specified column ordinal as a <see cref="Uri"/>.
    /// </summary>
    public static Uri GetUri(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return new(reader.GetString(ordinal));
    }

    /// <summary>
    /// Returns the string value at the specified column ordinal as a nullable <see cref="Uri"/>,
    /// returning <see langword="null"/> when the column is <c>DBNULL</c>.
    /// </summary>
    public static Uri? GetNullableUri(this SqlDataReader reader, int ordinal)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.IsDBNull(ordinal) ? null : new Uri(reader.GetString(ordinal));
    }

    /// <summary>
    /// Reads the string value at the specified column ordinal and parses it as <typeparamref name="T"/>
    /// using the specified format provider, defaulting to <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static T GetParseableValue<T>(this SqlDataReader reader, int ordinal, IFormatProvider? provider = null)
        where T : IParsable<T>
    {
        ArgumentNullException.ThrowIfNull(reader);

        provider ??= CultureInfo.InvariantCulture;

        var value = reader.GetString(ordinal);

        return T.Parse(value, provider);
    }

    /// <summary>
    /// Reads the string value at the specified column ordinal and deserializes it as JSON to
    /// <typeparamref name="T"/>, returning <see langword="default"/> when the column is <c>DBNULL</c>.
    /// </summary>
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    public static T? GetFromJson<T>(this SqlDataReader reader, int ordinal, JsonSerializerOptions? jsonOptions = default)
    {
        ArgumentNullException.ThrowIfNull(reader);

        if (reader.IsDBNull(ordinal))
        {
            return default;
        }

        jsonOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        var json = reader.GetString(ordinal);

        return JsonSerializer.Deserialize<T>(json, jsonOptions);
    }
}
