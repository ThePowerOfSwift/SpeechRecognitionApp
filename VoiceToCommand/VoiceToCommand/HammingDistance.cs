namespace VoiceToCommand
{
    public static partial class ComparisonMetrics
    {
        public static int HammingDistance(this string source, string target)
        {
            var distance = 0;

            if (source.Length == target.Length)
            {
                for (var i = 0; i < source.Length; i++)
                    if (!source[i].Equals(target[i]))
                        distance++;
                return distance;
            }

            return 99999;
        }
    }
}