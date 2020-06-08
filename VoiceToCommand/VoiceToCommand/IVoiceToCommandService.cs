﻿using System;
using System.Collections.Generic;

namespace VoiceToCommand
{
    public interface IVoiceToCommandService
    {
        void StartListening();

        void StopListening();

        bool IsListening();

        void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted);

        List<string> GetAvailableCommands(); 

        List<string> GetExecutableCommands(); 

        void DeregisterCommand(string commandString);

        void RegisterListeningCompletedCallBack(Action callBack); 

        void DeregisterListeningCompletedCallBack(Action callBack); 

        void RegisterUnrecognizableCommandCallBack(Action callBack);

        void DeregisterUnrecognizableCommandCallBack(Action callBack); 

        void RegisterUnexecuatbleCallBack(Action callBack);

        void DeregisterUnexecuatbleCallBack(Action callBack); 
    }

    public interface IVoiceCommandServiceFactory
    {
        IVoiceToCommandService Create();
    }
}
