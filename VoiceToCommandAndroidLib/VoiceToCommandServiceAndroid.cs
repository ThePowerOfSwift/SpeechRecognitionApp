using Android.Content;
using Android.App;
using Android.OS;
using Android.Speech;
using VoiceToCommand.Core;

namespace VoiceToCommand.Droid
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
            if (SpeechRecognizer.IsRecognitionAvailable(Application.Context))
            {
                StartRecording();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Recognition isn't available'");
            }
        }

        private void StartRecording()
        {
            var recListener = GetRecognitionListener();

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);
            Recognizer.SetRecognitionListener(recListener);

            InitialiseSpeechIntent();
            recListener.IsAlreadyRecognized = false;
            Recognizer.StartListening(SpeechIntent);
        }

        private RecognitionListener GetRecognitionListener()
        {
            var recListener = new RecognitionListener();
            recListener.BeginSpeech += RecListenerBeginSpeech;
            recListener.EndSpeech += RecListenerEndSpeech;
            recListener.Error += RecListenerError;
            recListener.Ready += RecListenerReady;
            recListener.Recognized += RecListenerRecognized;
            return recListener;
        }

        private void InitialiseSpeechIntent()
        {
            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, Application.Context.PackageName);
        }

        public override void StopListening()
        {
            if (_isRecording)
            {
                _isRecording = false;
                Recognizer.StopListening();
            }
        }

        private void RecListenerReady(object sender, Bundle e) => System.Diagnostics.Debug.WriteLine(nameof(RecListenerReady));

        private void RecListenerBeginSpeech()
        {
            _isRecording = true;
            System.Diagnostics.Debug.WriteLine(nameof(RecListenerBeginSpeech));
        }

        private void RecListenerEndSpeech()
        {
            _isRecording = false;
            System.Diagnostics.Debug.WriteLine(nameof(RecListenerEndSpeech));
        }

        private void RecListenerError(object sender, SpeechRecognizerError e)
        {
            _isRecording = false;
            System.Diagnostics.Debug.WriteLine(e.ToString());
        }

        private void RecListenerRecognized(object sender, string recognized)
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
