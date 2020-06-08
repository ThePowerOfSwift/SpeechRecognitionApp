using System;
using Xamarin.Forms;
using VoiceToCommand;
using Xamarin.Forms.Xaml;

namespace SpeechRecognitionApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondPage : ContentPage
    {
        private IVoiceToCommandService speechToTextService;
        
        public SecondPage()
        {
            InitializeComponent();
            try
            {
                speechToTextService = DependencyService.Get<IVoiceToCommandService>();
                RegisterVoiceCommands();
               
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }
        }

        private void NavigateToFirstPage()
        {
            Navigation.PushAsync(new MainPage());
        }

        private void NavigateToThirdPage()
        {
            Navigation.PushAsync(new ThirdPage());
        }

        private void RegisterVoiceCommands()
        {
            speechToTextService.RegisterCommand("Hello", new VoiceCommand(() => { SpeechToTextFinalResultRecieved("Command is 1:Hello"); }));
            speechToTextService.RegisterCommand("Back", new VoiceCommand(NavigateToFirstPage));
            speechToTextService.RegisterCommand("Next", new VoiceCommand(NavigateToThirdPage));
        }

        

        private void SpeechToTextFinalResultRecieved(string args)
        {
            recon.Text = args;
        }

        private void SecondButton_Clicked(object sender, EventArgs e)
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

        private void OnSliderValueChanged(object sender, EventArgs e)
        {

        }

        public class VoiceCommand : IVoiceCommand
        {
            private Action _action;
            public VoiceCommand(Action action)
            {
                _action = action;
            }
            public bool CanExecute()
            {
                return true;
            }

            public void Execute()
            {
                _action.Invoke();
            }
        }
    }
}