using UnityEngine;
using UnityEngine.Events;

public class ButtonPressEventHandler : MonoBehaviour
{
    // Event to be triggered
    public UnityEvent onHandDetected;
    private string textToSpeak;
    public Meta.WitAi.TTS.Utilities.TTSSpeaker tTSSpeaker;
    public ChatGPTChatManager chatManager; // Reference to the ChatGPTSample component
    private AudioSource buttonClickAudioSource;

    // Detects collision with hand
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Hand"))
        {
            buttonClickAudioSource = GetComponent<AudioSource>();
            onHandDetected?.Invoke();
        }
    }

    // Method called by the event to retrieve data and play it
    public void TriggerAction()
    {
        Debug.Log("Button pressed");
        if (!buttonClickAudioSource.isPlaying)
        {
            buttonClickAudioSource.Play();
        }

        // Example message sent to ChatGPT
        // REPLACE WITH QUERY OBTAINED FROM GLOBCENTREVALUESSCRIPTABLEOBJECT
        string userQuery = "2.0 6.25 4.95 8.0 10.5";

        // Send message to ChatGPT and handle response
        chatManager.SendMessageToChatGPT(userQuery, (response) =>
        {
            textToSpeak = response;
            PlayTTS();
        });
    }

    public void PlayTTS()
    {
        Debug.Log("Starting TTS with text: " + textToSpeak);
        tTSSpeaker.Speak(textToSpeak);
    }

    // Other utility methods for testing/debugging
    public void AudioStart()
    {
        Debug.Log("AudioStart() Triggered");
    }

    public void TextStart()
    {
        Debug.Log("TextStart() Triggered");
    }

    public void SpeakStart()
    {
        Debug.Log("SpeakStart() Triggered");
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ButtonPressManager : MonoBehaviour
//{
//    // Start is called before the first frame update
//    // Event to be triggered
//    public UnityEngine.Events.UnityEvent onHandDetected;
//    public string textToSpeak;
//    public Meta.WitAi.TTS.Utilities.TTSSpeaker tTSSpeaker;

//    // Detects collision with hand
//    private void OnTriggerEnter(Collider other)
//    {
//        // Adjust this condition based on your specific hand setup
//        if (other.gameObject.name.Contains("Hand"))
//        {
//            Debug.Log("Hand detected");
//            onHandDetected?.Invoke();
//        }
//    }

//    public void TriggerAction()
//    {
//        Debug.Log("Button pressed");
//    }

//    public void TTSTest()
//    {
//        //textToSpeak = "This is a sample for the text to speech system";
//        tTSSpeaker.Speak(textToSpeak);
//    }

//    public void AudioStart()
//    {
//        Debug.Log("AudioStart() Triggered");
//    }

//    public void TextStart()
//    {
//        Debug.Log("TextStart() Triggered");
//    }

//    public void SpeakStart()
//    {
//        Debug.Log("SpeakStart() Triggered");
//    }


//}