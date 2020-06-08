using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Foundation;
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
            //base.OnElementChanged(e);

            //var customButton = e.NewElement as CustomButton;

            //var thisButton = Control as Button;
            //thisButton.Touch += (object sender, UITouchEventArgs args) =>
            //{
            //    if (args.Event.Action == MotionEventActions.Down)
            //    {
            //        customButton.OnPressed();
            //    }
            //    else if (args.Event.Action == MotionEventActions.Up)
            //    {
            //        customButton.OnReleased();
            //    }
            //};
        }
    }
}