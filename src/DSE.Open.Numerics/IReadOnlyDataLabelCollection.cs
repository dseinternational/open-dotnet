// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyDataLabelCollection<TData, TLabel> : IReadOnlyCollection<DataLabel<TData, TLabel>>
    where TData : IEquatable<TData>
    where TLabel : IEquatable<TLabel>
{

}
