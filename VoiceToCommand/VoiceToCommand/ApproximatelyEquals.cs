﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VoiceToCommand
{
    public static partial class ComparisonMetrics
    {
        public static bool ApproximatelyEquals(this string source, string target,
            FuzzyStringComparisonTolerance tolerance, params FuzzyStringComparisonOptions[] options)
        {
            var comparisonResults = new List<double>();

            if (!options.Contains(FuzzyStringComparisonOptions.CaseSensitive))
            {
                source = source.Capitalize();
                target = target.Capitalize();
            }

            // Min: 0    Max: source.Length = target.Length
            if (options.Contains(FuzzyStringComparisonOptions.UseHammingDistance))
                if (source.Length == target.Length)
                    comparisonResults.Add(source.HammingDistance(target) / target.Length);


            // Min: 0    Max: LevenshteinDistanceUpperBounds - LevenshteinDistanceLowerBounds
            // Min: LevenshteinDistanceLowerBounds    Max: LevenshteinDistanceUpperBounds
            if (options.Contains(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance))
                comparisonResults.Add(Convert.ToDouble(source.NormalizedLevenshteinDistance(target)) /
                                      Convert.ToDouble(Math.Max(source.Length, target.Length) -
                                                       source.LevenshteinDistanceLowerBounds(target)));
            else if (options.Contains(FuzzyStringComparisonOptions.UseLevenshteinDistance))
                comparisonResults.Add(Convert.ToDouble(source.LevenshteinDistance(target)) /
                                      Convert.ToDouble(source.LevenshteinDistanceUpperBounds(target)));

            if (options.Contains(FuzzyStringComparisonOptions.UseLongestCommonSubsequence))
                comparisonResults.Add(1 - Convert.ToDouble(source.LongestCommonSubsequence(target).Length /
                                                           Convert.ToDouble(Math.Min(source.Length, target.Length))));

            if (options.Contains(FuzzyStringComparisonOptions.UseLongestCommonSubstring))
                comparisonResults.Add(1 - Convert.ToDouble(source.LongestCommonSubstring(target).Length /
                                                           Convert.ToDouble(Math.Min(source.Length, target.Length))));


            if (comparisonResults.Count == 0) return false;

            if (tolerance == FuzzyStringComparisonTolerance.Strong)
            {
                if (comparisonResults.Average() < 0.25)
                    return true;
                return false;
            }

            if (tolerance == FuzzyStringComparisonTolerance.Normal)
            {
                if (comparisonResults.Average() < 0.5)
                    return true;
                return false;
            }

            if (tolerance == FuzzyStringComparisonTolerance.Weak)
            {
                if (comparisonResults.Average() < 0.75)
                    return true;
                return false;
            }

            if (tolerance == FuzzyStringComparisonTolerance.Manual)
            {
                if (comparisonResults.Average() > 0.6)
                    return true;
                return false;
            }

            return false;
        }
    }
}