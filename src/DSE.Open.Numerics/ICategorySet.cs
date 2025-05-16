// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface ICategorySet : IReadOnlyCategorySet
{
}

public interface ICategorySet<T>
    : ICategorySet,
      IReadOnlyCategorySet<T>,
      ISet<T>
    where T : IEquatable<T>
{

}
