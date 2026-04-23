// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Marks a partial class as a localized resource provider to be completed by the
/// <c>DSE.Open.Localization.Generators</c> source generator. The generator emits a
/// <see cref="PackagedLocalizedResourceProvider"/> subclass with a strongly-typed accessor
/// for every key declared in the designated resource type.
/// </summary>
/// <param name="resource">
/// The resource type (typically the empty designer class sitting alongside a
/// <c>.resx</c> or <c>.restext</c> file) whose keys the provider exposes.
/// </param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceProviderAttribute(Type resource) : Attribute
{
    /// <summary>The resource type whose keys this provider exposes.</summary>
    public Type Resource { get; } = resource;
}
