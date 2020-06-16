using System;
namespace VoiceToCommandCore
{
    public interface IVoiceCommand
    {
        void Execute();

        bool CanExecute();
    }
}

