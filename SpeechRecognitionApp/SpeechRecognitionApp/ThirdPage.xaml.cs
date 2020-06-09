using System;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.Generic;

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
                AvailableCommands();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void AvailableCommands()
        {
            List<string> commandList = voiceToCommandService.GetExecutableCommands();
            var text = string.Empty;
            foreach (String s in commandList)
            {
                text += "\u25C9 \t" + s.ToString() + "\r\n";  // \u25C9- unicode for bullets
            }

            list.Text = text;

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
            
            voiceToCommandService.RegisterCommand("Home", new VoiceCommand(NavigateToFirstPage));
            voiceToCommandService.RegisterCommand("Back", new VoiceCommand(NavigateToPreviousPage));
            voiceToCommandService.RegisterCommand("Red", new VoiceCommand(()=>{ ChangeBackgroundColor("Red"); }));
            voiceToCommandService.RegisterCommand("Green", new VoiceCommand(() => { ChangeBackgroundColor("Green"); }));
            voiceToCommandService.RegisterCommand("Blue", new VoiceCommand(() => { ChangeBackgroundColor("Blue"); }));
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