// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile;

public static class TurnstileErrorCodes
{
    /// <summary>
    /// The secret parameter was not passed.
    /// </summary>
    public const string MissingInputSecret = "missing-input-secret";

    /// <summary>
    /// The secret parameter was invalid or did not exist.
    /// </summary>
    public const string InvalidInputSecret = "invalid-input-secret";

    /// <summary>
    /// The response parameter was not passed.
    /// </summary>
    public const string missingInputResponse = "missing-input-response";

    /// <summary>
    /// The response parameter is invalid or has expired.
    /// </summary>
    public const string InvalidInputResponse = "invalid-input-response";

    /// <summary>
    /// The widget ID extracted from the parsed site secret key was invalid or did not exist.
    /// </summary>
    public const string InvalidWidgetId = "invalid-widget-id";

    /// <summary>
    /// The secret extracted from the parsed site secret key was invalid.
    /// </summary>
    public const string InvalidParsedSecret = "invalid-parsed-secret";

    /// <summary>
    /// The request was rejected because it was malformed.
    /// </summary>
    public const string BadRequest = "bad-request";

    /// <summary>
    /// The response parameter has already been validated before.
    /// </summary>
    public const string TimeoutOrDuplicate = "timeout-or-duplicate";

    /// <summary>
    /// An internal error happened while validating the response. The request can be retried.
    /// </summary>
    public const string InternalError = "internal-error";
}
