using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;

public class DataReceiver : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private RealtimeNetworkManager realtimeNetworkManager;
    private LoadBalancingClient loadBalancingClient;
    
    [SerializeField]
    private GlobCentreValuesScriptableObject globCentreValuesScriptableObject;

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
            object data = photonEvent.CustomData;

            if (data is byte[] bytes)
            {
                Debug.Log(bytes.Length);
                int numFloats = bytes.Length / sizeof(float);
                float[] floatArray = new float[numFloats];
                Buffer.BlockCopy(bytes, 0, floatArray, 0, bytes.Length);
                Debug.Log("floatArray.Length: " + floatArray.Length.ToString());
                //Debug.Log(globCentreValuesScriptableObject.sensors.Length);
                //for (int i = 0; i < floatArray.Length; i++)
                //{
                //    Debug.Log(floatArray[i]);
                //    globCentreValuesScriptableObject.sensors[i].userPressure = floatArray[i];
                //}

                for (int i = 0; i < floatArray.Length; i++)
                {
                    Debug.Log("Index: " + i);
                    Debug.Log("Value: " + floatArray[i]);
                }

                // Use floatArray as needed
                //foreach (float value in floatArray)
                //{
                //    Debug.Log(value); // Example: logging the values
                //    //globCentreValuesScriptableObject.sensors[].userPressure = value;
                //}
            }
            else
            {
                Debug.LogError("Expected a byte array, but data was of a different type.");
            }
        }
    }








}