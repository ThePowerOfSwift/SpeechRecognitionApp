using System;

namespace VoiceToCommand
{
    public class VoiceCommand : IVoiceCommand
    {
        private readonly Action _action;

        /// <summary>
        ///     Tells about what needs to be done for given command
        /// </summary>
        /// <param name="action">Takes action as parameter</param>
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