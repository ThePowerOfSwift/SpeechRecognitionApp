using System;
using System.Collections.Generic;


namespace VoiceToCommand.Core
{
    /// <summary>
    /// Approximate String Comparision
    /// </summary>
    /// <param name="command">command present in available commands </param>
    /// <param name="recognized">string recognized by speech recognizer</param>
    /// <returns> true or false</returns>
    public static class FuzzyStringMatcher 
    {
        public static bool IsSuitableString(string command, string recognized)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();
            bool result = false;


            if (recognized.Contains(" "))
            {
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                options.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);

                var resultWeak = command.ApproximatelyEquals(recognized, FuzzyStringComparisonTolerance.Weak, options.ToArray());
                var resultNormal = command.ApproximatelyEquals(recognized, FuzzyStringComparisonTolerance.Normal, options.ToArray());


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

                var result1 = command.ApproximatelyEquals(recognized, FuzzyStringComparisonTolerance.Weak, options.ToArray());
                var result2 = command.ApproximatelyEquals(recognized, FuzzyStringComparisonTolerance.Normal, options.ToArray());

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
