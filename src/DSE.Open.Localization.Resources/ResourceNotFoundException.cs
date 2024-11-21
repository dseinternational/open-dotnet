// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Exception thrown when a resource string is not found.
/// </summary>
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
