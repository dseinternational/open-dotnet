// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;

namespace DSE.Open.Tests.Security;

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
}
