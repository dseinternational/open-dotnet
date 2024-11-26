// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IMeasure
{
    /// <summary>
    /// An identifier for the measure. This is generated from a predictable hash of the <see cref="Uri" />.
    /// </summary>
    MeasureId Id { get; }

    /// <summary>
    /// A URI that uniquely identifies the measure.
    /// </summary>
    Uri Uri { get; }

    MeasurementLevel MeasurementLevel { get; }

    string Name { get; }

    string Statement { get; }
}

public interface IMeasure<TValue> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    TObs CreateObservation<TObs>(TValue value, TimeProvider timeProvider)
        where TObs : IObservation<TValue>, IObservationFactory<TObs, TValue>;
}

public interface IMeasure<TValue, TParam> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    TObs CreateObservation<TObs>(TParam parameter, TValue value, TimeProvider timeProvider)
        where TObs : IObservation<TValue, TParam>, IObservationFactory<TObs, TValue, TParam>;
}
