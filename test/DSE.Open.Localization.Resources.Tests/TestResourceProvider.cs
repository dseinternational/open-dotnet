// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Concrete <see cref="PackagedLocalizedResourceProvider"/> backed by a
/// <see cref="StubResourceManager"/>. Provides hooks for tests to set
/// <see cref="LookupCulture"/> and <see cref="PresentationCulture"/>, which have
/// <c>protected</c> setters on the base type.
/// </summary>
internal sealed class TestResourceProvider : PackagedLocalizedResourceProvider
{
    private readonly ResourceManager _resourceManager;

    public TestResourceProvider(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    protected override ResourceManager ResourceManager => _resourceManager;

    public void SetLookupCulture(CultureInfo? value) => LookupCulture = value!;

    public void SetPresentationCulture(CultureInfo? value) => PresentationCulture = value!;
}
