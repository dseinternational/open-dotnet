// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Books;

namespace DSE.Open.Language.Readability;

// (0.121 * Average Sentence Length) + (0.082 * Percentage of unique unfamiliar words) + 0.659

public static class SpacheReadabilityCalculator
{
    private const double SentenceCountWeight = 0.121;
    private const double WordCountWeight = 0.082;
    private const double BaseFactor = 0.659;

    public static SpacheReadabilityResult Calculate(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        var allParas = book.Paragraphs.ToArray();

        var allSentences = allParas.SelectMany(p => p.Sentences).ToArray();

        var averageSentenceLength = ((double)allSentences.Select(s => s.Words.Count).Sum()) / allSentences.Length;

        var allWords = allParas.SelectMany(para => para.Words).ToArray();

        var distinctWordCount = allWords.Distinct().Count();

        var unfamiliarWords = allWords.Where(w => IsUnfamiliarWord(w.Form.ToString())).Distinct();

        var unfamiliarWordCount = unfamiliarWords.Count();

        var level = CalculateLevel(averageSentenceLength, distinctWordCount, unfamiliarWordCount);

        return new(level, averageSentenceLength, distinctWordCount,
            unfamiliarWords.Select(w => w.Form.ToString()));
    }

    public static double CalculateLevel(double averageSentenceLength, int distinctWordCount, int unfamiliarWordCount)
    {
        return BaseFactor +
            (SentenceCountWeight * distinctWordCount / averageSentenceLength) +
            (WordCountWeight * unfamiliarWordCount / distinctWordCount * 100d);
    }

    public static int GetUnfamiliarWordCount(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        return words.Distinct().Where(IsUnfamiliarWord).Count();
    }

    public static bool IsFamiliarWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        word = word.ToLower(CultureInfo.CurrentCulture).Trim();

        if (Array.BinarySearch(s_familiarWords, word, StringComparer.CurrentCulture) >= 0)
        {
            return true;
        }

