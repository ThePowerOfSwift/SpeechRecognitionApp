using System;
using System.Collections.Generic;

namespace VoiceToCommandLib
{
    public interface IVoiceToCommandService
    {
        void StartListening();

        void StopListening();

        bool IsListening();

        void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted);

        List<string> GetAvailableCommands();

        List<string> GetExecutableCommands();

        void DeRegisterCommand(string commandString);

        void RegisterListeningCompletedCallBack(Action callBack);

        void DeRegisterListeningCompletedCallBack(Action callBack);

        void RegisterUnrecognizableCommandCallBack(Action callBack);

        void DeRegisterUnrecognizableCommandCallBack(Action callBack);

        void RegisterUnExecutableCallBack(Action callBack);

        void DeRegisterUnExecutableCallBack(Action callBack);
    }
}
