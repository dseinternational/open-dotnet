// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;

public class Author : UpdateTimesTrackedEventRaisingEntity<int>
{
    public Author()
    {
    }

    [MaterializationConstructor]
    internal Author(
        int id,
        string givenName,
        string familyName,
        DateTimeOffset? created,
        DateTimeOffset? updated)
        : base(id, created, updated)
    {
        GivenName = givenName;
        FamilyName = familyName;
    }

    public required string GivenName { get; set; }

    public required string FamilyName { get; set; }

    public ICollection<Book> Books { get; } = [];
}
