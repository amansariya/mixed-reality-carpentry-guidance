using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DataReceiver : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private RealtimeNetworkManager realtimeNetworkManager;
    private LoadBalancingClient loadBalancingClient;
    
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
        // You need to obtain a reference to your LoadBalancingClient instance here.
        // This might involve accessing it from another component or passing it directly.
        loadBalancingClient = realtimeNetworkManager.LoadBalancingClient;
        loadBalancingClient.AddCallbackTarget(this);
    }

    void OnDestroy()
    {
        if (loadBalancingClient != null)
        {
            loadBalancingClient.RemoveCallbackTarget(this);
        }
    }


    //public void OnEvent(EventData photonEvent)
    //{
    //    byte eventCode = photonEvent.Code;
    //    if (eventCode == 1) // Replace YOUR_CUSTOM_EVENT_CODE with your actual event code
    //    {


    //        //float receivedValue = (float)photonEvent.CustomData;
    //        //object[] data = (object[])photonEvent.CustomData; // Correctly cast to object[]
    //        //float receivedValue = (float)data[0];
    //        //Debug.Log("Received float value: " + receivedValue);
    //        // Handle the received float value here
    //    }

    //}

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        { // This must match the eventCode used in C++
            Debug.Log("EventCode matched");


            object receivedData = photonEvent.CustomData;
            //Debug.Log(data.GetType());
            //Debug.Log(data);

            String data = receivedData.ToString();


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

                    string temp = "";

                    for (int i = 0; i < 20; i++)
                    {
                        temp += normalizedValues[i].ToString() + " ";
                    }


                    Debug.Log(temp);

                    for (int i = 0; i < valueStrings.Length; i++)
                    {
                        sensorDataObject.sensors[channelMapping[i]].userPressure = normalizedValues[i];
                    }
                }
            }



        }
    }

    //public void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == 1)
    //    { // This must match the eventCode used in C++
    //        Debug.Log("EventCode matched");


    //        object data = photonEvent.CustomData;
    //        Debug.Log(data.GetType());
    //        Debug.Log(data);

    //        if (data is byte[] bytes)
    //        {
    //            Debug.Log(bytes.Length);
    //            int numFloats = bytes.Length / sizeof(float);
    //            float[] floatArray = new float[numFloats];
    //            Buffer.BlockCopy(bytes, 0, floatArray, 0, bytes.Length);
    //            Debug.Log("floatArray.Length: " + floatArray.Length.ToString());
    //            //Debug.Log(globCentreValuesScriptableObject.sensors.Length);
    //            //for (int i = 0; i < floatArray.Length; i++)
    //            //{
    //            //    Debug.Log(floatArray[i]);
    //            //    globCentreValuesScriptableObject.sensors[i].userPressure = floatArray[i];
    //            //}

    //            for (int i = 0; i < floatArray.Length; i++)
    //            {
    //                Debug.Log("Index: " + i);
    //                Debug.Log("Value: " + floatArray[i]);
    //            }

    //            // Use floatArray as needed
    //            //foreach (float value in floatArray)
    //            //{
    //            //    Debug.Log(value); // Example: logging the values
    //            //    //globCentreValuesScriptableObject.sensors[].userPressure = value;
    //            //}
    //        }
    //        else
    //        {
    //            Debug.LogError("Expected a byte array, but data was of a different type.");
    //        }
    //    }
    //}
}