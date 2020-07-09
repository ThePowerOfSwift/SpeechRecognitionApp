# NuGet for Recognizing speech and performing action that has been registered


## About Project


>**A NuGet for Mobile application development using Xamarin.Forms, Which will listen to speech and then try to recognize the speech and try to execute the Action that has been registered by the NuGet consuming application.**

## Installation

   - **Platform Independent Library Installation:**
       > **Install Audiology.Library.VoiceToCommandCore(NuGet package Name) in the platform independent project**
       
       
   - **PlatForm Specific Project Installation:**    
       * **Android**
           > **Install Audiology.Library.VoiceToCommandCore and Audiology.Library.VoiceToCommandDroid in Android Project**
           
       * **iOS**
           > **Install  Audiology.Library.VoiceToCommandCore and Audiology.Library.VoiceToCommandiOS in iOS Project**
           
**Start Coding! :computer:**           


## Example

**How To Register Command :**

```

  _voiceToCommandService.RegisterCommand("NameOfTheCommand", new VoiceCommand(ActionToBeExecuted)); 


```


## Features
   
   - **Can pick the command from a sentence**
   - **Can get the nearest matching command (Back->Bat)**
   - **Has Fuzzy String implementation**
   - **Can execute command for words as well as sentences**





