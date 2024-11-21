// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceProviderAttribute(Type resource) : Attribute
{
    public Type Resource { get; } = resource;
}
