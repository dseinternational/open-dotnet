// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Net;

namespace DSE.Open.Results;

public static class ResultHttpStatusCodeMapper
{
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
