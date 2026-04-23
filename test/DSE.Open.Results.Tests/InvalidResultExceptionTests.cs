// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;

namespace DSE.Open.Results.Tests;

public class InvalidResultExceptionTests
{
    [Fact]
    public void Ctor_Generic_StoresResult()
    {
        var result = new Result { Status = ResultStatus.BadRequest };
        var ex = new InvalidResultException<Result>(result);
        Assert.Same(result, ex.Result);
    }

    [Fact]
    public void Ctor_Generic_UsesDefaultMessage_WhenMessageNull()
    {
        var result = new Result();
        var ex = new InvalidResultException<Result>(result);
        Assert.False(string.IsNullOrWhiteSpace(ex.Message));
    }

    [Fact]
    public void Ctor_Generic_UsesProvidedMessage()
    {
        var result = new Result();
        var ex = new InvalidResultException<Result>(result, "custom message");
        Assert.Equal("custom message", ex.Message);
    }

    [Fact]
    public void Ctor_Generic_CarriesInnerException()
    {
        var result = new Result();
        var inner = new InvalidOperationException("inner");
        var ex = new InvalidResultException<Result>(result, "outer", inner);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void Ctor_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new InvalidResultException<Result>(null!));
    }

    [Fact]
    public void ThrowIfAnyErrorNotifications_NoErrors_DoesNotThrow()
    {
        var result = new Result
        {
            Notifications =
            [
                Notification.Information("NTF100001", "info"),
                Notification.Warning("NTF100002", "warn"),
            ],
        };

        InvalidResultException.ThrowIfAnyErrorNotifications(result);
    }

    [Fact]
    public void ThrowIfAnyErrorNotifications_HasError_Throws()
    {
        var result = new Result
        {
            Notifications =
            [
                Notification.Information("NTF100001", "info"),
                Notification.Error("NTF100002", "error"),
            ],
        };

        var ex = Assert.Throws<InvalidResultException>(
            () => InvalidResultException.ThrowIfAnyErrorNotifications(result));
        Assert.Same(result, ex.Result);
    }

    [Fact]
    public void ThrowIfAnyErrorNotifications_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => InvalidResultException.ThrowIfAnyErrorNotifications(null!));
    }
}
