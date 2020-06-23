using System;
using Android.OS;
using Android.Speech;
using Android.Runtime;


namespace VoiceToCommand.Droid
{
    public class RecognitionListener : Java.Lang.Object, IRecognitionListener
    {
        public event EventHandler<Bundle> Ready;
        public event Action BeginSpeech;
        public event Action EndSpeech;
        public event EventHandler<string> Recognized;
        public event EventHandler<SpeechRecognizerError> Error;
        public bool IsAlreadyRecognized { get; set; }

        public RecognitionListener() : base() { }

        public void OnReadyForSpeech(Bundle @params) => Ready?.Invoke(this, @params);

        public void OnBeginningOfSpeech() => BeginSpeech?.Invoke();

        public void OnEndOfSpeech() => EndSpeech?.Invoke();

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (!IsAlreadyRecognized)
            {
                if (matches != null && matches.Count > 0)
                {
                    IsAlreadyRecognized = true;
                    Recognized?.Invoke(this, matches[0]);
                }
            }
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error) => Error?.Invoke(this, error);

        public void OnBufferReceived(byte[] buffer) { }

        public void OnEvent(int eventType, Bundle @params) { }

        public void OnPartialResults(Bundle partialResults) { }

        public void OnRmsChanged(float rmsdB) { }
    }
}