// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore.Tests;

public class StorageModelDbContextFake : DbContext
{
    public StorageModelDbContextFake(DbContextOptions options) : base(options)
    {
    }
}
