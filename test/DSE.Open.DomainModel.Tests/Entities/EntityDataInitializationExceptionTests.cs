// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EntityDataInitializationExceptionTests
{
    [Fact]
    public void Ctor_PositionalValidationResult_ResolvesUnambiguously()
    {
        // Regression for issue #320: previously this call was ambiguous between
        // the (string, ValidationResult?, string?) and
        // (string, ValidationResult?=null, string?=null, Exception?=null) ctors.
        var vr = new ValidationResult("invalid");

        var ex = new EntityDataInitializationException("foo", vr);

        Assert.Equal("foo", ex.ParameterName);
        Assert.Same(vr, ex.ValidationResult);
    }

    [Fact]
    public void Ctor_PositionalValidationResultWithMessage_ResolvesUnambiguously()
    {
        var vr = new ValidationResult("invalid");

        var ex = new EntityDataInitializationException("foo", vr, "custom message");

        Assert.Equal("foo", ex.ParameterName);
        Assert.Same(vr, ex.ValidationResult);
        Assert.Equal("custom message", ex.Message);
    }

    [Fact]
    public void Ctor_PositionalStringMessage_UsesStringMessageOverload()
    {
        var ex = new EntityDataInitializationException("foo", "custom message");

        Assert.Equal("foo", ex.ParameterName);
        Assert.Null(ex.ValidationResult);
        Assert.Equal("custom message", ex.Message);
    }

    [Fact]
    public void Ctor_SingleArgument_UsesDefaultMessage()
    {
        var ex = new EntityDataInitializationException("foo");

        Assert.Equal("foo", ex.ParameterName);
        Assert.Null(ex.ValidationResult);
        Assert.Contains("foo", ex.Message, StringComparison.Ordinal);
    }
}
