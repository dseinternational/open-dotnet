// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

public class ResourceNotFoundExceptionTests
{
    [Fact]
    public void ThrownByProvider_IncludesKeyInMessage()
    {
        var provider = new TestResourceProvider(new StubResourceManager(
            new Dictionary<string, IReadOnlyDictionary<string, string>>()));

        var ex = Assert.Throws<ResourceNotFoundException>(() => provider.GetString("MissingResource.Key"));

        Assert.Contains("MissingResource.Key", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void IsSealed()
    {
        Assert.True(typeof(ResourceNotFoundException).IsSealed);
    }
}
