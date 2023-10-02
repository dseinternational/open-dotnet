// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using DSE.Open.Values;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;

namespace DSE.Open.Web.Routing;

public class UriPathRouteConstraint : IRouteConstraint, IParameterLiteralNodeMatchingPolicy
{
    /// <inheritdoc />
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        Guard.IsNotNull(routeKey);
        Guard.IsNotNull(values);

        if (values.TryGetValue(routeKey, out var value))
        {
            if (value is UriPath)
            {
                return true;
            }

            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            return CheckConstraintCore(valueString);
        }

        return false;
    }

    private static bool CheckConstraintCore(string? valueString)
        => string.IsNullOrEmpty(valueString) || UriPath.IsValidValue(valueString.AsSpan(), true);

    public bool MatchesLiteral(string parameterName, string literal)
        => CheckConstraintCore(literal);
}
