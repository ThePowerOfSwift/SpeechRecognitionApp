﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Speech;
using VoiceToCommandAndroidLib;
using VoiceToCommandLib;
using Xamarin.Forms;

[assembly: Dependency(typeof(VoiceToCommandServiceFactory))]
namespace VoiceToCommandAndroidLib
{
    class VoiceToCommandService : IVoiceToCommandService
    {
        private bool isRecording;
        private IDictionary<string, IVoiceCommand> AllRegisteredCommands;

        public VoiceToCommandService()
        {
            AllRegisteredCommands = new Dictionary<string, IVoiceCommand>();
        }

        private SpeechRecognizer Recognizer { get; set; }
        private Intent SpeechIntent { get; set; }

        public bool IsListening()
        {
            return isRecording;
        }

        public void StartListening()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            var recListener = new RecognitionListener();
            recListener.BeginSpeech += RecListener_BeginSpeech;
            recListener.EndSpeech += RecListener_EndSpeech;
            recListener.Error += RecListener_Error;
            recListener.Ready += RecListener_Ready;
            recListener.Recognized += RecListener_Recognized;

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(Android.App.Application.Context);
            Recognizer.SetRecognitionListener(recListener);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, Android.App.Application.Context.PackageName);
            recListener.IsRecognizedAlready = false;
            Recognizer.StartListening(SpeechIntent);
        }

        public void StopListening()
        {
            if (isRecording)
            {
                isRecording = false;
                Recognizer.StopListening();
            }
        }

        public void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted)
        {
            AllRegisteredCommands.Add(commandString.ToLower(), commandToBeExecuted);
        }

        public void DeRegisterCommand(string commandString)
        {
            AllRegisteredCommands.Remove(commandString);
        }


        public List<string> GetAvailableCommands()
        {
            return AllRegisteredCommands.Keys.ToList();
        }

        public List<string> GetExecutableCommands()
        {
            return (AllRegisteredCommands.Where(item => item.Value.CanExecute()).Select(item => item.Key)).ToList();
        }

       
        private void RecListener_Ready(object sender, Bundle e) => System.Diagnostics.Debug.WriteLine(nameof(RecListener_Ready));

        private void RecListener_BeginSpeech()
        {
            isRecording = true;
            System.Diagnostics.Debug.WriteLine(nameof(RecListener_BeginSpeech));
        }

        private void RecListener_EndSpeech()
        {
            isRecording = false;
            System.Diagnostics.Debug.WriteLine(nameof(RecListener_EndSpeech));
        }

        private void RecListener_Error(object sender, SpeechRecognizerError e)
        {
            isRecording = false;
            System.Diagnostics.Debug.WriteLine(e.ToString());

        }

        private void RecListener_Recognized(object sender, string recognized)
        {
            isRecording = false;
            recognized = recognized.ToLower();
            System.Diagnostics.Debug.WriteLine(recognized);
            if (AllRegisteredCommands.ContainsKey(recognized))
            {
                var command = AllRegisteredCommands[recognized];
                if (command.CanExecute())
                {
                    command.Execute();
                }
            }

        }

        public void RegisterListeningCompletedCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }


        public void RegisterUnrecognizableCommandCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }



        public void DeRegisterListeningCompletedCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }

        public void DeRegisterUnrecognizableCommandCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }

        public void RegisterUnExecutableCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }

        public void DeRegisterUnExecutableCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }
    }

    public class VoiceToCommandServiceFactory : IVoiceCommandServiceFactory
    {
        public IVoiceToCommandService Create()
        {
            return new VoiceToCommandService();
        }
    }
}
