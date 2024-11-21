// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DSE.Open.Localization.Generators.Resources;
internal static class Diagnostics
{
    private const string DSEPrefix = "DSE";

    private const string Prefix = $"{DSEPrefix}RG";

    private const string Category = "DSE.Open.Localization.Generators.Resources";

    private static string GetId(int id)
    {
        return $"{Prefix}{id:D4}";
    }

    public static readonly string FailedToReadFileDiagnosticId = GetId(1);

    public static readonly string MultipleEqualsDelimitersDiagnosticId = GetId(2);

    public static readonly string EmptyHoleDiagnosticId = GetId(3);

    public static readonly string NoNameInHoleDiagnosticId = GetId(4);

    public static readonly string NoTypeInHoleDiagnosticId = GetId(5);

    public static readonly string DuplicateKeyDiagnosticId = GetId(6);

    public static readonly string InvalidKeyDiagnosticId = GetId(7);

    public static readonly string ResourceFileNotFoundDiagnosticId = GetId(8);

    public static readonly string ConflictingTypeConstraintDiagnosticId = GetId(9);

    public static Diagnostic FailedToReadFile(string path)
    {
        var d = new DiagnosticDescriptor(
            FailedToReadFileDiagnosticId,
            "Failed to read file",
            "Failed to read file '{0}'. Are you missing an AdditionalFile in your project file? Some operating systems are case-sensitive.",
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, Location.None, path);
    }

    internal static Diagnostic MultipleEqualsDelimiters(Location location)
    {
        var d = new DiagnosticDescriptor(
            MultipleEqualsDelimitersDiagnosticId,
            "Multiple key value delimiters",
            "Multiple key value delimiters ('=') found.",
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location);
    }

    internal static Diagnostic MultipleEqualsDelimiters(string path, TextLine line, int position)
    {
        var location = CreateLocation(path, line, position, position);
        return MultipleEqualsDelimiters(location);
    }

    internal static Diagnostic EmptyHole(Location location)
    {
        var d = new DiagnosticDescriptor(
            EmptyHoleDiagnosticId,
            "Empty resource string value hole",
            "A hole in a resource string value must not be empty.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location);
    }

    internal static Diagnostic EmptyHole(string path, TextLine line, int start, int end)
    {
        var location = CreateLocation(path, line, start, end);
        return EmptyHole(location);
    }

    internal static Diagnostic NoNameInHole(Location location)
    {
        var d = new DiagnosticDescriptor(
            NoNameInHoleDiagnosticId,
            "Resource string value hole is missing an identifier",
            "All holes in resource string values must have a valid (non-empty and not \":\") identifier.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location);
    }

    internal static Diagnostic NoNameInHole(string path, TextLine line, int start, int end)
    {
        var location = CreateLocation(path, line, start, end);
        return NoNameInHole(location);
    }

    internal static Diagnostic NoTypeInHole(Location location)
    {
        var d = new DiagnosticDescriptor(
            NoTypeInHoleDiagnosticId,
            "Resource string value hole is missing a type",
            "Resource string value is missing a type. Either add one, or remove the \":\" delimiter.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location);
    }

    internal static Diagnostic NoTypeInHole(string path, TextLine line, int start, int end)
    {
        var location = CreateLocation(path, line, start, end);
        return NoTypeInHole(location);
    }

    private static Location CreateLocation(string path, TextLine line, int start, int end)
    {
        var startPos = new LinePosition(line.LineNumber, start);
        var endPos = new LinePosition(line.LineNumber, end);
        var pos = new LinePositionSpan(startPos, endPos);
        return Location.Create(path, line.Span, pos);
    }

    internal static Diagnostic DuplicateKey(Location location, string key)
    {
        var d = new DiagnosticDescriptor(
            DuplicateKeyDiagnosticId,
            "Duplicate resource key",
            "Duplicate key '{0}' found.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location, key);
    }

    internal static Diagnostic DuplicateKey(string path, TextLine line, string key)
    {
        var location = CreateLocation(path, line, 0, key.Length);
        return DuplicateKey(location, key);
    }

    internal static Diagnostic InvalidKey(Location location, string key)
    {
        var d = new DiagnosticDescriptor(
            InvalidKeyDiagnosticId,
            "Invalid resource key",
            "Invalid key '{0}' found.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location, key);
    }

    internal static Diagnostic InvalidKey(string path, TextLine line, string v)
    {
        var location = CreateLocation(path, line, 0, v.Length);
        return InvalidKey(location, v);
    }

    internal static Diagnostic ResourceFileNotFound(string path)
    {
        var d = new DiagnosticDescriptor(
            ResourceFileNotFoundDiagnosticId,
            "No suitable resource file found",
            "No suitable resource file found for '{0}'.",
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, Location.None, path);
    }

    public static Diagnostic ConflictingTypeConstraint(Location location, string name)
    {
        var d = new DiagnosticDescriptor(
            ConflictingTypeConstraintDiagnosticId,
            "Conflicting type constraint",
            "Conflicting type constraint for resource hole with name '{0}'.",
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(d, location, name);
    }

    public static Diagnostic ConflictingTypeConstraint(string path, TextLine line, int start, int end, string name)
    {
        var location = CreateLocation(path, line, start, end);
        return ConflictingTypeConstraint(location, name);
    }
}
