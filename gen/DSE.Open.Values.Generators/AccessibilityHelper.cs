// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace DSE.Open.Values.Generators;

public static class AccessibilityHelper
{
    public static string GetKeyword(Accessibility accessibility)
    {
        return accessibility switch
        {
            Accessibility.NotApplicable => throw new NotImplementedException(),
            Accessibility.Private => "private",
            Accessibility.ProtectedAndInternal => "protected internal",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.ProtectedOrInternal => throw new NotImplementedException(),
            Accessibility.Public => "public",
            _ => throw new NotImplementedException(),
        };
    }
}
