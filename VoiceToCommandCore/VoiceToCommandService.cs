using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FuzzyString;

namespace VoiceToCommand.Core
{
    /// <summary>
    /// Represents collection of methods which can be used by Android and iOS 
    /// </summary>
    public abstract class VoiceToCommandService : IVoiceToCommandService
    {
        protected IDictionary<string, IVoiceCommand> AllRegisteredCommands;

        /// <summary>
        /// Adds commands to dictionary
        /// </summary>
        public VoiceToCommandService()
        {
            AllRegisteredCommands = new Dictionary<string, IVoiceCommand>();
        }

        public abstract void StartListening();


        public abstract void StopListening();


        public abstract bool IsListening();

        /// <summary>
        /// Approximate String Comparision
        /// </summary>
        /// <param name="commands">command present in available commands </param>
        /// <param name="recognized">string recognized by speech recognizer</param>
        /// <returns> true or false</returns>

        public bool FuzzyString(string commands , string recognized)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();
            bool result = false;
           

            if (recognized.Contains(" "))
            {
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                options.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);

                bool resultWeak = commands.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Weak);
                bool resultNormal = commands.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Normal);


                if (resultWeak && resultNormal == true)
                {
                    result = true;
                }

            }

            else
            {
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                options.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);



                bool result1 = commands.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Weak);
                bool result2 = commands.ApproximatelyEquals(recognized, options, FuzzyStringComparisonTolerance.Normal);

                bool resultTot = result1 && result2;

                if (commands[0] == recognized[0] && result1 == true)
                {
                    result = true;

                }


                else if (commands[0] != recognized[0] && resultTot == true)
                {
                    result = true;

                }
            }
            
            
            return result;
        }


        public void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted)
        {
            AllRegisteredCommands.Add(commandString.ToLower(), commandToBeExecuted);
        }

        public IList<string> GetAvailableCommands()
        {
            return AllRegisteredCommands.Keys.ToList();
        }

        public IList<string> GetExecutableCommands()
        {
            return (AllRegisteredCommands.Where(item => item.Value.CanExecute()).Select(item => item.Key)).ToList();
        }

    }
}
