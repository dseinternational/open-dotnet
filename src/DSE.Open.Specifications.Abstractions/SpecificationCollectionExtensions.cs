// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public static class SpecificationCollectionExtensions
{
    /// <summary>
    /// Creates a specification that is satisfied if all of the specifications in the collection are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the candidate evaluated by the specifications.</typeparam>
    /// <param name="specifications">The specifications to include.</param>
    /// <returns>A specification that is satisfied if all of the specifications in the collection are satisfied.</returns>
    public static ISpecification<T> AsAllSatisfied<T>(
        this IEnumerable<ISpecification<T>> specifications)
    {
        return new AllSatisfiedSpecification<T>(specifications);
    }

    /// <summary>
    /// Creates a specification that is satisfied if any of the specifications in the collection are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the candidate evaluated by the specifications.</typeparam>
    /// <param name="specifications">The specifications to include.</param>
    /// <returns>A specification that is satisfied if any of the specifications in the collection are satisfied</returns>
    public static ISpecification<T> AsAnySatisfied<T>(
        this IEnumerable<ISpecification<T>> specifications)
    {
        return new AnySatisfiedSpecification<T>(specifications);
    }
}
