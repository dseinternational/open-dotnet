// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;

namespace DSE.Open.Data.SqlClient;

/// <summary>
/// Provides extension methods for converting between <see cref="Timestamp"/> and <see cref="SqlBinary"/>.
/// </summary>
public static class TimestampExtensions
{
    /// <summary>
    /// Returns a <see cref="SqlBinary"/> containing the bytes of the specified <see cref="Timestamp"/>.
    /// </summary>
    public static SqlBinary ToSqlBinary(this Timestamp timestamp)
    {
        return new(timestamp.GetBytes());
    }

    /// <summary>
    /// Returns a <see cref="Timestamp"/> created from the bytes of the specified <see cref="SqlBinary"/>,
    /// or <see langword="null"/> if the value is <see cref="SqlBinary.IsNull"/>.
    /// </summary>
    public static Timestamp? ToTimestamp(this SqlBinary value)
    {
        return value.IsNull ? null : new Timestamp(value.Value);
    }
}
