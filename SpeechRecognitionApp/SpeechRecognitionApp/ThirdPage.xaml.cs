using System;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Forms.Xaml;

namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThirdPage : ContentPage
    {

        private IVoiceToCommandService speechToTextService;
        public ThirdPage()
        {
            InitializeComponent();
            try
            {
                speechToTextService = DependencyService.Get<IVoiceToCommandService>();
                RegisterVoiceCommands();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void NavigateToFirstPage()
        {
            Navigation.PushAsync(new MainPage());
        }

        private void NavigateToPreviousPage()
        {
            Navigation.PopAsync();
        }

        private void RegisterVoiceCommands()
        {
            //speechToTextService.RegisterCommand("Type", new VoiceCommand(SpeechToTextFinalResultRecieved));
            speechToTextService.RegisterCommand("Home", new VoiceCommand(NavigateToFirstPage));
            speechToTextService.RegisterCommand("Back", new VoiceCommand(NavigateToPreviousPage));
        }


        private void ThirdButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                speechToTextService.StartListening();
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