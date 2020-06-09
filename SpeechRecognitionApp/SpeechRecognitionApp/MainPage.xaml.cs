using System;
using System.ComponentModel;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace SpeechRecognitionApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private IVoiceToCommandService voiceToCommandService;
        private bool isPermissionGranted;
        
        public MainPage()
        {
            InitializeComponent();
            
            try
            {
                voiceToCommandService = DependencyService.Get<IVoiceCommandServiceFactory>().Create();
                RegisterVoiceCommands();
                MyButton.Source = ImageSource.FromResource("SpeechRecognitionApp.Images.mic.png");
                CheckPermissionStatus();
                
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

        }

        private void NavigateToSecondPage()
        {
            TextToSpeech.SpeakAsync("Navigating To Second Page");
            Navigation.PushAsync(new SecondPage());
        }

        private void NavigateToThirdPage()
        {
            TextToSpeech.SpeakAsync("Navigating To Third Page");
            Navigation.PushAsync(new ThirdPage());
        }

        private void RegisterVoiceCommands()
        {
           
            voiceToCommandService.RegisterCommand("Next", new VoiceCommand(NavigateToSecondPage));
            voiceToCommandService.RegisterCommand("Third", new VoiceCommand(NavigateToThirdPage));
        }

        private async void CheckPermissionStatus()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (PermissionStatus.Granted == permissionStatus)
            {
                isPermissionGranted = true;
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
                    isPermissionGranted = true;
                }

            }
        }

        private void SpeakInitialInstruction()
        {
            TextToSpeech.SpeakAsync("To Speak Press the microphone Image.");
        }

        private void SpeechToTextFinalResultRecieved(string args)
        {
            recon.Text = args;
        }


        private void MyButton_Pressed(object sender, EventArgs e)
        {
            try
            {
                if (isPermissionGranted)
                {
                    //MyButton.ImageSource = ImageSource.FromResource("SpeechRecognitionApp.Images.mic.png");
                    voiceToCommandService.StartListening();
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

            if (Device.RuntimePlatform == Device.iOS)
            {
                MyButton.IsEnabled = false;
            }

        }

       
    }
}
