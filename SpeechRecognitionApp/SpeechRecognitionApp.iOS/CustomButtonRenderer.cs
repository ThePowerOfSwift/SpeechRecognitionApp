
using Xamarin.Forms;
using SpeechRecognitionApp.iOS;
using VoiceToCommand;
using UIKit;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace SpeechRecognitionApp.iOS
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var customButton = e.NewElement as CustomButton;

            UIButton thisButton = Control as UIButton;
            thisButton.TouchDown += delegate
            { 
                    customButton.OnPressed();
               
            };

            thisButton.TouchUpInside += delegate
            {
                customButton.OnReleased();
            };
        }
    }
}