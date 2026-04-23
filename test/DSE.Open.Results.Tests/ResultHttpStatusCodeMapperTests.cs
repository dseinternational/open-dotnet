// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Net;

namespace DSE.Open.Results.Tests;

public class ResultHttpStatusCodeMapperTests
{
    [Theory]
    [InlineData(ResultStatus.Unspecified, HttpStatusCode.OK)]
    [InlineData(ResultStatus.Found, HttpStatusCode.OK)]
    [InlineData(ResultStatus.Created, HttpStatusCode.Created)]
    [InlineData(ResultStatus.Updated, HttpStatusCode.OK)]
    [InlineData(ResultStatus.Deleted, HttpStatusCode.NoContent)]
    [InlineData(ResultStatus.Accepted, HttpStatusCode.Accepted)]
    [InlineData(ResultStatus.BadRequest, HttpStatusCode.BadRequest)]
    [InlineData(ResultStatus.RuleViolation, HttpStatusCode.UnprocessableEntity)]
    [InlineData(ResultStatus.StateConflict, HttpStatusCode.Conflict)]
    [InlineData(ResultStatus.NotFound, HttpStatusCode.NotFound)]
    [InlineData(ResultStatus.Unauthenticated, HttpStatusCode.Unauthorized)]
    [InlineData(ResultStatus.Unauthorized, HttpStatusCode.Forbidden)]
    [InlineData(ResultStatus.PurchaseRequired, HttpStatusCode.PaymentRequired)]
    [InlineData(ResultStatus.PurchaseExpired, HttpStatusCode.PaymentRequired)]
    [InlineData(ResultStatus.ServerError, HttpStatusCode.InternalServerError)]
    [InlineData(ResultStatus.ServiceUnavailable, HttpStatusCode.ServiceUnavailable)]
    public void ToHttpStatusCode_KnownValues_MapToExpected(ResultStatus status, HttpStatusCode expected)
    {
        Assert.Equal(expected, status.ToHttpStatusCode());
    }

    [Fact]
    public void ToHttpStatusCode_UnknownValue_FallsBackToInternalServerError()
    {
        var unknown = (ResultStatus)(-1);
        Assert.Equal(HttpStatusCode.InternalServerError, unknown.ToHttpStatusCode());
    }

    [Fact]
    public void ToHttpStatusCode_CoversEveryDefinedValue()
    {
        // Regression guard: if a new ResultStatus member is added and the mapper
        // isn't updated, it'll silently fall through to InternalServerError.
        // This test will surface that by failing.
        foreach (ResultStatus status in Enum.GetValues<ResultStatus>())
        {
            var mapped = status.ToHttpStatusCode();

            // ServerError legitimately maps to 500. Anything else falling through
            // to InternalServerError indicates an unmapped value.
            if (status != ResultStatus.ServerError)
            {
                Assert.True(
                    mapped != HttpStatusCode.InternalServerError,
                    $"ResultStatus.{status} maps to InternalServerError — looks unmapped.");
            }
        }
    }
}
