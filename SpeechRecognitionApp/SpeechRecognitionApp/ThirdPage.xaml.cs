using System;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThirdPage : ContentPage
    {

        private IVoiceToCommandService voiceToCommandService;
        public ThirdPage()
        {
            InitializeComponent();
            try
            {
                voiceToCommandService= DependencyService.Get<IVoiceCommandServiceFactory>().Create();
                MyButton.Source = ImageSource.FromResource("SpeechRecognitionApp.Images.mic.png");
                RegisterVoiceCommands();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void NavigateToFirstPage()
        {
            TextToSpeech.SpeakAsync("Navigating To Home Page");
            Navigation.PushAsync(new MainPage());
        }

        private void NavigateToPreviousPage()
        {
            Navigation.PopAsync();
        }

        private void RegisterVoiceCommands()
        {
            //speechToTextService.RegisterCommand("Type", new VoiceCommand(SpeechToTextFinalResultRecieved));
            voiceToCommandService.RegisterCommand("Home", new VoiceCommand(NavigateToFirstPage));
            voiceToCommandService.RegisterCommand("Back", new VoiceCommand(NavigateToPreviousPage));
        }


        private void ThirdButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                voiceToCommandService.StartListening();
            }

            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    MyButton.IsEnabled = false;
            //}

        }
    }
}