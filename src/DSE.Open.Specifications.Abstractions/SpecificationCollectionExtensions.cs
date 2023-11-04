// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public static class SpecificationCollectionExtensions
{
    /// <summary>
    /// Returns a specification that is satisfied if all of the specifications in the collection are satisfied.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specifications"></param>
    /// <returns></returns>
    public static ISpecification<T> AsAllSatisfied<T>(this IEnumerable<ISpecification<T>> specifications)
    {
        return new AllSatisfiedSpecification<T>(specifications);
    }

    /// <summary>
    /// Returns a specification that is satisfied if any of the specifications in the collection are satisfied.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specifications"></param>
    /// <returns></returns>
    public static ISpecification<T> AsAnySatisfied<T>(this IEnumerable<ISpecification<T>> specifications)
    {
        return new AnySatisfiedSpecification<T>(specifications);
    }
}
