// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public enum MeasurementParameterType
{
    /// <summary>
    /// Provides an integer to identify the parameter. The value must be greater than or equal to
    /// <see cref="IObservationParameter.MinIntegerId"/> and less than or equal to
    /// <see cref="IObservationParameter.MaxIntegerId"/>.
    /// </summary>
    Integer,

    /// <summary>
    /// Provides a text value to identify the parameter. The value must not be empty and must be no
    /// longer than <see cref="IObservationParameter.MaxTextLength"/> UTF-16 characters.
    /// </summary>
    Text
}
