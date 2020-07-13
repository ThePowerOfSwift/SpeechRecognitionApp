using System;

namespace VoiceToCommand.Core
{
    public class VoiceCommand : IVoiceCommand
    {
        private readonly Action _action;
        private readonly Action<int> _actionWithIntResult;
        private readonly Action<string> _actionWithStringResult;

        /// <summary>
        ///     Tells about what needs to be done for given command
        /// </summary>
        /// <param name="action">Takes action as parameter</param>
        public VoiceCommand(Action action)
        {
            _action = action;
        }
        public VoiceCommand(Action<int> action)
        {
            _actionWithIntResult = action;
        }
        public VoiceCommand(Action<string> action)
        {
            _actionWithStringResult = action;
        }

        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
        public void ExecuteWithIntResult(int result)
        {
            _actionWithIntResult?.Invoke(result);
        }
        public void ExecuteWithStringResult(string result)
        {
            _actionWithStringResult?.Invoke(result);
        }
    }
}