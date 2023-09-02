// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;

namespace DSE.Open.Labels.Tests;

public sealed class TestResourceLabelDescriptionProvider : ResourceLabelDescriptionProvider<MyValues>
{
    private static readonly Lazy<ResourceManager> s_resourceManager = new(()
        => new ResourceManager($"{typeof(TestResourceLabelDescriptionProvider).Namespace}.Resources.Test", typeof(TestResourceLabelDescriptionProvider).Assembly));

    public static readonly TestResourceLabelDescriptionProvider Default = new();

    private TestResourceLabelDescriptionProvider()
    {
    }

    public override ResourceManager ResourceManager => s_resourceManager.Value;

    public override string GetLabelKey(MyValues value)
    {
        return value.ToString();
    }

    public override string GetDescriptionKey(MyValues value)
    {
        return "{value}_description";
    }
}

public enum MyValues
{
    Value1,
    Value2,
    Value3
}
