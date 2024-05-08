using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Event to be triggered
    public UnityEngine.Events.UnityEvent onHandDetected;
    public string textToSpeak;
    public Meta.WitAi.TTS.Utilities.TTSSpeaker tTSSpeaker;

    // Detects collision with hand
    private void OnTriggerEnter(Collider other)
    {
        // Adjust this condition based on your specific hand setup
        if (other.gameObject.name.Contains("Hand")) // or "Touch", depending on your prefab naming
        {
            Debug.Log("Hand detected");
            onHandDetected?.Invoke(); // Triggers the event
        }
    }

    public void TriggerAction()
    {
        Debug.Log("Button pressed");
    }

    public void TTSTest()
    {
        //textToSpeak = "This is a sample for the text to speech system";
        tTSSpeaker.Speak(textToSpeak);
    }

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