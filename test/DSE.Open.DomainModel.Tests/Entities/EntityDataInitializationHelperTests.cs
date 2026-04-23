// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EntityDataInitializationHelperTests
{
    [Fact]
    public void EnsureInitialized_NonNull_ReturnsValue()
    {
        var value = "hello";
        var result = EntityDataInitializationHelper.EnsureInitialized(value);
        Assert.Same(value, result);
    }

    [Fact]
    public void EnsureInitialized_Null_Throws()
    {
        string? value = null;
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            EntityDataInitializationHelper.EnsureInitialized(value));
    }

    [Fact]
    public void EnsureInitialized_Null_CapturesArgumentName()
    {
        string? myField = null;
        var ex = Assert.Throws<EntityDataInitializationException>(() =>
            EntityDataInitializationHelper.EnsureInitialized(myField));

        Assert.Equal(nameof(myField), ex.ParameterName);
    }

    [Fact]
    public void EnsureInitialized_ExplicitName_UsesSuppliedName()
    {
        string? value = null;
        var ex = Assert.Throws<EntityDataInitializationException>(() =>
            EntityDataInitializationHelper.EnsureInitialized(value, "CustomName"));

        Assert.Equal("CustomName", ex.ParameterName);
    }
}
