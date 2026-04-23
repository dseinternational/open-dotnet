// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Thrown by <see cref="ILocalizedResourceProvider"/> implementations when a
/// lookup for a resource key returns no value for the requested culture (or any
/// of its fallbacks).
/// </summary>
/// <remarks>
/// Carries the missing key in its <see cref="Exception.Message"/>. Cannot be
/// constructed by callers; use the provider methods, which throw this exception
/// on miss.
/// </remarks>
public sealed class ResourceNotFoundException : Exception
{
    private ResourceNotFoundException(string message) : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string key)
    {
        throw new ResourceNotFoundException($"Resource with key '{key}' not found.");
    }
}
