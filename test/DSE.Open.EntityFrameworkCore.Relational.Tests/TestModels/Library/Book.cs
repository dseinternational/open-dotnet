// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;

public class Book : UpdateTimesTrackedEventRaisingEntity<int>
{
    public Book()
    {
    }

    [MaterializationConstructor]
    internal Book(
        int id,
        string title,
        string description,
        DateTimeOffset? created,
        DateTimeOffset? updated)
        : base(id, created, updated)
    {
        Title = title;
        Description = description;
    }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public ICollection<Author> Authors { get; } = new List<Author>();
}
