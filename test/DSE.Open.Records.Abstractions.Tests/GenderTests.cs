// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Records.Abstractions.Tests;

public class GenderTests
{
    [Theory]
    [MemberData(nameof(Values))]
    public void SerializeDeserialize(Gender value)
    {
        AssertJson.Roundtrip(value);
    }

    public static TheoryData<Gender> Values { get; } = new()
    {
        Gender.Female,
        Gender.Male,
        Gender.Other,
    };
}
