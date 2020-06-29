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
            if (AllRegisteredCommands.ContainsKey(recognizedString))
            {
                var command = AllRegisteredCommands[recognizedString];
                if (command.CanExecute())
                {
                    command.Execute();
                }
            }
            else
            {
                foreach (var key in AllRegisteredCommands.Keys)
                {
                    if (FuzzyString.IsSuitableString(key, recognizedString))
                    {
                        var command = AllRegisteredCommands[key];
                        if (command.CanExecute())
                        {
                            command.Execute();
                        }
                    }
                }
            }
        }
    }
}
