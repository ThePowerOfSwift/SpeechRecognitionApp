

namespace VoiceToCommand
{
    public interface IVoiceCommand
    {
        /// <summary>
        ///     Method to execute the command
        /// </summary>
        void Execute();


        /// <summary>
        ///     Tells if command can be executed
        /// </summary>
        /// <returns>Returns true or false</returns>
        bool CanExecute();
    }
}
