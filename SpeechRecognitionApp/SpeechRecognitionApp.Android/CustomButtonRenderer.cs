
using Android.Views;
using SpeechRecognitionApp.Droid;
using Xamarin.Forms;
using VoiceToCommand;
using VoiceToCommand.Droid;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace SpeechRecognitionApp.Droid
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var customButton = e.NewElement as CustomButton;

            var thisButton = Control as Android.Widget.Button;
            thisButton.Touch += (object sender, TouchEventArgs args) =>
            {
                if (args.Event.Action == MotionEventActions.Down)
                {
                    customButton.OnPressed();
                }
                else if (args.Event.Action == MotionEventActions.Up)
                {
                    customButton.OnReleased();
                }
            };
        }
    }
}