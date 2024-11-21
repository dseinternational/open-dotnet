// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;

namespace DSE.Open.Localization.Generators.Resources;

public static class TypeConstraints
{
    private const string SystemNamespace = "global::System";

    private static readonly FrozenSet<string> s_languageKeywords = new List<string>()
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while"
    }.ToFrozenSet();

    public static readonly FrozenDictionary<string, string> Lookup =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // C# Keywords
                { "object", "object" },

                { "string", "string" },
                { "bool", "bool" },
                { "byte", "byte" },
                { "sbyte", "sbyte" },
                { "short", "short" },
                { "ushort", "ushort" },
                { "int", "int" },
                { "uint", "uint" },
                { "long", "long" },
                { "ulong", "ulong" },
                { "float", "float" },
                { "double", "double" },
                { "decimal", "decimal" },
                { "char", "char" },

                // Common .NET Types
                { "Guid", $"{SystemNamespace}.Guid" },
                { "Uri", $"{SystemNamespace}.Uri" },

                { "DateTime", $"{SystemNamespace}.DateTime" },
                { "DateTimeOffset", $"{SystemNamespace}.DateTimeOffset" },
                { "DateOnly", $"{SystemNamespace}.DateOnly" },
                { "TimeOnly", $"{SystemNamespace}.TimeOnly" },
                { "TimeSpan", $"{SystemNamespace}.TimeSpan" },

                { "ReadOnlySpan<char>", $"{SystemNamespace}.ReadOnlySpan<char>" },
            }
            .ToFrozenDictionary();

    public static bool IsLanguageKeyword(string text)
    {
        return s_languageKeywords.Contains(text);
    }
}
