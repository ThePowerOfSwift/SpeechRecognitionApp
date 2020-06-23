using Android.Content;
using Android.App;
using Android.OS;
using Android.Speech;
using VoiceToCommand.Core;
using Debug=System.Diagnostics.Debug;

namespace VoiceToCommand.Droid
{
    public class VoiceToCommandServiceAndroid : VoiceToCommandService
    {
        private bool _isRecording;
        private RecognitionListener _recognitionListener;

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
                Debug.WriteLine("Recognition isn't available'");
            }
        }

        private void StartRecording()
        {
            _recognitionListener = GetRecognitionListener();

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);
            Recognizer.SetRecognitionListener(_recognitionListener);

            InitializeSpeechIntent();
            _recognitionListener.IsAlreadyRecognized = false;
            Recognizer.StartListening(SpeechIntent);
        }

        private RecognitionListener GetRecognitionListener()
        {
            _recognitionListener = new RecognitionListener();
            RegisterRecognitionListenerHandlers();
            return _recognitionListener;
        }

        private void RegisterRecognitionListenerHandlers()
        {
            _recognitionListener.BeginSpeech += ListenerOnBeginSpeechHandler;
            _recognitionListener.EndSpeech += ListenerOnEndOfSpeechHandler;
            _recognitionListener.Error += ListenerOnErrorHandler;
            _recognitionListener.Ready += ListenerOnReadyForSpeechHandler;
            _recognitionListener.Recognized += ListenerOnRecognizedHandler;
        }

        private void InitializeSpeechIntent()
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
            DeRegisterRecognitionListenerHandlers();
        }

        private void DeRegisterRecognitionListenerHandlers()
        {
            _recognitionListener.BeginSpeech -= ListenerOnBeginSpeechHandler;
            _recognitionListener.EndSpeech -= ListenerOnEndOfSpeechHandler;
            _recognitionListener.Error -= ListenerOnErrorHandler;
            _recognitionListener.Ready -= ListenerOnReadyForSpeechHandler;
            _recognitionListener.Recognized -= ListenerOnRecognizedHandler;
        }

        private void ListenerOnReadyForSpeechHandler(object sender, Bundle e) => Debug.WriteLine(nameof(ListenerOnReadyForSpeechHandler));

        private void ListenerOnBeginSpeechHandler()
        {
            _isRecording = true;
        }

        private void ListenerOnEndOfSpeechHandler()
        {
            _isRecording = false;
        }

        private void ListenerOnErrorHandler(object sender, SpeechRecognizerError e)
        {
            _isRecording = false;
            Debug.WriteLine(e.ToString());
        }

        private void ListenerOnRecognizedHandler(object sender, string recognized)
        {
            _isRecording = false;
            recognized = recognized.ToLower();
            Debug.WriteLine("Recognized: "+recognized);
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
