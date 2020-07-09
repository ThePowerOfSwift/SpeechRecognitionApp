# Voice To Command 

> **NuGet for Recognizing speech and performing action that has been registered**


## About Project


>**A NuGet for Mobile application development using Xamarin.Forms, Which will listen to speech and try to recognize and execute the Action that has been registered by the application consuming NuGet.**



>**Library provides support for .NET Framework 4.5 , ASP.NET Core 1.0 , MonoAndroid v9.0 , iOS**


## Installation

**The quickest way to get the latest release of VoiceToCommand Library is to add it to your project using NuGet**

   - **Platform Independent Library Installation:**
       > **Install Audiology.Library.VoiceToCommandCore(NuGet package Name) in the platform independent project**
       
       
   - **PlatForm Specific Project Installation:**    
       * **Android**
       > **Install Audiology.Library.VoiceToCommandCore and Audiology.Library.VoiceToCommandDroid in Android Project**
           
       * **iOS**
       > **Install  Audiology.Library.VoiceToCommandCore and Audiology.Library.VoiceToCommandiOS in iOS Project**
           
**Happy Coding! :computer:**           



## Example


**How To Register Command :**


```c#



   _voiceToCommandService.RegisterCommand("NameOfTheCommand", new VoiceCommand(ActionToBeExecuted));



```


## Features
   
   - **Can pick the command from a sentence**
   - **Can get the nearest matching command (For example : Back -> Bat)**
   - **Has Fuzzy String implementation**
   - **Can execute command for words as well as sentences**
   - **Easy to Register Commands and Use**





