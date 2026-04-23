// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results.Tests;

#pragma warning disable CS0618 // Type or member is obsolete

/// <summary>
/// Covers the obsolete <see cref="ValueResultExtensions.GetRequiredValue{T}"/>
/// helper until it is removed. Users should migrate to
/// <see cref="ValueResult{T}.RequiredValue"/>.
/// </summary>
public class ValueResultExtensionsTests
{
    [Fact]
    public void GetRequiredValue_HasValue_ReturnsValue()
    {
        var result = ValueResult.Create<string>("hello");
        Assert.Equal("hello", result.GetRequiredValue());
    }

    [Fact]
    public void GetRequiredValue_NoValue_Throws()
    {
        var result = ValueResult.Create<string>(null);
        _ = Assert.Throws<InvalidOperationException>(() => result.GetRequiredValue());
    }

    [Fact]
    public void GetRequiredValue_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ValueResultExtensions.GetRequiredValue<string>(null!));
    }
}
