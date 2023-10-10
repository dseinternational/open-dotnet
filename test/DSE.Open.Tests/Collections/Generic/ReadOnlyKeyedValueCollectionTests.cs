// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Text.Json;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public class ReadOnlyKeyedValueCollectionTests
{
    [Fact]
    public void Equal_returns_true_identical_value_collections()
    {
        var c1 = new TestReadOnlyKeyedValueCollection(Enumerable.Range(0, 20));
        var c2 = new TestReadOnlyKeyedValueCollection(Enumerable.Range(0, 20));

        Assert.Equal(c1, c2);
    }

    [Fact]
    public void Equal_returns_false_nonidentical_value_collections()
    {
        var c1 = new TestReadOnlyKeyedValueCollection(Enumerable.Range(0, 20));
        var c2 = new TestReadOnlyKeyedValueCollection(Enumerable.Range(2, 20));

        Assert.NotEqual(c1, c2);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var c1 = new TestReadOnlyKeyedValueCollection(Enumerable.Range(0, 20));
        var json = JsonSerializer.Serialize(c1);
        var c2 = JsonSerializer.Deserialize<TestReadOnlyKeyedValueCollection>(json);
        Assert.Equal(c1, c2);
    }

}

public sealed class TestReadOnlyKeyedValueCollection : ReadOnlyKeyedValueCollection<string, int>
{
    public TestReadOnlyKeyedValueCollection()
    {
    }

    public TestReadOnlyKeyedValueCollection(IEnumerable<int> list) : base(list)
    {
    }

    protected override string GetKeyForItem(int item) => item.ToString(CultureInfo.InvariantCulture);
}
