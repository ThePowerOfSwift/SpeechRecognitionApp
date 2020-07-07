using System;
using System.Collections.Generic;

namespace VoiceToCommand.Core
{
    /// <summary>
    ///     Represents collection of methods for implementing VoiceToCommand
    /// </summary>
    public interface IVoiceToCommandService
    {
        /// <summary>
        ///     Starts listening to speech
        /// </summary>
        void StartListening();

        /// <summary>
        ///     Stops listening to speech
        /// </summary>
        void StopListening();

        /// <summary>
        ///     Returns if recognizer is listening or not
        /// </summary>
        /// <returns>
        ///     The method returns true or false
        /// </returns>
        bool IsListening();


        /// <summary>
        ///     Method to register commands
        /// </summary>
        /// <param name="commandString">commandString is string : name of the command</param>
        /// <param name="commandToBeExecuted">commandToBeExecuted : tells about what needs to be done when command is called</param>
        void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted);

        /// <summary>
        ///     Method to get list of available commands
        /// </summary>
        /// <returns> List of available commands</returns>
        IList<string> GetAvailableCommands();

        /// <summary>
        ///     Method to get list of executable commands
        /// </summary>
        /// <returns>List containing commands which can be executed </returns>
        IList<string> GetExecutableCommands();


        /// <summary>
        /// Callback is called when Command Recognized is not Registered one
        /// </summary>
        void UnrecognizableCommandCallBack();

       

        /// <summary>
        /// Callback gives error if occured during recognizer
        /// </summary>
        /// <param name="error">string</param>
        void DidFinish(string error);



    }
}