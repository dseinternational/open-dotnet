// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using DSE.Open.Localization.Generators.Resources.Parsing;

namespace DSE.Open.Localization.Generators.Resources;

internal static class InterpolatedStringBuilder
{
    internal static string CreateInterpolatedString(
        ResourceItem item,
        List<ParameterDefinition> parameters)
    {
        var holes = item.Holes;
        var formatLength = item.FormatLength;

        var builder = new StringBuilder();

        var formatIndex = 0;

        for (var i = 0; i < holes.Count; i++)
        {
            var hole = holes[i];
            var param = parameters[i];

            if (hole.Index > formatIndex)
            {
                // The next hole is preceded by a constant string.
                _ = builder.Append(
                    $$"""
                      {format[{{formatIndex}}..{{hole.Index - 1}}]}
                      """);
            }

            _ = builder.Append(
                $$"""
                  {{{param.Name}}}
                  """);

            // "t{0}t" -> 1 + 3 = 4
            formatIndex = hole.Index + hole.Length;

            if (i == holes.Count - 1 && formatIndex < formatLength)
            {
                // Copy the trailing constant string.
                _ = builder.Append(
                    $$"""
                      {format[{{formatIndex}}..]}
                      """);
            }
        }

        return builder.ToString();
    }
}
