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

/// <summary>
/// Identifies an observation that has two parameters.
/// </summary>
/// <typeparam name="TParam"></typeparam>
/// <typeparam name="TParam2"></typeparam>
public interface IParameterizedObservation<TParam, TParam2> : IObservation
    where TParam : struct, IEquatable<TParam>
    where TParam2 : struct, IEquatable<TParam2>
{
    new TParam Parameter { get; }

    new TParam Parameter2 { get; }
}
