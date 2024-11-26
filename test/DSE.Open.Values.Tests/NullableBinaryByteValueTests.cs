// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.TestValues;

namespace DSE.Open.Values;

public class NullableBinaryByteValueTests : ValueTestsBase<NullableBinaryByteValue, byte>
{
    public override IEnumerable<NullableBinaryByteValue> ValidValues =>
    [
        NullableBinaryByteValue.False,
        NullableBinaryByteValue.True,
    ];
}
