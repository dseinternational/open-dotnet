// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Security;

public class RandomValueGeneratorTests
{
    private readonly ITestOutputHelper _output;

    public RandomValueGeneratorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GetStringId_generates_id_with_default_length()
    {
        for (var i = 0; i < 50; i++)
        {
            var id = RandomValueGenerator.GetStringValue();
            Assert.NotNull(id);
            _output.WriteLine(id);
            Assert.Equal(16, id.Length);
        }
    }

    [Fact]
    public void GetInt32Value_min_max()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomValueGenerator.GetInt32Value(100, 999);
            //_output.WriteLine(value.ToString());
            Assert.True(value >= 100);
            Assert.True(value <= 999);
        }
    }

    [Fact]
    public void GetInt32Value_min_max_6()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomValueGenerator.GetInt32Value(100000, 999999);
            //_output.WriteLine(value.ToString());
            Assert.True(value >= 100000);
            Assert.True(value <= 999999);
        }
    }

    [Fact]
    public void GetInt64Value()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomValueGenerator.GetInt64Value(10000000000001, 99999999999999);
            _output.WriteLine(value.ToStringInvariant());
            Assert.True(value >= 10000000000001);
            Assert.True(value <= 99999999999999);
        }
    }

    [Fact]
    public void GetUInt64Value()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomValueGenerator.GetUInt64Value(100000000001, 999999999999);
            _output.WriteLine(value.ToStringInvariant());
            Assert.True(value >= 100000000001);
            Assert.True(value <= 999999999999);
        }
    }

    [Fact]
    public void GetJsonSafeUInt64()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomValueGenerator.GetJsonSafeUInt64();
            _output.WriteLine(value.ToStringInvariant());
            Assert.True(value >= 0);
            Assert.True(value <= NumberHelper.MaxJsonSafeInteger);
        }
    }
}
