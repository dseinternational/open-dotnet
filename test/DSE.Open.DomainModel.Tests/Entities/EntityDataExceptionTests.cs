// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EntityDataExceptionTests
{
    [Fact]
    public void Default_Ctor_UsesFrameworkMessage()
    {
        var ex = new EntityDataException();
        Assert.NotNull(ex.Message);
    }

    [Fact]
    public void Message_Ctor_StoresMessage()
    {
        var ex = new EntityDataException("boom");
        Assert.Equal("boom", ex.Message);
    }

    [Fact]
    public void MessageAndInnerException_Ctor_StoresBoth()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new EntityDataException("boom", inner);

        Assert.Equal("boom", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }
}
