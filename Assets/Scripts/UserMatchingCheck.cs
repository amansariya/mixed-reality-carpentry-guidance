using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMatchingCheck : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;

    [SerializeField]
    private float THRESHOLD;

    private float startTime;

    private AudioSource successSoundAudioSource;

    void Start()
    {
        startTime = Time.time;
        successSoundAudioSource = GetComponent<AudioSource>();
        StartCoroutine(MatchingChecker());
    }

    IEnumerator MatchingChecker()
    {
        while (true)
        {
            bool isWithinThreshold = true;
            foreach (var sensor in sensorDataObject.sensors)
            {
                if (Mathf.Abs(sensor.idealPressure - sensor.userPressure) < THRESHOLD)
                {
                    isWithinThreshold &= true;
                }
                else
                {
                    isWithinThreshold &= false;
                }
            }

            if (isWithinThreshold == true)
            {
                // Log time
                float endTime = Time.time;
                float totalTime = endTime - startTime;
                LogDuration(THRESHOLD, startTime, endTime, totalTime);
                Debug.Log("Logged");


                // Play sound
                if (!successSoundAudioSource.isPlaying)
                {
                    successSoundAudioSource.Play();
                }
                // Write success


                break;
            }

            yield return null;
        }
    }

    private void LogDuration(float threshold, float startTime, float endTime, float totalTime)
    {
        LogFile.Log($"DurationFile {LogFile.Timestamp()}", $"{THRESHOLD}, {startTime}, {endTime}, {totalTime}");
    }
}
