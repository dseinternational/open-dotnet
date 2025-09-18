// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DSE.Open.Localization.Generators.Resources.Parsing;

public sealed class ResourceValueParser
{
    private const int MaxIterations = 100;

    public static bool TryParseLine(
        TextLine line,
        string filePath,
        Action<Diagnostic> reportDiagnostic,
        out ResourceItem? resourceItem)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (reportDiagnostic is null)
        {
            throw new ArgumentNullException(nameof(reportDiagnostic));
        }

        var span = line.ToString().AsSpan();
        var delimiterIndex = span.IndexOf('=');

        if (delimiterIndex < 0)
        {
            // Not a key-value pair.
            resourceItem = null;
            return false;
        }

        var second = span.Slice(delimiterIndex + 1).IndexOf('=');

        if (second >= 0)
        {
            // There are multiple '=' characters on a single line, which is invalid.
            var pos = delimiterIndex + 1 + second;
            reportDiagnostic(Diagnostics.MultipleEqualsDelimiters(filePath, line, pos));
            resourceItem = null;
            return false;
        }

        var trimmedKey = span.Slice(0, delimiterIndex).Trim();

        if (trimmedKey.IndexOfAny([' ', '\t']) >= 0)
        {
            // There are spaces in the key, which is invalid (for now)
            reportDiagnostic(Diagnostics.InvalidKey(filePath, line, span.Slice(0, delimiterIndex).ToString()));
            resourceItem = null;
            return false;
        }

        var key = trimmedKey.ToString();

        // Slice from the char after the key-value delimiter.
        var offset = delimiterIndex + 1;
        var slice = span.Slice(offset);

        // Trim the leading and trailing whitespace.
        //
        // We need to patch up the offset with the number of trimmed leading chars so
        // that diagnostics are reported correctly.
        var startTrimmed = slice.TrimStart();
        var trimmedLeadingCharCount = slice.Length - startTrimmed.Length;
        offset += trimmedLeadingCharCount;

        var trimmed = startTrimmed.TrimEnd();

        if (!TryParseValue(trimmed, filePath, line, offset, reportDiagnostic, out var holes))
        {
            resourceItem = null;
            return false;
        }

        resourceItem = ResourceItem.Create(key, holes, slice.Length);
        return true;
    }

    public static bool TryParseValue(
        ReadOnlySpan<char> source,
        string path,
        TextLine line,
        int offset,
        Action<Diagnostic> reportDiagnostic,
        out ReadOnlyCollection<Hole> holes)
    {
        if (reportDiagnostic is null)
        {
            throw new ArgumentNullException(nameof(reportDiagnostic));
        }

        List<Hole> holesList = [];

        var sourceIndex = 0;
        var remainingSlice = source;
        var iterations = 0;
        var anyErrors = false;

        while (TryGetNextStart(remainingSlice, out var startIndex))
        {
            if (++iterations > MaxIterations)
            {
                throw new InvalidOperationException(
                    "Exceeded maximum iterations. This is likely a source generator bug.");
            }

            sourceIndex += startIndex;

            if (sourceIndex >= source.Length)
            {
                // No closing brace found.
                break;
            }

            var slice = source.Slice(sourceIndex);
            var closeIndex = slice.IndexOf('}');

            if (closeIndex < 0)
            {
                // No closing brace found.
                break;
            }

            // Advance `remainingSlice` over the current hole
            var step = closeIndex + 1;
            remainingSlice = slice.Slice(step);

            if (closeIndex == 1)
            {
                // Empty hole "{}"
                var start = startIndex + offset;
                var end = start + 1;
                reportDiagnostic(Diagnostics.EmptyHole(path, line, start, end));
                anyErrors = true;
                sourceIndex += step;
                continue;
            }

            // Slice over the innards of the hole (excluding the enclosing brackets)
            var holeContents = slice.Slice(1, closeIndex - 1);

            var separatorIndex = holeContents.IndexOf(':');

            if (separatorIndex == 0)
            {
                // No name before the separator "{:"
                var start = startIndex + offset;
                var end = start + closeIndex;
                reportDiagnostic(Diagnostics.NoNameInHole(path, line, start, end));
                anyErrors = true;
                sourceIndex += step;
                continue;
            }

            if (separatorIndex == holeContents.Length - 1)
            {
                // No type after the separator "{name:}"
                var start = startIndex + offset;
                var end = start + closeIndex;
                reportDiagnostic(Diagnostics.NoTypeInHole(path, line, start, end));
                anyErrors = true;
                sourceIndex += step;
                continue;
            }

            if (anyErrors)
            {
                // If we've emitted an error, do not emit information about the line
                // which contained error-producing holes so we don't generate code.
                //
                // We do, however, want to continue, so we can report all errors for
                // a given line.
                sourceIndex += step;
                continue;
            }

            var name = GetName(holeContents, separatorIndex);

            var type = separatorIndex switch
            {
                < 0 => null,
                _ => holeContents.Slice(separatorIndex + 1).ToString(),
            };

            if (holesList.Any(h => h.Name == name && h.Type != type))
            {
                var start = startIndex + offset;
                var end = start + closeIndex;
                reportDiagnostic(Diagnostics.ConflictingTypeConstraint(path, line, start, end, name));
                anyErrors = true;
                sourceIndex += step;
                continue;
            }

            holesList.Add(new Hole
            {
                Index = sourceIndex,
                Length = closeIndex + 1,
                Name = name,
                Type = type
            });

            sourceIndex += step;
        }

        if (anyErrors)
        {
            holes = new ReadOnlyCollection<Hole>([]);
            return false;
        }

        holes = new ReadOnlyCollection<Hole>(holesList);
        return true;
    }

    private static string GetName(ReadOnlySpan<char> holeContents, int separatorIndex)
    {
        var name = separatorIndex switch
        {
            < 0 => holeContents.ToString(),
            _ => holeContents.Slice(0, separatorIndex).ToString(),
        };

        // Do some normalization to .NET style variable naming, if needed.
        name = char.IsUpper(name[0]) switch
        {
            true => $"{char.ToLower(name[0], CultureInfo.InvariantCulture)}{name.Substring(1)}",
            false => name
        };

        // If the name is an index, rename to `arg{index}`
        name = int.TryParse(name, out var index) switch
        {
            true => $"arg{index}",
            false => name
        };

        name = TypeConstraints.IsLanguageKeyword(name) switch
        {
            true => $"@{name}",
            false => name
        };
        return name;
    }

    public static bool TryGetNextStart(ReadOnlySpan<char> source, out int index)
    {
        index = source.IndexOf('{');
        return index >= 0;
    }
}
