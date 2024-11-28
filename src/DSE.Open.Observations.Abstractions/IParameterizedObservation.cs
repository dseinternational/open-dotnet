// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Identifies an observation that has a single parameter.
/// </summary>
/// <typeparam name="TParam"></typeparam>
public interface IParameterizedObservation<TParam> : IObservation
    where TParam : struct, IEquatable<TParam>
{
    new TParam Parameter { get; }
}
