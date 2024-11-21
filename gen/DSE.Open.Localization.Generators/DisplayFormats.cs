// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace DSE.Open.Localization.Generators;

public static class DisplayFormats
{
    public static readonly SymbolDisplayFormat FullyQualifiedNonGenericWithGlobalPrefix = new(
        SymbolDisplayGlobalNamespaceStyle.Included,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.None,
        SymbolDisplayMemberOptions.IncludeContainingType,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeType,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.IncludeType,
        SymbolDisplayKindOptions.None,
        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
    );

    public static readonly SymbolDisplayFormat FullyQualifiedGenericWithGlobalPrefix = new(
        SymbolDisplayGlobalNamespaceStyle.Included,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.IncludeTypeParameters,
        SymbolDisplayMemberOptions.IncludeContainingType,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeType,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.IncludeType,
        SymbolDisplayKindOptions.None,
        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
    );

    public static readonly SymbolDisplayFormat FullyQualifiedNonGeneric = new(
        SymbolDisplayGlobalNamespaceStyle.Omitted,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.None,
        SymbolDisplayMemberOptions.IncludeContainingType,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeType,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.IncludeType,
        SymbolDisplayKindOptions.None,
        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
    );

    public static readonly SymbolDisplayFormat NameOnly = new();
}
