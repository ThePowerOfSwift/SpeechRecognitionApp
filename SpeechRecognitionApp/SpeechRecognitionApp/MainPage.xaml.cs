using System;
using System.ComponentModel;
using Xamarin.Forms;
using VoiceToCommand.Core;
using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using CommonServiceLocator;

namespace SpeechRecognitionApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IVoiceToCommandService _voiceToCommandService;
        private bool _isPermissionGranted;
        
        public MainPage()
        {
            InitializeComponent();
            
            try
            {
                _voiceToCommandService = ServiceLocator.Current.GetInstance<IVoiceToCommandService>();
                RegisterVoiceCommands();
                MyButton.Source = ImageSource.FromResource("SpeechRecognitionApp.Images.mic.png");
                CheckPermissionStatus();
                AvailableCommands();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

        }

        private void CloseApplication()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        private void NavigateToSecondPage()
        {
            TextToSpeech.SpeakAsync("You are in Second Page now!");
            Navigation.PushAsync(new SecondPage());
        }

        private void NavigateToThirdPage()
        {
            TextToSpeech.SpeakAsync("You are in Third Page now");
            Navigation.PushAsync(new ThirdPage());
        }

        private void RegisterVoiceCommands()
        {
            _voiceToCommandService.RegisterCommand("Quit", new VoiceCommand(CloseApplication));
            _voiceToCommandService.RegisterCommand("Next", new VoiceCommand(NavigateToSecondPage));
            _voiceToCommandService.RegisterCommand("Third", new VoiceCommand(NavigateToThirdPage));
        }

        private void AvailableCommands()
        {
            var commandList = _voiceToCommandService.GetExecutableCommands();
            var text = CommandList.Text + "\n";
            foreach (String command in commandList)
            {
                text += "\u25C9 \t" + command.ToString() + "\r\n";  // \u25C9- unicode for bullets
            }

            CommandList.Text = text;
   
        }


        private async void CheckPermissionStatus()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (PermissionStatus.Granted == permissionStatus)
            {
                _isPermissionGranted = true;
            }
            else
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                {
                    await DisplayAlert("Need Mic", "Need Microphone Access", "OK");
                }

                var status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                if (status == PermissionStatus.Granted)
                {
                    _isPermissionGranted = true;
                    MyButton_Pressed(this,null);
                }

            }
        }

        private void MyButton_Pressed(object sender, EventArgs e)
        {
            try
            {
                if (_isPermissionGranted)
                {
                    _voiceToCommandService.StartListening();
                }
                else
                {
                    CheckPermissionStatus();
                }
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

        }
    }
}
