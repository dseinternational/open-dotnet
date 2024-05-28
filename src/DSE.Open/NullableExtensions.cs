// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

public static class NullableExtensions
{
    public static bool IsNullOrDefault<T>([NotNullWhen(true)] this T? nullable)
        where T : struct, IEquatable<T>
    {
        return !nullable.HasValue || nullable.Value.Equals(default);
    }
}
