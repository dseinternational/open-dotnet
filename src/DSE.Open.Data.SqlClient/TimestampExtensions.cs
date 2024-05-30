// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;

namespace DSE.Open.Data.SqlClient;

public static class TimestampExtensions
{
    public static SqlBinary ToSqlBinary(this Timestamp timestamp)
    {
        return new(timestamp.GetBytes());
    }

    public static Timestamp? ToTimestamp(this SqlBinary value)
    {
        return value.IsNull ? null : new Timestamp(value.Value);
    }
}
