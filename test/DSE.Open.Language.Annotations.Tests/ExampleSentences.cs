// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public static class ExampleSentences
{
    public static Sentence HeCanSwimAndMustPractiseEveryDay =>
        Sentence.FromConllu(ExampleSentenceDefinitions.HeCanSwimAndMustPractiseEveryDay);

    public static Sentence TheDogIsRunningAndTheHorseIsWalking =>
        Sentence.FromConllu(ExampleSentenceDefinitions.TheDogIsRunningAndTheHorseIsWalking);
}
