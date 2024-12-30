// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public enum ResultStatus
{
    Unspecified,

    /// <summary>
    /// The operation was successful. The resource was found and is included in the result.
    /// </summary>
    Found,

    /// <summary>
    /// The operation was successful. A new resource was created and may be included in the result.
    /// </summary>
    Created,

    /// <summary>
    /// The operation was successful. The resource was updated and may be included in the result.
    /// </summary>
    Updated,

    /// <summary>
    /// The operation was successful. The resource was deleted.
    /// </summary>
    Deleted,

    /// <summary>
    /// The request has been accepted for processing, but the processing has not been completed.
    /// </summary>
    Accepted,

    BadRequest = 900,

    /// <summary>
    /// The request was not successful. The requested resource was not found.
    /// </summary>
    NotFound = 1000,

    /// <summary>
    /// The request was not successful. The user copuld not be authenticated.
    /// </summary>
    Unauthenticated = 2000,

    /// <summary>
    /// The request was not successful. The user is not authorized to perform the operation.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// The request was not successful. The user has not purchased the access required to
    /// perform the operation.
    /// </summary>
    PurchaseRequired = 3000,

    /// <summary>
    /// The request was not successful. The user's purchase has expired.
    /// </summary>
    PurchaseExpired,

    /// <summary>
    /// The server has encountered a situation it does not know how to handle.
    /// </summary>
    ServerError = 5000,

    /// <summary>
    /// The server is currently unable to handle the request due to a temporary overloading
    /// or maintenance of the server.
    /// </summary>
    ServiceUnavailable,
}
