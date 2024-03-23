// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Requests.Tests;

public class UpdateTests
{
    [Fact]
    public void NewUpdate_WithClass_ShouldCreate()
    {
        // Arrange
        const string value = "value";

        // Act
        var update = Update.NewUpdate(value);

        // Assert
        Assert.Equal(UpdateMode.Update, update.Mode);
        Assert.Equal(value, update.GetValue());
    }

    [Fact]
    public void NewUpdate_WithStruct_ShouldCreate()
    {
        // Arrange
        const int value = 42;

        // Act
        var update = Update.NewUpdate(value);

        // Assert
        Assert.Equal(UpdateMode.Update, update.Mode);
        Assert.Equal(value, update.GetValue());
    }

    [Fact]
    public void NewUpdate_WithNullStruct_ShouldCreate()
    {
        // Arrange
        int? value = null;

        // Act
        var update = Update.NewUpdate(value);

        // Assert
        Assert.Equal(UpdateMode.Update, update.Mode);
        Assert.Equal(value, update.GetValue());
    }

    [Fact]
    public void New_WithNonDefaultValueAndModeNone_ShouldThrowArgumentException()
    {
        // Arrange
        const int value = 42;

        // Act
        void Act() => _ = Update.New(value, UpdateMode.None);

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void GetValue_WithNoChange_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var update = Update.NoChange<int>();

        // Act
        void Act() => update.GetValue();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void GetValue_WithChange_ShouldReturnValue()
    {
        // Arrange
        const int innerValue = 42;
        var update = Update.NewUpdate(innerValue);

        // Act
        var value = update.GetValue();

        // Assert
        Assert.Equal(innerValue, value);
    }

    [Fact]
    public void TryGetValue_WithValue_ShouldReturnValueAndTrue()
    {
        // Arrange
        const int innerValue = 42;
        var update = Update.NewUpdate(innerValue);

        // Act
        var result = update.TryGetValue(out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(innerValue, value);
    }

    [Fact]
    public void TryGetValue_WithNoValue_ShouldReturnFalseAndDefault()
    {
        // Arrange
        var update = Update.NoChange<int>();

        // Act
        var result = update.TryGetValue(out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default, value);
    }
}
