// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

internal static class NullableExtensions
{
    public static bool IsUnknown<T>([MaybeNullWhen(true)] this T? nullable)
        where T : struct
    {
        return !nullable.HasValue;
    }
}
