using System;
using System.Diagnostics;
using System.Threading;
using AVFoundation;
using Foundation;
using Speech;
using VoiceToCommand.Core;

namespace VoiceToCommand.iOS
{
    public class VoiceToCommandServiceiOS : VoiceToCommandService
    {
        private AVAudioEngine _audioEngine = new AVAudioEngine();
        private SFSpeechRecognizer _speechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest _recognitionRequest;
        private SFSpeechRecognitionTask _recognitionTask;
        private string _recognizedString;
        private NSTimer _timer;
        public event EventHandler<string> FinishAction;

        public VoiceToCommandServiceiOS()
        {
        }

        public override void StartListening()
        {
            Debug.WriteLine("Start listening");
            if (_audioEngine.Running)
            {
                StopRecording();
                Debug.WriteLine("Stop recording");
            }
            StartRecording();
        }

        private void StopRecording(AVAudioSession aVAudioSession = null)
        {
            if (_audioEngine.Running)
            {
                Console.WriteLine("called stop recording with internal closing");
                _audioEngine.Stop();
                _audioEngine.InputNode.RemoveTapOnBus(0);
                _recognitionTask.Finish();
                _recognitionRequest.EndAudio();
                _recognitionRequest = null;
                _recognitionTask = null;
            }
        }

        private void StartRecording()
        {
            Debug.WriteLine("Start recording");

            _recognitionTask?.Finish();
            _recognitionTask = null;

            NSError nsError;
            var audioSession = GetAudioSessionComponent();

            var inputNode = _audioEngine.InputNode;
            if (inputNode == null)
            {
                DidFinish("Input Node is null");
                throw new Exception("Input Node is null");
            }

            var recordingFormat = inputNode.GetBusOutputFormat(0);

            inputNode.InstallTapOnBus(0, 1024, recordingFormat, (buffer, when) =>
            {
                _recognitionRequest?.Append(buffer);
            });

            _audioEngine.Prepare();
            _audioEngine.StartAndReturnError(out nsError);
            if (nsError != null)
            {
                DidFinish("Input Node is null");
                return;
            }

            _timer = NSTimer.CreateRepeatingScheduledTimer(50, delegate
            {
                DidFinishTalk();
            });

            PerformRecognitionTask(audioSession);
        }

        private void DidFinishTalk()
        {
            if (_timer != null)
            {
                _timer.Invalidate();
                _timer = null;
            }
            if (_audioEngine.Running)
            {
                StopRecording();
            }
            DidFinish("null");
        }

        private AVAudioSession GetAudioSessionComponent()
        {
            var audioSession = AVAudioSession.SharedInstance();
            NSError nsError;
            nsError = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetMode(AVAudioSession.ModeDefault, out nsError);
            nsError = audioSession.SetActive(true, AVAudioSessionSetActiveOptions.NotifyOthersOnDeactivation);
            audioSession.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out nsError);
            _recognitionRequest = new SFSpeechAudioBufferRecognitionRequest();
            return audioSession;
        }

        private void PerformRecognitionTask(AVAudioSession audioSession)
        {
            Debug.WriteLine("recognition part");

            _recognitionTask = _speechRecognizer.GetRecognitionTask(_recognitionRequest, (result, error) =>
            {
                var isFinal = false;
                if (result != null && _recognitionRequest != null)
                {
                    Console.WriteLine("_RecognizedString" + _recognizedString);
                    _recognizedString = result.BestTranscription.FormattedString.ToLower();

                    Debug.WriteLine(_recognizedString);
                    ExecuteRecognizedCommand(_recognizedString);
                    isFinal = true;
                }

                if (error != null || isFinal)
                {
                    DidFinishTalk();
                    StopRecording(audioSession);
                }

                if (error != null)
                {
                    DidFinishTalk();
                    DidFinish(error.ToString());
                    Console.WriteLine("Error occured" + error + error.Code + error.Description);
                }
            });
        }

        public override bool IsListening()
        {
            return _audioEngine.Running;
        }

        public override void StopListening()
        {
            StopRecording();
        }
    }
}