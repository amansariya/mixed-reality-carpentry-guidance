using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialCommunication : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM3", 115200);
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;

    Dictionary<int, int> channelMapping = new Dictionary<int, int>
    {
        { 0, 15 },  // Pinky Metacarpal
        { 1, 3 },   // Thumb Base
        { 2, 18 },  // Palm Bottom Left
        { 3, 13 },  // Pinky Tip
        { 4, 8 },   // Middle Proximal
        { 5, 11 },  // Ring Proximal
        { 6, 10 },  // Ring Tip
        { 7, 14 },  // Pinky Proximal
        { 8, 7 },   // Middle Tip
        { 9, 4 },   // Index Tip
        { 10, 12 }, // Ring Metacarpal
        { 11, 17 }, // Palm Center
        { 12, 19 }, // Palm Bottom
        { 13, 16 }, // Palm Right (Palm Left in the second list)
        { 14, 9 },  // Middle Metacarpal
        { 15, 6 },  // Index Metacarpal
        { 16, 2 },  // Thumb Metacarpal
        { 17, 5 },  // Pointer Proximal (Index Proximal in the second list)
        { 18, 0 },  // Thumb Tip
        { 19, 1 }   // Thumb Proximal
    };
    void Start()
    {
        stream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (stream.IsOpen)
        {
            try
            {
                string data = stream.ReadLine();
                //Debug.Log(data);

                // Extract the values from the data string
                string[] parts = data.Split('|');
                if (parts.Length > 3)
                {
                    string[] valueStrings = parts[3].Split(',');
                    if (valueStrings.Length == 20)
                    {
                        float[] normalizedValues = new float[20];

                        for (int i = 0; i < valueStrings.Length; i++)
                        {
                            int value = int.Parse(valueStrings[i]);
                            // Invert and normalize the value
                            normalizedValues[i] = (1023f - value) / 1023f;
                        }

                        //string temp = "";

                        //for (int i = 0; i < 20; i++)
                        //{
                        //    temp += normalizedValues[i].ToString() + " ";
                        //}


                        //Debug.Log(temp);

                        for (int i = 0; i < valueStrings.Length; i ++)
                        {
                            sensorDataObject.sensors[channelMapping[i]].userPressure = normalizedValues[i];
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
