using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

public class TestSender : MonoBehaviour
{
    [SerializeField]
    private RealtimeNetworkManager realtimeNetworkManager;
    private LoadBalancingClient loadBalancingClient;

    void Start()
    {
        // Assuming loadBalancingClient is initialized and connected elsewhere, e.g., in your RealtimeNetworkManager
        // Example: loadBalancingClient = FindObjectOfType<RealtimeNetworkManager>().LoadBalancingClient;
        loadBalancingClient = realtimeNetworkManager.LoadBalancingClient;
        loadBalancingClient.AddCallbackTarget(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SendTestFloatEvent(123.45f); // Example float value to send
        }
    }

    private void SendTestFloatEvent(float value)
    {
        byte eventCode = 1; // Your custom event code
        object content = new object[] { value }; // Wrap your float value in an object array
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Who should receive this event
        SendOptions sendOptions = new SendOptions { Reliability = true }; // Ensure reliable delivery

        loadBalancingClient.OpRaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
    }
}
