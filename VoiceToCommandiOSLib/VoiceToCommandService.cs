using System;
using System.Collections.Generic;
using System.Linq;
using AVFoundation;
using Foundation;
using Speech;
using UIKit;
using VoiceToCommandLib;

namespace VoiceToCommandApp.iOS
{
    public class VoiceToCommandService : IVoiceToCommandService
    {
        //Need to give Permission
        private AVAudioEngine _audioEngine = new AVAudioEngine();
        private SFSpeechRecognizer _speechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest _recognitionRequest;
        private SFSpeechRecognitionTask _recognitionTask;
        private string _recognizedString;
        private bool _isAuthorized;
        private NSTimer _timer;
        private IDictionary<string, IVoiceCommand> AllRegisteredCommands;

        public VoiceToCommandService()
        {
            AllRegisteredCommands = new Dictionary<string, IVoiceCommand>();
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

        public void StartListening()
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
            UIActivityIndicatorView activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Large);
            
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
                if (result != null)
                {
                    activityIndicatorView.StartAnimating();
                    
                    _recognizedString = result.BestTranscription.FormattedString.ToLower();
                    //MessagingCenter.Send<ISpeechToTextService, string>(this, "STT", _recognizedString);
                    _timer.Invalidate();
                    _timer = null;
                    _timer = NSTimer.CreateRepeatingScheduledTimer(2, delegate
                    {
                        DidFinishTalk();
                    });


                    System.Diagnostics.Debug.WriteLine(_recognizedString);
                    if (AllRegisteredCommands.ContainsKey(_recognizedString))
                    {
                        var command = AllRegisteredCommands[_recognizedString];
                        if (command.CanExecute())
                        {
                            command.Execute();
                        }
                    }
                    StopRecordingAndRecognition(audioSession);
                    activityIndicatorView.StopAnimating();

                }
                if (error != null || isFinal)
                {
                    //MessagingCenter.Send<ISpeechToTextService>(this, "Final");
                    StopRecordingAndRecognition(audioSession);
                }
            });
        }

        private void DidFinishTalk()
        {
           // MessagingCenter.Send<ISpeechToTextService>(this, "Final");
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

        public void StopListening()
        {
            StopRecordingAndRecognition();
        }

        public bool IsListening()
        {
            throw new NotImplementedException();
        }

        public void RegisterCommand(string commandString, IVoiceCommand commandToBeExecuted)
        {
            AllRegisteredCommands.Add(commandString.ToLower(), commandToBeExecuted);
        }

        public void DeregisterCommand(string commandString)
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

        public void DeRegisterCommand(string commandString)
        {
            AllRegisteredCommands.Remove(commandString);
        }

        public void RegisterListeningCompletedCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }

        public void DeRegisterListeningCompletedCallBack(Action callBack)
        {
            throw new NotImplementedException();
        }

        public void RegisterUnrecognizableCommandCallBack(Action callBack)
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
}