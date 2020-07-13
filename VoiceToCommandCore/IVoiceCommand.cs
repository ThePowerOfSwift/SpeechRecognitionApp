namespace VoiceToCommand.Core
{
    public interface IVoiceCommand
    {
        /// <summary>
        ///     Method to execute the command
        /// </summary>
        void Execute();
        void ExecuteWithIntResult(int result);
        void ExecuteWithStringResult(string result);

        /// <summary>
        ///     Tells if command can be executed
        /// </summary>
        /// <returns>Returns true or false</returns>
        bool CanExecute();
    }
}