// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

public class ResourceProviderAttributeTests
{
    [Fact]
    public void Ctor_CapturesResourceType()
    {
        var attr = new ResourceProviderAttribute(typeof(string));
        Assert.Equal(typeof(string), attr.Resource);
    }

    [Fact]
    public void AttributeUsage_TargetsClassesOnly()
    {
        var usage = typeof(ResourceProviderAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.Equal(AttributeTargets.Class, usage.ValidOn);
    }
}
