using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class DataReceiver : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private RealtimeNetworkManager realtimeNetworkManager;
    private LoadBalancingClient loadBalancingClient;

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

    public void OnEvent(EventData photonEvent)
    {
        //byte eventCode = photonEvent.Code;
        //if (eventCode == 1) // Replace YOUR_CUSTOM_EVENT_CODE with your actual event code
        //{
        //    float receivedValue = (float)photonEvent.CustomData;
        //    Debug.Log("Received float value: " + receivedValue);
        //    // Handle the received float value here
        //}
        //float receivedValue = (float)photonEvent.CustomData;
        object[] data = (object[])photonEvent.CustomData; // Correctly cast to object[]
        float receivedValue = (float)data[0]; // Then access the float value
        Debug.Log("Received float value: " + receivedValue);
    }
}