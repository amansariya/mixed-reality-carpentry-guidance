using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserPressureValueManipulatorTest : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;
    System.Random random;

    void Start()
    {
        random = new System.Random();
        StartCoroutine(UpdateUserPressureValue());
    }

    IEnumerator UpdateUserPressureValue()
    {
        while (true)
        {
            // Thumb
            for (int i = 0; i < 4; i++)
            {
                float min = 0.0f;
                float max = 0.2f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }

            // Index
            for (int i = 4; i < 7; i++)
            {
                float min = 0.8f;
                float max = 1.0f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }

            // Middle
            for (int i = 7; i < 10; i++)
            {
                float min = 0.1f;
                float max = 0.8f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }

            //Ring
            for (int i = 10; i < 13; i++)
            {
                float min = 0.1f;
                float max = 0.8f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }

            // Pinky
            for (int i = 13; i < 16; i++)
            {
                float min = 0.0f;
                float max = 0.15f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }

            // Palm
            for (int i = 16; i < 20; i++)
            {
                float min = 0.0f;
                float max = 0.3f;
                double randomNumber = random.NextDouble() * (max - min) + min;
                sensorDataObject.sensors[i].userPressure = (float)randomNumber;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
