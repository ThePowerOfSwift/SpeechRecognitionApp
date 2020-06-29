using System.Collections.Generic;
using FuzzyString;

namespace VoiceToCommand.Core
{
    /// <summary>
    /// Approximate String Comparision
    /// </summary>
    /// <param name="command">command present in available commands </param>
    /// <param name="recognized">string recognized by speech recognizer</param>
    /// <returns> true or false</returns>
    public static class FuzzyString
    {
        public static bool IsSuitableString(string command, string recognized)
        {
            var options = new List<FuzzyStringComparisonOptions>();
            bool result = false;


            if (recognized.Contains(" "))
            {
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                options.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);

                var resultWeak = command.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Weak);
                var resultNormal = command.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Normal);


                if (resultWeak && resultNormal)
                {
                    result = true;
                    return result;
                }
            }

            else
            {
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                options.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);

                var result1 = command.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Weak);
                var result2 = command.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Normal);

                var resultTot = result1 && result2;

                if (result1 && command[0] == recognized[0])
                {
                    result = true;
                }
                else if (resultTot)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
