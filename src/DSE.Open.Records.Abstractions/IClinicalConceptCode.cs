// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Records;

public interface IClinicalConceptCode<TSelf> : IEquatableValue<TSelf, long>
    where TSelf : struct, IClinicalConceptCode<TSelf>;
