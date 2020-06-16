using Android.Content;
using Android.OS;
using Android.Speech;
using VoiceToCommandCore;

namespace VoiceToCommandAndroidLib
{
    public class VoiceToCommandServiceAndroid : VoiceToCommandService
    {
        private bool _isRecording;

        private SpeechRecognizer Recognizer { get; set; }
        private Intent SpeechIntent { get; set; }

        public override bool IsListening()
        {
            return _isRecording;
        }

        public override void StartListening()
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

        public override void StopListening()
        {
            if (_isRecording)
            {
                _isRecording = false;
                Recognizer.StopListening();
            }
        }

        private void RecListener_Ready(object sender, Bundle e) => System.Diagnostics.Debug.WriteLine(nameof(RecListener_Ready));

        private void RecListener_BeginSpeech()
        {
            _isRecording = true;
            System.Diagnostics.Debug.WriteLine(nameof(RecListener_BeginSpeech));
        }

        private void RecListener_EndSpeech()
        {
            _isRecording = false;
            System.Diagnostics.Debug.WriteLine(nameof(RecListener_EndSpeech));
        }

        private void RecListener_Error(object sender, SpeechRecognizerError e)
        {
            _isRecording = false;
            System.Diagnostics.Debug.WriteLine(e.ToString());

        }

        private void RecListener_Recognized(object sender, string recognized)
        {
            _isRecording = false;
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
    }
}
