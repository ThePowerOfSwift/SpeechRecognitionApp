using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VoiceToCommand.Core
{
    /// <summary>
    ///     Represents collection of methods which can be used by Android and iOS
    /// </summary>
    public abstract class VoiceToCommandService : IVoiceToCommandService
    {
        protected IDictionary<string, IVoiceCommand> AllRegisteredCommands;
        

        public Action<string> FinishAction;

        
        /// <summary>
        ///     Adds commands to dictionary
        /// </summary>
        public VoiceToCommandService()
        {
            AllRegisteredCommands = new Dictionary<string, IVoiceCommand>();
        }

        public abstract void StartListening();


        public abstract void StopListening();


        public abstract bool IsListening();


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
            return AllRegisteredCommands.Where(item => item.Value.CanExecute()).Select(item => item.Key).ToList();
        }

        protected void ExecuteRecognizedCommand(string recognizedString)
        {
            if (AllRegisteredCommands.ContainsKey(recognizedString))
            {
                ExecuteCommand(recognizedString);
            }
            else
            {
                var result = GetAllWords(recognizedString).Where(word => AllRegisteredCommands.ContainsKey(word))
                    .FirstOrDefault();
                
                if (result != null)
                {
                    var data = Regex.Match(recognizedString, @"\d+").Value;
                    if(data.Length>0)
                    {
                        System.Diagnostics.Debug.WriteLine(data);
                        var command = AllRegisteredCommands[result];
                        command?.ExecuteWithIntResult(int.Parse(data));
                    }

                    var substring = recognizedString.Substring(recognizedString.IndexOf(result)+result.Length);
                    var testCommand = AllRegisteredCommands[result];
                    testCommand?.ExecuteWithStringResult(substring);
                }
                else
                    FindApproximateCommand(recognizedString);
            }
            
            FinishAction?.Invoke("Unrecognized command");
            
        }

        private void FindApproximateCommand(string recognizedString)
        {
            foreach (var key in AllRegisteredCommands.Keys)
            {
                if (FuzzyStringMatcher.IsSuitableString(key, recognizedString))
                    ExecuteCommand(key);
            }
        }

        private void ExecuteCommand(string commandString)
        {
            var command = AllRegisteredCommands[commandString];
            if (command.CanExecute())
            {
                command?.Execute();
               // command?.ExecuteWithIntResult()
            }
        }

        private IList<string> GetAllWords(string sentence)
        {
            return sentence.Split(' ').ToList();
        }

        public void RecognitionFinishAction(Action<string> callback)
        {
            FinishAction = callback;
        }
    }
}