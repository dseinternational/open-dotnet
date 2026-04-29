// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;

namespace DSE.Open.Web.Routing;

/// <summary>
/// A route constraint that matches route values that are valid <see cref="UriAsciiPath"/> values.
/// </summary>
public class UriAsciiPathRouteConstraint : IRouteConstraint, IParameterLiteralNodeMatchingPolicy
{
    /// <inheritdoc />
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        ArgumentNullException.ThrowIfNull(routeKey);
        ArgumentNullException.ThrowIfNull(values);

        if (values.TryGetValue(routeKey, out var value))
        {
            if (value is UriAsciiPath)
            {
                return true;
            }

            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            return CheckConstraintCore(valueString);
        }

        return false;
    }

    private static bool CheckConstraintCore(string? valueString)
    {
        return string.IsNullOrEmpty(valueString) || UriAsciiPath.IsValidValue(valueString);
    }

    /// <inheritdoc />
    public bool MatchesLiteral(string parameterName, string literal)
    {
        return CheckConstraintCore(literal);
    }
}
