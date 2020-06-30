namespace VoiceToCommand.Core
{
    public enum FuzzyStringComparisonOptions
    {
        UseHammingDistance,

        UseLevenshteinDistance,

        UseLongestCommonSubsequence,

        UseLongestCommonSubstring,

        UseNormalizedLevenshteinDistance,

        CaseSensitive
    }
}