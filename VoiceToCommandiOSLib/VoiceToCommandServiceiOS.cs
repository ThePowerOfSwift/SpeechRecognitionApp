using System;
using System.Collections.Generic;
using System.Linq;
using AVFoundation;
using Foundation;
using Speech;
using UIKit;
using VoiceToCommandCore;

namespace VoiceToCommandApp.iOS
{
    public class VoiceToCommandServiceiOS : VoiceToCommandService
    {
        //Need to give Permission
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
                StopRecordingAndRecognition();

            }
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            _timer = NSTimer.CreateRepeatingScheduledTimer(2, delegate
            {
                DidFinishTalk();
            });

            _recognitionTask?.Cancel();
            _recognitionTask = null;
            
            var audioSession = AVAudioSession.SharedInstance();
            NSError nsError;
            nsError = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetMode(AVAudioSession.ModeDefault, out nsError);
            nsError = audioSession.SetActive(true, AVAudioSessionSetActiveOptions.NotifyOthersOnDeactivation);
            audioSession.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out nsError);
            _recognitionRequest = new SFSpeechAudioBufferRecognitionRequest();

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
            

            _recognitionTask = _speechRecognizer.GetRecognitionTask(_recognitionRequest, (result, error) =>
            {
                var isFinal = false;
                if (result != null && _recognitionRequest != null)
                {
                    _recognizedString = result.BestTranscription.FormattedString.ToLower();
                    _timer.Invalidate();
                    _timer = null;

                    System.Diagnostics.Debug.WriteLine(_recognizedString);
                    if (AllRegisteredCommands.ContainsKey(_recognizedString))
                    {
                        var command = AllRegisteredCommands[_recognizedString];
                        if (command.CanExecute())
                        {
                            command.Execute();
                        }
                    }
                  
                    isFinal = true;
                    StopRecordingAndRecognition(audioSession);
                }   
                if (error != null || isFinal)
                {
                    StopRecordingAndRecognition(audioSession);
                }
        });
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
                StopRecordingAndRecognition();
            }
        }

        private void StopRecordingAndRecognition(AVAudioSession aVAudioSession = null)
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
            StopRecordingAndRecognition();
        }

        public override bool IsListening()
        {
            throw new NotImplementedException();
        }
    }
}