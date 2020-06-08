using System;
using VoiceToCommand;

namespace SpeechRecognitionApp
{
        public class VoiceCommand : IVoiceCommand
        {
            private Action _action;
            public VoiceCommand(Action action)
            {
                _action = action;
            }

            public bool CanExecute()
            {
                return true;
            }

            public void Execute()
            {
                _action.Invoke();
            }
        }
}
