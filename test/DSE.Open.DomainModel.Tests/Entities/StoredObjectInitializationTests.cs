// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class StoredObjectInitializationTests
{
    [Fact]
    public void Created_HasDefaultValueZero()
    {
        // Enum default must remain Created so newly-constructed entities that
        // haven't called the materialization constructor report Created.
        Assert.Equal(StoredObjectInitialization.Created, default(StoredObjectInitialization));
        Assert.Equal(0, (int)StoredObjectInitialization.Created);
    }

    [Fact]
    public void Materialized_HasValueOne()
    {
        Assert.Equal(1, (int)StoredObjectInitialization.Materialized);
    }

    [Fact]
    public void Enum_HasExactlyTwoDefinedMembers()
    {
        var names = Enum.GetNames<StoredObjectInitialization>();
        Assert.Equal(2, names.Length);
    }
}
