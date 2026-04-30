// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// Resolves the integer or text identifier of an observation parameter of type <typeparamref name="TParam"/>,
/// dispatching to <see cref="IObservationParameter"/> when implemented and otherwise to a fixed set of
/// well-known parameter types.
/// </summary>
/// <typeparam name="TParam">The parameter type.</typeparam>
public sealed class ObservationParameterProvider<TParam>
    where TParam : struct, IEquatable<TParam>
{
    /// <summary>
    /// The default singleton instance for <typeparamref name="TParam"/>.
    /// </summary>
    public static readonly ObservationParameterProvider<TParam> Default = new();

    /// <summary>
    /// Returns whether <typeparamref name="TParam"/> is identified by an integer or by text.
    /// </summary>
    /// <param name="param">A parameter value (used only when <typeparamref name="TParam"/> implements
    /// <see cref="IObservationParameter"/>).</param>
    public MeasurementParameterType GetParameterType(TParam param)
    {
        if (typeof(TParam).IsAssignableTo(typeof(IObservationParameter)))
        {
            return ((IObservationParameter)param).ParameterType;
        }

        if (typeof(TParam) == typeof(WordId)
            || typeof(TParam) == typeof(WordMeaningId)
            || typeof(TParam) == typeof(SentenceId)
            || typeof(TParam) == typeof(SentenceMeaningId))
        {
            return MeasurementParameterType.Integer;
        }

        if (typeof(TParam) == typeof(SpeechSound))
        {
            return MeasurementParameterType.Text;
        }

        ThrowHelper.ThrowInvalidOperationException();
        return default;
    }

    /// <summary>
    /// Returns the integer identifier for <paramref name="param"/>. Throws if <typeparamref name="TParam"/>
    /// is not an integer-identified parameter type.
    /// </summary>
    /// <param name="param">The parameter value.</param>
    public ulong GetIntegerId(TParam param)
    {
        if (typeof(TParam).IsAssignableTo(typeof(IObservationParameter)))
        {
            return ((IObservationParameter)param).GetIntegerId();
        }

        if (param is WordId wordId)
        {
            return wordId;
        }

        if (param is SentenceId sentenceId)
        {
            return sentenceId;
        }

        if (param is WordMeaningId wordMeaningId)
        {
            return wordMeaningId;
        }

        if (param is SentenceMeaningId sentenceMeaningId)
        {
            return sentenceMeaningId;
        }

        return IObservationParameter.ThrowParameterMismatchException<ulong>();
    }

    /// <summary>
    /// Returns the text identifier for <paramref name="param"/>. Throws if <typeparamref name="TParam"/>
    /// is not a text-identified parameter type.
    /// </summary>
    /// <param name="param">The parameter value.</param>
    public ReadOnlySpan<char> GetTextId(TParam param)
    {
        if (typeof(TParam).IsAssignableTo(typeof(IObservationParameter)))
        {
            return ((IObservationParameter)param).GetTextId();
        }

        if (param is SpeechSound speechSound)
        {
            return speechSound.AsCharSpan();
        }

        return IObservationParameter.ThrowParameterMismatchException<char[]>();
    }
}
