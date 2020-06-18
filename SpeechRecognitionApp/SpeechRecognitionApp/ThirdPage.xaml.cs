using System;
using Xamarin.Forms;
using VoiceToCommand.Core;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using CommonServiceLocator;

namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThirdPage : ContentPage
    {

        private readonly IVoiceToCommandService _voiceToCommandService;
        public ThirdPage()
        {
            InitializeComponent();
            try
            {
                _voiceToCommandService = ServiceLocator.Current.GetInstance<IVoiceToCommandService>();
                MyButton.Source = ImageSource.FromResource("SpeechRecognitionApp.Images.mic.png");
                RegisterVoiceCommands();
                AvailableCommands();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
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

        private void NavigateToFirstPage()
        {
            TextToSpeech.SpeakAsync("You are in Home Page now");
            Navigation.PushAsync(new MainPage());
        }

        private void NavigateToPreviousPage()
        {
            Navigation.PopAsync();
        }

        private void NavigateToSecondPage()
        {
            Navigation.PushAsync(new SecondPage());
        }

        private void ChangeBackgroundColor(string backgroundColor)
        {
            if(backgroundColor == "Red" )
            {
                BackgroundColor = Color.Red;
            }
            else if(backgroundColor == "Green")
            {
                BackgroundColor = Color.Green;
            }

            else if(backgroundColor == "Blue")
            {
                BackgroundColor = Color.Blue;
            }
            

           
        }

        private void RegisterVoiceCommands()
        {
            
            _voiceToCommandService.RegisterCommand("Home", new VoiceCommand(NavigateToFirstPage));
            _voiceToCommandService.RegisterCommand("Back", new VoiceCommand(NavigateToPreviousPage));
            _voiceToCommandService.RegisterCommand("Red", new VoiceCommand(()=>{ ChangeBackgroundColor("Red"); }));
            _voiceToCommandService.RegisterCommand("Green", new VoiceCommand(() => { ChangeBackgroundColor("Green"); }));
            _voiceToCommandService.RegisterCommand("Go To Second Page", new VoiceCommand(NavigateToSecondPage));
            _voiceToCommandService.RegisterCommand("Blue", new VoiceCommand(() => { ChangeBackgroundColor("Blue"); }));
        }


        private void ThirdButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                _voiceToCommandService.StartListening();
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