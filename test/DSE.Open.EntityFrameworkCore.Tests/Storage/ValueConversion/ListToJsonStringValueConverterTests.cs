// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class ListToJsonStringValueConverterTests
{
    [Fact]
    public void SerializesIntArray()
    {
        int[] value = [1, 2, 3, 4, 5, 6, 7, 8];
        var converter = new ListToJsonStringValueConverter<int>();
        var serialized = converter.ConvertToProviderExpression.Compile().Invoke(value);
        Assert.Equal("[1,2,3,4,5,6,7,8]", serialized);
    }
    [Fact]
    public void SerializeDeserializeAreEqual()
    {
        int[] value = [1, 2, 3, 4, 5, 6, 7, 8];
        var converter = new ListToJsonStringValueConverter<int>();
        var serialized = converter.ConvertToProviderExpression.Compile().Invoke(value);
        var deserialized = converter.ConvertFromProviderExpression.Compile().Invoke(serialized);
        Assert.Equal(value, deserialized);
    }
}
