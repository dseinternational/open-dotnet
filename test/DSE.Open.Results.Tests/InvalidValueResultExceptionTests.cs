// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;

namespace DSE.Open.Results.Tests;

public class InvalidValueResultExceptionTests
{
    [Fact]
    public void Ctor_StoresResult()
    {
        var result = ValueResult.Create<string>("hello");
        var ex = new InvalidValueResultException<string>(result);
        Assert.Same(result, ex.Result);
    }

    [Fact]
    public void Ctor_UsesProvidedMessage()
    {
        var result = ValueResult.Create<string>(null);
        var ex = new InvalidValueResultException<string>(result, "custom");
        Assert.Equal("custom", ex.Message);
    }

    [Fact]
    public void Ctor_CarriesInnerException()
    {
        var result = ValueResult.Create<string>(null);
        var inner = new InvalidOperationException("inner");
        var ex = new InvalidValueResultException<string>(result, "outer", inner);
        Assert.Same(inner, ex.InnerException);
    }

    // ---------- ThrowIfNotHasValue ----------

    [Fact]
    public void ThrowIfNotHasValue_HasValue_DoesNotThrow()
    {
        var result = ValueResult.Create<string>("value");
        InvalidValueResultException.ThrowIfNotHasValue(result);
    }

    [Fact]
    public void ThrowIfNotHasValue_NullValue_Throws()
    {
        var result = ValueResult.Create<string>(null);
        var ex = Assert.Throws<InvalidValueResultException<string>>(
            () => InvalidValueResultException.ThrowIfNotHasValue(result));
        Assert.Same(result, ex.Result);
    }

    [Fact]
    public void ThrowIfNotHasValue_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => InvalidValueResultException.ThrowIfNotHasValue<string>(null!));
    }

    // ---------- ThrowIfAnyErrorNotifications ----------

    [Fact]
    public void ThrowIfAnyErrorNotifications_NoErrors_DoesNotThrow()
    {
        var result = ValueResult.Create(
            "value",
            [Notification.Warning("NTF100001", "warn")]);
        InvalidValueResultException.ThrowIfAnyErrorNotifications(result);
    }

    [Fact]
    public void ThrowIfAnyErrorNotifications_HasError_Throws()
    {
        var result = ValueResult.Create(
            "value",
            [Notification.Error("NTF100002", "error")]);
        _ = Assert.Throws<InvalidValueResultException<string>>(
            () => InvalidValueResultException.ThrowIfAnyErrorNotifications(result));
    }

    [Fact]
    public void ThrowIfAnyErrorNotifications_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => InvalidValueResultException.ThrowIfAnyErrorNotifications<string>(null!));
    }

    // ---------- ThrowIfNotHasValueOrAnyErrorNotifications ----------

    [Fact]
    public void ThrowIfNotHasValueOrAnyErrorNotifications_ValueAndNoErrors_DoesNotThrow()
    {
        var result = ValueResult.Create(
            "value",
            [Notification.Information("NTF100001", "info")]);
        InvalidValueResultException.ThrowIfNotHasValueOrAnyErrorNotifications(result);
    }

    [Fact]
    public void ThrowIfNotHasValueOrAnyErrorNotifications_NoValue_Throws()
    {
        var result = ValueResult.Create<string>(null);
        _ = Assert.Throws<InvalidValueResultException<string>>(
            () => InvalidValueResultException.ThrowIfNotHasValueOrAnyErrorNotifications(result));
    }

    [Fact]
    public void ThrowIfNotHasValueOrAnyErrorNotifications_HasErrorEvenWithValue_Throws()
    {
        var result = ValueResult.Create(
            "value",
            [Notification.Error("NTF100002", "error")]);
        _ = Assert.Throws<InvalidValueResultException<string>>(
            () => InvalidValueResultException.ThrowIfNotHasValueOrAnyErrorNotifications(result));
    }

    [Fact]
    public void ThrowIfNotHasValueOrAnyErrorNotifications_NullResult_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => InvalidValueResultException.ThrowIfNotHasValueOrAnyErrorNotifications<string>(null!));
    }
}
