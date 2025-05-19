// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public interface ITriEquatable<T>
{
    Trilean Equals(T other);

    bool EqualAndNeitherUnknown(T other);

    bool EqualOrBothUnknown(T other);

    bool EqualOrEitherUnknown(T other);
}
