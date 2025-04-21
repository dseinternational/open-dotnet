// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

public static class UniversalFeatureValues
{
    /// <summary>
    /// <c>Prs</c>: personal or possessive personal pronoun or determiner.
    /// </summary>
    public static readonly AlphaNumericCode PronominalPersonal = new("Prs");

    /// <summary>
    /// <c>Rcp</c>: reciprocal pronoun
    /// </summary>
    public static readonly AlphaNumericCode PronominalRecipricol = new("Rcp");

    /// <summary>
    /// Art: article
    /// </summary>
    public static readonly AlphaNumericCode PronominalArticle = new("Art");

    /// <summary>
    /// <c>Int</c>: interrogative pronoun, determiner, numeral or adverb
    /// </summary>
    public static readonly AlphaNumericCode PronominalInterrogative = new("Int");

    /// <summary>
    /// <c>Rel</c>: relative pronoun, determiner, numeral or adverb
    /// </summary>
    public static readonly AlphaNumericCode PronominalRelative = new("Rel");

    /// <summary>
    /// <c>Exc</c>: exclamative determiner
    /// </summary>
    public static readonly AlphaNumericCode PronominalExclamative = new("Exc");

    /// <summary>
    /// <c>Dem</c>: demonstrative pronoun, determiner, numeral or adverb
    /// </summary>
    public static readonly AlphaNumericCode PronominalDemonstrative = new("Dem");

    /// <summary>
    /// <c>Finite</c>
    /// </summary>
    public static readonly AlphaNumericCode VerbFormFinite = new("Finite");

    /// <summary>
    /// <c>Infinitive</c>
    /// </summary>
    public static readonly AlphaNumericCode VerbFormInfinitive = new("Infinitive");

    /// <summary>
    /// <c>Part</c>
    /// </summary>
    public static readonly AlphaNumericCode VerbFormParticiple = new("Part");

    /// <summary>
    /// <c>Conv</c>
    /// </summary>
    public static readonly AlphaNumericCode VerbFormConverb = new("Conv");

}
