// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

#pragma warning disable CA1034 // Nested types should not be visible

public static class NullableExtensions
{
    extension<T>(T? nullable)
        where T : struct
    {
        public bool IsUnknown => !nullable.HasValue;
    }
}
