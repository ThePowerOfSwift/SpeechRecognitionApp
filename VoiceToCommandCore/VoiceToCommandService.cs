using System.Collections.Generic;
using System.Linq;

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

        protected void ExecuteRecognizedCommand(string recognizedString)
        {
            IVoiceCommand command;
            if (AllRegisteredCommands.ContainsKey(recognizedString))
            {
                command = AllRegisteredCommands[recognizedString];
                if (command.CanExecute())
                {
                    command.Execute();
                }
            }
            else
            {
                foreach (var word in GetAllWords(recognizedString).Where(word => AllRegisteredCommands.ContainsKey(word)))
                {
                    command = AllRegisteredCommands[word];
                    if (command.CanExecute())
                    {
                        command.Execute();
                    }
                    return;
                }
                foreach (var key in AllRegisteredCommands.Keys)
                {
                    if (FuzzyStringMatcher.IsSuitableString(key, recognizedString))
                    {
                        command = AllRegisteredCommands[key];
                        if (command.CanExecute())
                        {
                            command.Execute();
                        }
                    }
                }
            }
        }

        private IList<string> GetAllWords(string sentence)
        {
            return sentence.Split(' ').ToList();
        }
    }
}
