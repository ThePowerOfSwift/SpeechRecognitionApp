# Voice To Command 

> **Nuget for Recognizing speech to invoke User interface actions/Custom Business actions**


## About Project


>**A NuGet for Xamarin.Forms Mobile Application, which recognizes speech and invokes actions which are registered by the App**



>**Library provides support for .NET Framework 4.5 , ASP.NET Core 1.0 , MonoAndroid v9.0 , iOS**


## Demo

![](https://github.com/SpeechRecognitionDemo/SpeechRecognitionApp/blob/master/WorkingApp.gif)


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


## Prerequistes

 

**Both Android and iOS needs permission to be added to application in order to use Speech recognition**
 
 - **Android** : requires 1 permission i.e **Microphone Permission** , add below given code in Manifest file
    ```xml
    
         <uses-permission android:name="android.permission.RECORD_AUDIO" />
         
    
    ```
    - **Request User to grant Permission at runtime for android 6.0+**
    
    
      
 - **iOS** : requires 2 permission i.e **Microphone Permission** and **Speech Recognition Permission** , add below given key in info.plist in app
     ```
     
                 <key>NSSpeechRecognitionUsageDescription</key>
     
                 <string>Allows you recognize speech</string>
     
                 <key>NSMicrophoneUsageDescription</key>
         
                 <string>Allows you to record audio</string>
 
     ```
     
     
     
## Enabling Offline Speech Recognition in Android 

* **For offline recognition you need to download the offline language files . It can done in  following steps in your system settings under Language & Input:**

    - **Find "Google Voice Typing", make sure it's enabled**
    
    - **Tap 'Offline Speech Recognition', and install / download all languages that you would like to use**
    
    - **If preferred language does not have an offline voice available under Google Voice Typing, you must choose a language that does and make it your default language.**
    
    - **Go back to 'Language & Input', and select the same languages again. Then select your primary language. Note that this must be one of the 'Offline Languages'you downloaded and installed first.**
    
     - **A common problem is that the default language is not available as an offline voice.For example en-NZ is not available offline so you must use en-US or en-GB as your default language (but you can keep en-NZ as a secondary language)**


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
  

## Limitations

   * **Android**
      - **Works well in Online and offline mode, provided we enable google voice typing for Offline mode.** [Refer this section for how to Enable Offline speech recognition](https://github.com/SpeechRecognitionDemo/SpeechRecognitionApp#enabling-offline-speech-recognition-in-android)
      
   * **iOS**
      - **Our NuGet supports only online speech recognition.**
      - **Individual iOS devices have a limited number of recognitions that can be performed per day.**
      - **iOS speech recognizer can only recognize maximum for 1 min , As of now timer is set for 50 Seconds**

  





