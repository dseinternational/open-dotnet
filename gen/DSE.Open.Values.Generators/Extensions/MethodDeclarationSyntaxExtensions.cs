// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DSE.Open.Values.Generators.Extensions;

internal static class MethodDeclarationSyntaxExtensions
{
    public sealed class KnownMethods
    {
        public const string TryFormat = "TryFormat";

        public const string TryParse = "TryParse";

        public const string Parse = "Parse";
    }

    // public string ToString(string? format, IFormatProvider? provider);
    public static bool IsIFormattableToStringMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != "ToString")
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 2 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is NullableTypeSyntax nts0 &&
            nts0.ElementType is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "string" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider";
    }

    // IParsable<TSelf>.Parse(string s, IFormatProvider? provider)
    public static bool IsIParsableParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.Parse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 2 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "string" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider";
    }

    // bool IParsable<TSelf>.TryParse(string? s, IFormatProvider? provider, out TSelf result)
    public static bool IsIParsableTryParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.TryParse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 3 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is NullableTypeSyntax nts0 &&
            nts0.ElementType is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "string" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider" &&
            s.ParameterList.Parameters[2] is ParameterSyntax ps2 &&
            ps2.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.OutKeyword));
    }

    // ISpanParseable<TSelf>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    public static bool IsISpanParsableParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.Parse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 2 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "ReadOnlySpan" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "char" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider";
    }

    // bool ISpanParseable<TSelf>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TSelf result)
    public static bool IsISpanParsableTryParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.TryParse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 3 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "ReadOnlySpan" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "char" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider" &&
            s.ParameterList.Parameters[2] is ParameterSyntax ps2 &&
            ps2.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.OutKeyword));
    }

    // bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    public static bool IsISpanFormattableTryFormatMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.TryFormat)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 4 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "Span" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "char" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is PredefinedTypeSyntax pts1 &&
            pts1.Keyword.Text == "int" &&
            s.ParameterList.Parameters[2] is ParameterSyntax ps2 &&
            ps2.Type is GenericNameSyntax gns2 &&
            gns2.Identifier.Text == "ReadOnlySpan" &&
            s.ParameterList.Parameters[3] is ParameterSyntax ps3 &&
            ps3.Type is NullableTypeSyntax;
    }

    // public bool TryFormat(this Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
    public static bool IsIUtf8SpanFormattableTryFormatMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.TryFormat)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 4 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "Span" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "byte" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is PredefinedTypeSyntax pts1 &&
            pts1.Keyword.Text == "int" &&
            s.ParameterList.Parameters[2] is ParameterSyntax ps2 &&
            ps2.Type is GenericNameSyntax gns2 &&
            gns2.Identifier.Text == "ReadOnlySpan" &&
            s.ParameterList.Parameters[3] is ParameterSyntax ps3 &&
            ps3.Type is NullableTypeSyntax;
    }

    // TryParse<T>(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out T result)
    public static bool IsIUtf8SpanParsableTryParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.TryParse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 3 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "ReadOnlySpan" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "byte" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax nts1 &&
            nts1.ElementType is IdentifierNameSyntax ins1 &&
            ins1.Identifier.Text == "IFormatProvider" &&
            s.ParameterList.Parameters[2] is ParameterSyntax ps2 &&
            ps2.Modifiers.Any(s => s.IsKind(SyntaxKind.OutKeyword));
    }

    // T Parse<T>(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = default)
    public static bool IsIUtf8SpanParsableParseMethod(this MethodDeclarationSyntax s)
    {
        if (s.Identifier.Text != KnownMethods.Parse)
        {
            return false;
        }

        return s.ParameterList.Parameters.Count == 2 &&
            s.ParameterList.Parameters[0] is ParameterSyntax ps0 &&
            ps0.Type is GenericNameSyntax gns0 &&
            gns0.Identifier.Text == "ReadOnlySpan" &&
            gns0.TypeArgumentList.Arguments.Count == 1 &&
            gns0.TypeArgumentList.Arguments[0] is PredefinedTypeSyntax pts0 &&
            pts0.Keyword.Text == "byte" &&
            s.ParameterList.Parameters[1] is ParameterSyntax ps1 &&
            ps1.Type is NullableTypeSyntax;
    }
}
