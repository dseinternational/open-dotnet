// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.DomainModel.Entities;

public static class EntityDataInitializationHelper
{
    /// <summary>
    /// Verifies than value is not <see langword="null"/>  and, if so, returns it. Otherwise throws <see cref="EntityDataInitializationException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <param name="value">The value to test.</param>
    /// <param name="valueName">The name of the value.</param>
    /// <returns>The value, if it is not <see langword="null"/>.</returns>
    public static T EnsureInitialized<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? valueName = null)
        where T : class
    {
        EntityDataInitializationException.ThrowIf(value is null, valueName);
        return value;
    }
}
