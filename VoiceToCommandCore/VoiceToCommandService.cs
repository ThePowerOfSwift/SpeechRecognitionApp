using System;
using System.Collections.Generic;
using System.Linq;

namespace VoiceToCommand.Core
{
    public abstract class VoiceToCommandService : IVoiceToCommandService
    {
        protected IDictionary<string, IVoiceCommand> AllRegisteredCommands;

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

        public List<string> GetAvailableCommands()
        {
            return AllRegisteredCommands.Keys.ToList();
        }

        public List<string> GetExecutableCommands()
        {
            return (AllRegisteredCommands.Where(item => item.Value.CanExecute()).Select(item => item.Key)).ToList();
        }

        public void DeRegisterCommand(string commandString)
        {
            AllRegisteredCommands.Remove(commandString);
        }

        public void RegisterListeningCompletedCallBack(Action callBack)
        {
            //Need to implement
        }

        public void DeRegisterListeningCompletedCallBack(Action callBack)
        {
            //Need to implement
        }

        public void RegisterUnrecognizableCommandCallBack(Action callBack)
        {
            //Need to implement
        }

        public void DeRegisterUnrecognizableCommandCallBack(Action callBack)
        {
            //Need to implement
        }

        public void RegisterUnExecutableCallBack(Action callBack)
        {
            //Need to implement
        }

        public void DeRegisterUnExecutableCallBack(Action callBack)
        {
            //Need to implement
        }
    }
}
