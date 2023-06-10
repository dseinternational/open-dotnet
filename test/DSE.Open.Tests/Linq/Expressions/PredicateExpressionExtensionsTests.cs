// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Linq.Expressions;

namespace DSE.Open.Tests.Linq.Expressions;

public class PredicateExpressionExtensionsTests
{
    [Fact]
    public void AndAlso()
    {
        var collection = Enumerable.Range(0, 50).ToList().AsQueryable();

        var range = UnboundedRange.LessThanOrEqual(20);
        var range2 = UnboundedRange.GreaterThanOrEqual(10);

        var predicate = Predicates.CreateRange(range)!;
        predicate = predicate.AndAlso(Predicates.CreateRange(range2)!);

        var filtered = collection.Where(predicate);

        filtered.ForEach(e => Assert.True(e is <= 20 and >= 10));
    }

    [Fact]
    public void Or()
    {
        var collection = Enumerable.Range(0, 50).ToList().AsQueryable();

        var range = UnboundedRange.LessThanOrEqual(10);
        var range2 = UnboundedRange.GreaterThanOrEqual(40);

        var predicate = Predicates.CreateRange(range)!;
        predicate = predicate.AndAlso(Predicates.CreateRange(range2)!);

        var filtered = collection.Where(predicate);

        filtered.ForEach(e => Assert.True(e is <= 10 or >= 40));
    }
}

