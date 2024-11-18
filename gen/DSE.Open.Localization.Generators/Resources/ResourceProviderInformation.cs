// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Generators.Resources;

internal sealed record ResourceProviderInformation
{
    public string ProviderName { get; set; } = null!;

    public string ProviderNamespace { get; set; } = null!;

    public string ProviderAccessibility { get; set; } = null!;

    public string ResourcesName { get; set; } = null!;

    public string ResourcesPath { get; set; } = null!;
}
