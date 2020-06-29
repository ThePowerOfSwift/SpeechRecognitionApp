using Android.Content;
using Android.App;
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
            _recognitionListener.BeginSpeech += OnBeginSpeechHandler;
            _recognitionListener.EndSpeech += OnEndOfSpeechHandler;
            _recognitionListener.Error += OnErrorHandler;
            _recognitionListener.Recognized += OnRecognizedHandler;
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
            _recognitionListener.BeginSpeech -= OnBeginSpeechHandler;
            _recognitionListener.EndSpeech -= OnEndOfSpeechHandler;
            _recognitionListener.Error -= OnErrorHandler;
            _recognitionListener.Recognized -= OnRecognizedHandler;
        }

        private void OnBeginSpeechHandler()
        {
            _isRecording = true;
        }

        private void OnEndOfSpeechHandler()
        {
            _isRecording = false;
        }

        private void OnErrorHandler(object sender, SpeechRecognizerError e)
        {
            _isRecording = false;
            Debug.WriteLine(e.ToString());
        }

        private void OnRecognizedHandler(object sender, string recognized)
        {
            _isRecording = false;
            recognized = recognized.ToLower();
            Debug.WriteLine("Recognized: " + recognized);
            ExecuteRecognizedCommand(recognized);
        }
    }
}

    