// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed class ObservationParameterProvider<TParam>
    where TParam : struct, IEquatable<TParam>
{
    public static readonly ObservationParameterProvider<TParam> Default = new();

    public ObservationParameterType GetParameterType(TParam param)
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
            return ObservationParameterType.Integer;
        }

        if (typeof(TParam) == typeof(SpeechSound))
        {
            return ObservationParameterType.Text;
        }

        ThrowHelper.ThrowInvalidOperationException();
        return default;
    }

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
