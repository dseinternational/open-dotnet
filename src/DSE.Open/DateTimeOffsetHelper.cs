// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class DateTimeOffsetHelper
{
    public static DateTimeOffset ParseIso8601(string value) => DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
}
