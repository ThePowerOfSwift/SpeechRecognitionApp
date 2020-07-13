namespace VoiceToCommand
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