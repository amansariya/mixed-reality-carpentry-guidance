using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMatchingCheck : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;

    [SerializeField]
    private float THRESHOLD;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MatchingChecker());
    }

    IEnumerator MatchingChecker()
    {
        while (true)
        {
            bool isWithinThreshold = false;
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
                // Play sound
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                // Write success


                break;
            }

            yield return null;
        }
    }
}
