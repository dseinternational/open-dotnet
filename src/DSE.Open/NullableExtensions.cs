// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open;

/// <summary>
/// Extensions for working with nullable value types.
/// </summary>
public static class NullableExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="nullable"/> is <see langword="null"/> or
    /// equals <c>default(T)</c>.
    /// </summary>
    public static bool IsNullOrDefault<T>([NotNullWhen(false)] this T? nullable)
        where T : struct, IEquatable<T>
    {
        return !nullable.HasValue || nullable.Value.Equals(default);
    }
}
