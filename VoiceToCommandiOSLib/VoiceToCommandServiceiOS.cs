﻿using System;
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
        private bool _isAuthorized;
        private NSTimer _timer;
        

        public VoiceToCommandServiceiOS()
        {
            AskForSpeechPermission();
        }

        private void AskForSpeechPermission()
        {
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) =>
            {
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        _isAuthorized = true;
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                        break;
                }
            });
        }

        public override void StartListening()
        {
            if (_audioEngine.Running)
            {
                StopRecording();
            }
            StartRecording();
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
                throw new Exception();
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
                    ExecuteCommand();

                    isFinal = true;
                    StopRecording(audioSession);
                }

                if (error != null || isFinal)
                {
                    StopRecording(audioSession);
                }
            });
        }

        private void ExecuteCommand()
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

        public override void StopListening()
        {
            StopRecording();
        }

        public override bool IsListening()
        {
            return _audioEngine.Running;
        }
    }
}