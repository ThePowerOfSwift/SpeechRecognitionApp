﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Autofac;
using CommonServiceLocator;
using Autofac.Extras.CommonServiceLocator;
using Plugin.Permissions;
using VoiceToCommand.Droid;
using VoiceToCommand.Core;

namespace SpeechRecognitionApp.Droid
{
    [Activity(Label = "SpeechRecognitionApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            var builder = new ContainerBuilder();
            builder.RegisterType<VoiceToCommandServiceAndroid>().As<IVoiceToCommandService>();
            var container = builder.Build();


            var serviceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}