using System;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Forms.Xaml;

namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondPage : ContentPage
    {
        private IVoiceToCommandService voiceToCommandService;
        
        public SecondPage()
        {
            InitializeComponent();
            try
            {
                voiceToCommandService = DependencyService.Get<IVoiceCommandServiceFactory>().Create();
                RegisterVoiceCommands();
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void NavigateToPreviousPage()
        {
            Navigation.PopAsync();
        }

        private void NavigateToThirdPage()
        {
            Navigation.PushAsync(new ThirdPage());
        }

        private void RegisterVoiceCommands()
        {
            //speechToTextService.RegisterCommand("Hello", new VoiceCommand(() => { SpeechToTextFinalResultRecieved("Command is 1:Hello"); }));
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
    }
}