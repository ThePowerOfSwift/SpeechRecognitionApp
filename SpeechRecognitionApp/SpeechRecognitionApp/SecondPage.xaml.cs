using System;
using Xamarin.Forms;
using VoiceToCommandLib;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;


namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondPage : ContentPage
    {
        private IVoiceToCommandService voiceToCommandService;
        private static int NumberOfClicks=0;
        
        public SecondPage()
        {
            InitializeComponent();
            try
            {
                voiceToCommandService = DependencyService.Get<IVoiceCommandServiceFactory>().Create();
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
            var commandList = voiceToCommandService.GetExecutableCommands();
            var text = CommandList.Text + "\n";
            foreach (String command in commandList)
            {
                text += "\u25C9 \t" + command.ToString() + "\r\n";  // \u25C9- unicode for bullets
            }

            CommandList.Text = text;

        }

        private void NavigateToPreviousPage()
        {
            Navigation.PopAsync();
        }

        private void NavigateToThirdPage()
        {
            TextToSpeech.SpeakAsync("You are in Third Page now");
            Navigation.PushAsync(new ThirdPage());
        }

        private void RegisterVoiceCommands()
        {
            voiceToCommandService.RegisterCommand("Confirm", new VoiceCommand(() => { Confirm_Clicked(this, null);})) ;
            voiceToCommandService.RegisterCommand("Back", new VoiceCommand(NavigateToPreviousPage));
            voiceToCommandService.RegisterCommand("Next", new VoiceCommand(NavigateToThirdPage));
        }

        private void SecondButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                voiceToCommandService.StartListening();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        private void OnSliderValueChanged(object sender, EventArgs e)
        {

        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            NumberOfClicks += 1;
            ClickCount.Text = "You Clicked the button " + NumberOfClicks + " times";
        }
    }
}