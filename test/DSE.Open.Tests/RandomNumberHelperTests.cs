// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class RandomNumberHelperTests
{
    [Fact]
    public void GetJsonSafeInteger()
    {
        var values = Enumerable.Range(0, 10000)
            .Select(_ => RandomNumberHelper.GetJsonSafeInteger()).ToArray();

        Assert.All(values, v => Assert.InRange(v, (ulong)0, RandomNumberHelper.MaxJsonSafeInteger));

        var uniqueValues = new HashSet<ulong>(values);

        Assert.Equal(values.Length, uniqueValues.Count);
    }
}
