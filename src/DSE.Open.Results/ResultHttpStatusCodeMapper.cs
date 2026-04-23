// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Net;

namespace DSE.Open.Results;

/// <summary>
/// Maps <see cref="ResultStatus"/> values onto the equivalent
/// <see cref="HttpStatusCode"/>. Convenient when surfacing a
/// <see cref="Result"/> from a web endpoint.
/// </summary>
public static class ResultHttpStatusCodeMapper
{
    /// <summary>
    /// Returns the <see cref="HttpStatusCode"/> that corresponds to <paramref name="status"/>.
    /// </summary>
    /// <param name="status">The result status to translate.</param>
    /// <returns>
    /// The mapped HTTP status code. Unknown enum values fall back to
    /// <see cref="HttpStatusCode.InternalServerError"/>.
    /// </returns>
    public static HttpStatusCode ToHttpStatusCode(this ResultStatus status)
    {
        return status switch
        {
            ResultStatus.Unspecified => HttpStatusCode.OK,
            ResultStatus.Found => HttpStatusCode.OK,
            ResultStatus.Created => HttpStatusCode.Created,
            ResultStatus.Updated => HttpStatusCode.OK,
            ResultStatus.Deleted => HttpStatusCode.NoContent,
            ResultStatus.Accepted => HttpStatusCode.Accepted,
            ResultStatus.BadRequest => HttpStatusCode.BadRequest,
            ResultStatus.RuleViolation => HttpStatusCode.UnprocessableEntity,
            ResultStatus.StateConflict => HttpStatusCode.Conflict,
            ResultStatus.NotFound => HttpStatusCode.NotFound,
            ResultStatus.Unauthenticated => HttpStatusCode.Unauthorized,
            ResultStatus.Unauthorized => HttpStatusCode.Forbidden,
            ResultStatus.PurchaseRequired => HttpStatusCode.PaymentRequired,
            ResultStatus.PurchaseExpired => HttpStatusCode.PaymentRequired,
            ResultStatus.ServerError => HttpStatusCode.InternalServerError,
            ResultStatus.ServiceUnavailable => HttpStatusCode.ServiceUnavailable,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