        foreach (var fw in s_familiarWords)
        {
            if (word.Length > fw.Length)
            {
                if (word.Length == fw.Length + 1)
                {
                    if (string.Equals(fw + "s", word, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
                else if (word.Length == fw.Length + 2)
                {
                    if (string.Equals(fw + "ʼs", word, StringComparison.Ordinal)
                        || string.Equals(fw + "ed", word, StringComparison.Ordinal)
                        || string.Equals(fw + "es", word, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    if (fw.EndsWith('e'))
                    {
                        var stem = fw[..^1];
                        if (string.Equals(stem + "ing", word, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }
                }
                else if (word.Length == fw.Length + 3)
                {
                    if (string.Equals(fw + "ing", word, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
                else if (word.Length == fw.Length + 4)
                {
                    if (string.Equals(fw + "ning", word, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    if (string.Equals(fw + "ming", word, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool IsUnfamiliarWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        return !IsFamiliarWord(word);
    }

    private static readonly string[] s_familiarWords =
    [
      "a",
        "able",
        "about",
        "above",
        "across",
        "act",
        "add",
        "afraid",
        "after",
        "afternoon",
        "again",
        "against",
        "ago",
        "air",
        "airplane",
        "alarm",
        "all",
        "almost",
        "alone",
        "along",
        "already",
        "also",
        "always",
        "am",
        "among",
        "an",
        "and",
        "angry",
        "animal",
        "another",
        "answer",
        "any",
        "anyone",
        "appear",
        "apple",
        "are",
        "arm",
        "around",
        "arrow",
        "as",
        "ask",
        "asleep",
        "at",
        "ate",
        "attention",
        "aunt",
        "awake",
        "away",
        "b",
        "baby",
        "back",
        "bad",
        "bag",
        "ball",
        "balloon",
        "bang",
        "bank",
        "bark",
        "barn",
        "basket",
        "be",
        "bean",
        "bear",
        "beat",
        "beautiful",
        "became",
        "because",
        "become",
        "bed",
        "bee",
        "been",
        "before",
        "began",
        "begin",
        "behind",
        "believe",
        "bell",
        "belong",
        "bend",
        "bent",
        "beside",
        "best",
        "better",
        "between",
        "big",
        "bird",
        "birthday",
        "bit",
        "bite",
        "black",
        "blanket",
        "blew",
        "block",
        "blow",
        "blue",
        "board",
        "boat",
        "book",
        "boot",
        "born",
        "borrow",
        "both",
        "bother",
        "bottle",
        "bottom",
        "bought",
        "bow",
        "box",
        "boy",
        "branch",
        "brave",
        "bread",
        "break",
        "breakfast",
        "breath",
        "brick",
        "bridge",
        "bright",
        "bring",
        "broke",
        "broken",
        "brother",
        "brought",
        "brown",
        "brush",
        "build",
        "bump",
        "burn",
        "bus",
        "busy",
        "but",
        "butter",
        "button",
        "buy",
        "by",
        "c",
        "cabin",
        "cage",
        "cake",
        "call",
        "came",
        "camp",
        "can",
        "can't",
        "candle",
        "candy",
        "cap",
        "captain",
        "car",
        "card",
        "care",
        "careful",
        "carrot",
        "carry",
        "case",
        "castle",
        "cat",
        "catch",
        "cattle",
        "caught",
        "cause",
        "cent",
        "certain",
        "chair",
        "chance",
        "change",
        "chase",
        "chicken",
        "chief",
        "child",
        "children",
        "church",
        "circle",
        "circus",
        "city",
        "clap",
        "clean",
        "clever",
        "cliff",
        "climb",
        "clock",
        "close",
        "cloth",
        "clothes",
        "clown",
        "coat",
        "cold",
        "color",
        "come",
        "comfortable",
        "company",
        "contest",
        "continue",
        "cook",
        "cool",
        "corner",
        "could",
        "count",
        "country",
        "course",
        "cover",
        "cow",
        "crawl",
        "cream",
        "cry",
        "cup",
        "curtain",
        "cut",
        "d",
        "dad",
        "dance",
        "danger",
        "dangerous",
        "dark",
        "dash",
        "daughter",
        "day",
        "dear",
        "decide",
        "deep",
        "desk",
        "did",
        "didn't",
        "die",
        "different",
        "dig",
        "dinner",
        "direction",
        "disappear",
        "disappoint",
        "discover",
        "distance",
        "do",
        "doctor",
        "does",
        "dog",
        "dollar",
        "don't",
        "done",
        "door",
        "down",
        "dragon",
        "dream",
        "dress",
        "drink",
        "drive",
        "drop",
        "drove",
        "dry",
        "duck",
        "during",
        "dust",
        "e",
        "each",
        "eager",
        "ear",
        "early",
        "earn",
        "earth",
        "easy",
        "eat",
        "edge",
        "egg",
        "eight",
        "eighteen",
        "either",
        "elephant",
        "else",
        "emma", // count first names as familiar
        "empty",
        "end",
        "enemy",
        "enough",
        "enter",
        "even",
        "ever",
        "every",
        "everything",
        "exact",
        "except",
        "excite",
        "exclaim",
        "explain",
        "eye",
        "face",
        "fact",
        "fair",
        "fall",
        "family",
        "far",
        "farm",
        "farmer",
        "farther",
        "fast",
        "fat",
        "father",
        "feather",
        "feed",
        "feel",
        "feet",
        "fell",
        "fellow",
        "felt",
        "fence",
        "few",
        "field",
        "fierce",
        "fight",
        "figure",
        "fill",
        "final",
        "find",
        "fine",
        "finger",
        "finish",
        "fire",
        "first",
        "fish",
        "five",
        "flag",
        "flash",
        "flat",
        "flew",
        "floor",
        "flower",
        "fly",
        "follow",
        "food",
        "for",
        "forest",
        "forget",
        "forth",
        "found",
        "four",
        "fourth",
        "fox",
        "fresh",
        "friend",
        "frighten",
        "frog",
        "from",
        "front",
        "fruit",
        "full",
        "fun",
        "funny",
        "fur",
        "g",
        "game",
        "garden",
        "gasp",
        "gate",
        "gave",
        "get",
        "giant",
        "gift",
        "girl",
        "give",
        "glad",
        "glass",
        "go",
        "goat",
        "gone",
        "good",
        "got",
        "grandfather",
        "grandmother",
        "grass",
        "gray",
        "great",
        "green",
        "grew",
        "grin",
        "ground",
        "group",
        "grow",
        "growl",
        "guess",
        "gun",
        "h",
        "had",
        "hair",
        "half",
        "hall",
        "hand",
        "handle",
        "hang",
        "happen",
        "happiness",
        "happy",
        "hard",
        "harm",
        "has",
        "hat",
        "hate",
        "have",
        "he",
        "he's",
        "head",
        "hear",
        "heard",
        "heavy",
        "held",
        "hello",
        "help",
        "hen",
        "her",
        "here",
        "herself",
        "hid",
        "hide",
        "high",
        "hill",
        "him",
        "himself",
        "his",
        "hit",
        "hold",
        "hole",
        "holiday",
        "home",
        "honey",
        "hop",
        "horn",
        "horse",
        "hot",
        "hour",
        "house",
        "how",
        "howl",
        "hum",
        "hundred",
        "hung",
        "hungry",
        "hunt",
        "hurry",
        "hurt",
        "husband",
        "i",
        "i'll",
        "i'm",
        "ice",
        "idea",
        "if",
        "imagine",
        "important",
        "in",
        "inch",
        "indeed",
        "inside",
        "instead",
        "into",
        "invite",
        "is",
        "it",
        "it's",
        "its",
        "j",
        "jacket",
        "jar",
        "jet",
        "job",
        "join",
        "joke",
        "joy",
        "jump",
        "just",
        "k",
        "keep",
        "kept",
        "key",
        "kick",
        "kill",
        "kind",
        "king",
        "kitchen",
        "kitten",
        "knee",
        "knew",
        "knock",
        "know",
        "l",
        "ladder",
        "lady",
        "laid",
        "lake",
        "land",
        "large",
        "last",
        "late",
        "laugh",
        "lay",
        "lazy",
        "lead",
        "leap",
        "learn",
        "least",
        "leave",
        "left",
        "leg",
        "less",
        "let",
        "let's",
        "letter",
        "lick",
        "lift",
        "light",
        "like",
        "line",
        "lion",
        "list",
        "listen",
        "little",
        "live",
        "load",
        "long",
        "look",
        "lost",
        "lot",
        "loud",
        "love",
        "low",
        "luck",
        "lump",
        "lunch",
        "m",
        "machine",
        "made",
        "magic",
        "mail",
        "make",
        "man",
        "many",
        "march",
        "mark",
        "market",
        "master",
        "matter",
        "may",
        "maybe",
        "me",
        "mean",
        "meant",
        "meat",
        "meet",
        "melt",
        "men",
        "merry",
        "met",
        "middle",
        "might",
        "mile",
        "milk",
        "milkman",
        "mind",
        "mine",
        "minute",
        "miss",
        "mistake",
        "moment",
        "money",
        "monkey",
        "month",
        "more",
        "morning",
        "most",
        "mother",
        "mountain",
        "mouse",
        "mouth",
        "move",
        "much",
        "mud",
        "music",
        "must",
        "my",
        "n",
        "name",
        "near",
        "neck",
        "need",
        "needle",
        "neighbor",
        "neighborhood",
        "nest",
        "never",
        "new",
        "next",
        "nibble",
        "nice",
        "night",
        "nine",
        "no",
        "nod",
        "noise",
        "none",
        "north",
        "nose",
        "not",
        "note",
        "nothing",
        "notice",
        "now",
        "number",
        "o",
        "ocean",
        "of",
        "off",
        "offer",
        "often",
        "oh",
        "old",
        "on",
        "once",
        "one",
        "only",
        "open",
        "or",
        "orange",
        "order",
        "other",
        "our",
        "out",
        "outside",
        "over",
        "owl",
        "own",
        "p",
        "pack",
        "paid",
        "pail",
        "paint",
        "pair",
        "palace",
        "pan",
        "paper",
        "parade",
        "parent",
        "park",
        "part",
        "party",
        "pass",
        "past",
        "pasture",
        "path",
        "paw",
        "pay",
        "peanut",
        "peek",
        "pen",
        "penny",
        "people",
        "perfect",
        "perhaps",
        "person",
        "pet",
        "pick",
        "picket",
        "picnic",
        "picture",
        "pie",
        "piece",
        "pig",
        "pile",
        "pin",
        "place",
        "plan",
        "plant",
        "play",
        "pleasant",
        "please",
        "plenty",
        "plow",
        "point",
        "poke",
        "pole",
        "policeman",
        "pond",
        "poor",
        "pop",
        "postman",
        "pot",
        "potato",
        "pound",
        "pour",
        "practice",
        "prepare",
        "present",
        "pretend",
        "pretty",
        "princess",
        "prize",
        "probably",
        "problem",
        "promise",
        "protect",
        "proud",
        "puff",
        "pull",
        "puppy",
        "push",
        "put",
        "q",
        "queen",
        "queer",
        "quick",
        "quiet",
        "quite",
        "r",
        "rabbit",
        "raccoon",
        "race",
        "radio",
        "rag",
        "rain",
        "raise",
        "ran",
        "ranch",
        "rang",
        "reach",
        "read",
        "ready",
        "real",
        "red",
        "refuse",
        "remember",
        "reply",
        "rest",
        "return",
        "reward",
        "rich",
        "ride",
        "right",
        "ring",
        "river",
        "road",
        "roar",
        "rock",
        "rode",
        "roll",
        "roof",
        "room",
        "rope",
        "round",
        "row",
        "rub",
        "rule",
        "run",
        "rush",
        "s",
        "sad",
        "safe",
        "said",
        "sail",
        "sale",
        "salt",
        "same",
        "sand",
        "sang",
        "sat",
        "save",
        "saw",
        "say",
        "scare",
        "school",
        "scold",
        "scratch",
        "scream",
        "sea",
        "seat",
        "second",
        "secret",
        "see",
        "seed",
        "seem",
        "seen",
        "sell",
        "send",
        "sent",
        "seven",
        "several",
        "sew",
        "shadow",
        "shake",
        "shall",
        "shape",
        "she",
        "sheep",
        "shell",
        "shine",
        "ship",
        "shoe",
        "shone",
        "shook",
        "shoot",
        "shop",
        "shore",
        "short",
        "shot",
        "should",
        "show",
        "sick",
        "side",
        "sight",
        "sign",
        "signal",
        "silent",
        "silly",
        "silver",
        "since",
        "sing",
        "sister",
        "sit",
        "six",
        "size",
        "skip",
        "sky",
        "sled",
        "sleep",
        "slid",
        "slide",
        "slow",
        "small",
        "smart",
        "smell",
        "smile",
        "smoke",
        "snap",
        "sniff",
        "snow",
        "so",
        "soft",
        "sold",
        "some",
        "something",
        "sometimes",
        "son",
        "song",
        "soon",
        "sorry",
        "sound",
        "speak",
        "special",
        "spend",
        "spill",
        "splash",
        "spoke",
        "spot",
        "spread",
        "spring",
        "squirrel",
        "stand",
        "star",
        "start",
        "station",
        "stay",
        "step",
        "stick",
        "still",
        "stone",
        "stood",
        "stop",
        "store",
        "story",
        "straight",
        "strange",
        "street",
        "stretch",
        "strike",
        "strong",
        "such",
        "sudden",
        "sugar",
        "suit",
        "summer",
        "sun",
        "supper",
        "suppose",
        "sure",
        "surprise",
        "swallow",
        "sweet",
        "swim",
        "swing",
        "t",
        "table",
        "tail",
        "take",
        "talk",
        "tall",
        "tap",
        "taste",
        "teach",
        "teacher",
        "team",
        "tear",
        "teeth",
        "telephone",
        "tell",
        "ten",
        "tent",
        "than",
        "thank",
        "that",
        "that's",
        "the",
        "their",
        "them",
        "then",
        "there",
        "these",
        "they",
        "thick",
        "thin",
        "thing",
        "think",
        "third",
        "this",
        "those",
        "though",
        "thought",
        "three",
        "threw",
        "through",
        "throw",
        "tie",
        "tiger",
        "tight",
        "time",
        "tiny",
        "tip",
        "tire",
        "to",
        "today",
        "toe",
        "together",
        "told",
        "tom", // count first names as familiar
        "tomorrow",
        "too",
        "took",
        "tooth",
        "top",
        "touch",
        "toward",
        "tower",
        "town",
        "toy",
        "track",
        "traffic",
        "train",
        "trap",
        "tree",
        "trick",
        "trip",
        "trot",
        "truck",
        "true",
        "trunk",
        "try",
        "turkey",
        "turn",
        "turtle",
        "twelve",
        "twin",
        "two",
        "u",
        "ugly",
        "uncle",
        "under",
        "unhappy",
        "until",
        "up",
        "upon",
        "upstairs",
        "us",
        "use",
        "usual",
        "v",
        "valley",
        "vegetable",
        "very",
        "village",
        "visit",
        "voice",
        "w",
        "wag",
        "wagon",
        "wait",
        "wake",
        "walk",
        "want",
        "war",
        "warm",
        "was",
        "wash",
        "waste",
        "watch",
        "water",
        "wave",
        "way",
        "we",
        "wear",
        "weather",
        "week",
        "well",
        "went",
        "were",
        "wet",
        "what",
        "wheel",
        "when",
        "where",
        "which",
        "while",
        "whisper",
        "whistle",
        "white",
        "who",
        "whole",
        "whose",
        "why",
        "wide",
        "wife",
        "will",
        "win",
        "wind",
        "window",
        "wing",
        "wink",
        "winter",
        "wire",
        "wise",
        "wish",
        "with",
        "without",
        "woke",
        "wolf",
        "woman",
        "women",
        "won't",
        "wonder",
        "wood",
        "word",
        "wore",
        "work",
        "world",
        "worm",
        "worry",
        "worth",
        "would",
        "wrong",
        "x",
        "y",
        "yard",
        "year",
        "yell",
        "yellow",
        "yes",
        "yet",
        "you",
        "young",
        "your",
        "z",
        "zoo"
    ];

    public static readonly IReadOnlyList<string> FamiliarWords
        = s_familiarWords.ToList().AsReadOnly();

}
