// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

public class PackagedLocalizedResourceProviderTests
{
    private static readonly CultureInfo s_enGb = new("en-GB");
    private static readonly CultureInfo s_enUs = new("en-US");
    private static readonly CultureInfo s_frFr = new("fr-FR");

    private static TestResourceProvider CreateProvider()
    {
        var byCulture = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            [""] = new Dictionary<string, string>
            {
                ["Greeting"] = "Hello",
                ["Greeted"] = "Hello, {0}!",
                ["Totals"] = "{0} items for {1:C}",
            },
            ["en-US"] = new Dictionary<string, string>
            {
                ["Greeting"] = "Howdy",
            },
            ["fr-FR"] = new Dictionary<string, string>
            {
                ["Greeting"] = "Bonjour",
                ["Greeted"] = "Bonjour, {0} !",
            },
        };

        return new TestResourceProvider(new StubResourceManager(byCulture));
    }

    // ---------- Defaults ----------

    [Fact]
    public void LookupCulture_DefaultsToCurrentUICulture()
    {
        var provider = CreateProvider();
        Assert.Same(CultureInfo.CurrentUICulture, provider.LookupCulture);
    }

    [Fact]
    public void PresentationCulture_DefaultsToCurrentCulture()
    {
        var provider = CreateProvider();
        Assert.Same(CultureInfo.CurrentCulture, provider.PresentationCulture);
    }

    [Fact]
    public void LookupCulture_SetToNull_FallsBackToCurrentUICulture()
    {
        var provider = CreateProvider();
        provider.SetLookupCulture(s_enUs);
        provider.SetLookupCulture(null);

        Assert.Same(CultureInfo.CurrentUICulture, provider.LookupCulture);
    }

    [Fact]
    public void PresentationCulture_SetToNull_FallsBackToCurrentCulture()
    {
        var provider = CreateProvider();
        provider.SetPresentationCulture(s_enUs);
        provider.SetPresentationCulture(null);

        Assert.Same(CultureInfo.CurrentCulture, provider.PresentationCulture);
    }

    // ---------- GetString ----------

    [Fact]
    public void GetString_UsesExplicitCulture_WhenProvided()
    {
        var provider = CreateProvider();

        Assert.Equal("Howdy", provider.GetString("Greeting", s_enUs));
        Assert.Equal("Bonjour", provider.GetString("Greeting", s_frFr));
    }

    [Fact]
    public void GetString_FallsBackToInvariant_WhenCultureHasNoResource()
    {
        var provider = CreateProvider();

        Assert.Equal("Hello", provider.GetString("Greeting", s_enGb));
    }

    [Fact]
    public void GetString_UsesLookupCulture_WhenNoCulturePassed()
    {
        var provider = CreateProvider();
        provider.SetLookupCulture(s_enUs);

        Assert.Equal("Howdy", provider.GetString("Greeting"));
    }

    [Fact]
    public void GetString_MissingKey_ThrowsResourceNotFound()
    {
        var provider = CreateProvider();
        var ex = Assert.Throws<ResourceNotFoundException>(() => provider.GetString("DoesNotExist"));
        Assert.Contains("DoesNotExist", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void GetString_WhitespaceName_Throws(string name)
    {
        var provider = CreateProvider();
        _ = Assert.Throws<ArgumentException>(() => provider.GetString(name));
    }

    [Fact]
    public void GetString_NullName_Throws()
    {
        var provider = CreateProvider();
        _ = Assert.Throws<ArgumentNullException>(() => provider.GetString(null!));
    }

    // ---------- GetFormattedString ----------

    [Fact]
    public void GetFormattedString_SubstitutesSingleArgument()
    {
        var provider = CreateProvider();
        provider.SetLookupCulture(CultureInfo.InvariantCulture);

        Assert.Equal("Hello, World!", provider.GetFormattedString("Greeted", "World"));
    }

    [Fact]
    public void GetFormattedString_ExplicitCulture_UsedForLookupAndFormat()
    {
        var provider = CreateProvider();

        // fr-FR has "Bonjour, {0} !"
        Assert.Equal("Bonjour, Alice !", provider.GetFormattedString("Greeted", s_frFr, "Alice"));
    }

    [Fact]
    public void GetFormattedString_UsesPresentationCulture_ForFormatting_WhenCultureNull()
    {
        var provider = CreateProvider();
        provider.SetLookupCulture(CultureInfo.InvariantCulture);
        provider.SetPresentationCulture(s_enUs);

        // fr-FR currency (€) would differ; en-US uses $ with thousands separator.
        var result = provider.GetFormattedString("Totals", 1234, 99.95m);
        Assert.Contains("1234", result, StringComparison.Ordinal);
        Assert.Contains("$", result, StringComparison.Ordinal);
    }

    [Fact]
    public void GetFormattedString_MissingKey_ThrowsResourceNotFound()
    {
        var provider = CreateProvider();
        _ = Assert.Throws<ResourceNotFoundException>(() => provider.GetFormattedString("DoesNotExist"));
    }

    // ---------- GetStream ----------

    // Argument validation happens before the ResourceManager is touched, so it
    // can be exercised without a stub supplying a real UnmanagedMemoryStream.
    // The happy path for GetStream is covered indirectly by
    // DSE.Open.Localization.Generators.Tests.Functional, which wires a real
    // resource-backed ResourceManager.
    [Fact]
    public void GetStream_NullName_Throws()
    {
        var provider = CreateProvider();
        _ = Assert.Throws<ArgumentNullException>(() => provider.GetStream(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetStream_WhitespaceName_Throws(string name)
    {
        var provider = CreateProvider();
        _ = Assert.Throws<ArgumentException>(() => provider.GetStream(name));
    }
}
