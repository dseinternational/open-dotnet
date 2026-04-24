// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.TestValues;

namespace DSE.Open.Values;

public class ValueCollectionExtensionsTests
{
    [Fact]
    public void AveragePrimitives_WithEmptySpan_ShouldThrowInvalidOperationException()
    {
        static void Average()
        {
            _ = ValueCollectionExtensions.AveragePrimitives<Percentage, float>(ReadOnlySpan<Percentage>.Empty);
        }

        _ = Assert.Throws<InvalidOperationException>(Average);
    }

    [Fact]
    public void AveragePrimitives_WithValues_ShouldReturnAverage()
    {
        var values = new[] { (Percentage)10f, (Percentage)20f };

        var average = ValueCollectionExtensions.AveragePrimitives<Percentage, float>(values);

        Assert.Equal(15f, average);
    }
}
