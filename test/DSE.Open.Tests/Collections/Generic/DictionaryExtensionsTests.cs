// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public class DictionaryExtensionsTests
{
    [Fact]
    public void AddOrSet()
    {
        var dict = new Dictionary<string, string>();
        dict.AddOrSet("key", "value");
        Assert.True(dict.Count == 1);
        Assert.True(dict.ContainsKey("key"));
        Assert.True(dict.ContainsValue("value"));
    }

    [Fact]
    public void AddOrSetAll()
    {
        var dict = new Dictionary<string, string>();
        dict.AddOrSet("key", "value");
        dict.AddOrSet("key2", "value2");
        var dict2 = new Dictionary<string, string>();
        dict2.AddOrSetRange(dict);
        Assert.True(dict2.Count == 2);
        Assert.True(dict2.ContainsKey("key"));
        Assert.True(dict2.ContainsValue("value"));
        Assert.True(dict2.ContainsKey("key2"));
        Assert.True(dict2.ContainsValue("value2"));
    }
}
