// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public abstract class ValueTestsBase<TValue, T>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>
{
    [Fact]
    public void TryFromValue_succeeds_for_valid_values()
    {
        foreach (var value in ValidValues)
        {
            Assert.True(TValue.TryFromValue((T)value, out _),
                $"Failed to convert '{value}' to a value of type {typeof(TValue).Name}");
        }
    }

    [Fact]
    public void Equal_valid_values_are_equal()
    {
        foreach (var val in ValidValues)
        {
            Assert.Equal(val, val);
            Assert.True(val.Equals(val));
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(val == val);
#pragma warning restore CS1718 // Comparison made to same variable

            var val2 = (TValue)(T)val;

            Assert.Equal(val, val2);
            Assert.True(val.Equals(val2));
            Assert.True(val == val2);
        }
    }

    public abstract IEnumerable<TValue> ValidValues { get; }
}
