using System;
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

        public VoiceToCommandServiceiOS()
        {
        }

        public override void StartListening()
        {
            if (_audioEngine.Running)
            {
                StopRecording();
            }
            StartRecording();
        }

        private void StopRecording(AVAudioSession aVAudioSession = null)
        {
            if (_audioEngine.Running)
            {
                _audioEngine.Stop();
                _audioEngine.InputNode.RemoveTapOnBus(0);
                _recognitionTask?.Cancel();
                _recognitionRequest.EndAudio();
                _recognitionRequest = null;
                _recognitionTask = null;
            }
        }

        private void StartRecording()
        {
            _timer = NSTimer.CreateRepeatingScheduledTimer(2, delegate
            {
                DidFinishTalk();
            });

            _recognitionTask?.Cancel();
            _recognitionTask = null;

            NSError nsError;
            var audioSession = GetAudioSessionComponent();

            var inputNode = _audioEngine.InputNode;
            if (inputNode == null)
            {
                throw new Exception("Input Node is null");
            }

            var recordingFormat = inputNode.GetBusOutputFormat(0);
            inputNode.InstallTapOnBus(0, 1024, recordingFormat, (buffer, when) =>
            {
                _recognitionRequest?.Append(buffer);
            });

            _audioEngine.Prepare();
            _audioEngine.StartAndReturnError(out nsError);

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
            _recognitionTask = _speechRecognizer.GetRecognitionTask(_recognitionRequest, (result, error) =>
                {
                    var isFinal = false;
                    if (result != null && _recognitionRequest != null)
                    {
                        _recognizedString = result.BestTranscription.FormattedString.ToLower();
                        _timer.Invalidate();
                        _timer = null;

                        System.Diagnostics.Debug.WriteLine(_recognizedString);
                        ExecuteRecognizedCommand();

                        isFinal = true;
                        StopRecording(audioSession);
                    }

                    if (error != null || isFinal)
                    {
                        StopRecording(audioSession);
                    }
                });
        }

        private void ExecuteRecognizedCommand()
        {
            if (AllRegisteredCommands.ContainsKey(_recognizedString))
            {
                var command = AllRegisteredCommands[_recognizedString];
                if (command.CanExecute())
                {
                    command.Execute();
                }
            }
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