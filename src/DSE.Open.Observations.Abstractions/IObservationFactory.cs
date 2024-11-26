// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
#pragma warning disable CA1000 // Do not declare static members on generic types

public interface IObservationFactory<TSelf, TValue>
    where TSelf : IObservation<TValue>, IObservationFactory<TSelf, TValue>
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    static TSelf Create(IMeasure<TValue> measure, TValue value)
    {
        return TSelf.Create(measure, value, TimeProvider.System);
    }

    static abstract TSelf Create(IMeasure<TValue> measure, TValue value, TimeProvider timeProvider);
}

public interface IObservationFactory<TSelf, TValue, TParam>
    where TSelf : IObservation<TValue, TParam>, IObservationFactory<TSelf, TValue, TParam>
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    static TSelf Create(IMeasure<TValue, TParam> measure, TParam parameter, TValue value)
    {
        return TSelf.Create(measure, parameter, value, TimeProvider.System);
    }

    static abstract TSelf Create(IMeasure<TValue, TParam> measure, TParam parameter, TValue value, TimeProvider timeProvider);
}
