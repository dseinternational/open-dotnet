// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;

namespace DSE.Open.Specifications;

/// <summary>
/// Adds annotations to specification results.
/// </summary>
public class SpecificationResultAnnotator
{
    private readonly Lazy<ConcurrentDictionary<string, object>> _properties =
        new(() => new ConcurrentDictionary<string, object>());

    /// <summary>
    /// Data that can be specified by specifications providing results.
    /// </summary>
    public ConcurrentDictionary<string, object> Properties => _properties.Value;
}

