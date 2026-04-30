// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Helpers for working with <see cref="DateTimeOffset"/> values.
/// </summary>
public static class DateTimeOffsetHelper
{
    /// <summary>
    /// Parses an ISO 8601 round-trip ("o") formatted string into a <see cref="DateTimeOffset"/>.
    /// </summary>
    public static DateTimeOffset ParseIso8601(string value)
    {
        return DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
    }
}
