﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IReadOnlyNumericVector<T> : IReadOnlyVector<T>
    where T : struct, INumber<T>
{
}

