// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public static class AsyncSpecificationExtensions
{
    /// <summary>
    /// Returns a specification that is satisfied if a candidate satisfies both specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IAsyncSpecification<T> And<T>(this IAsyncSpecification<T> left, IAsyncSpecification<T> right)
    {
        return new AndAsyncSpecification<T>(left, right);
    }

    /// <summary>
    /// Returns a specification that is satisfied if a candidate satisfies either specification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IAsyncSpecification<T> Or<T>(this IAsyncSpecification<T> left, IAsyncSpecification<T> right)
    {
        return new OrAsyncSpecification<T>(left, right);
    }
}
