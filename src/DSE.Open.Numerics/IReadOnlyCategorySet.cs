// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyCategorySet
{
    bool IsEmpty { get; }
}

public interface IReadOnlyCategorySet<T> : IReadOnlyCategorySet, IReadOnlySet<T>
    where T : IEquatable<T>
{

}
