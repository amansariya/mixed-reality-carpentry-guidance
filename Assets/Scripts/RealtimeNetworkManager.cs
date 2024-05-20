using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading;
//using Photon.Realtime.LoadBalancingClient;
using ExitGames.Client.Photon;
using UnityEngine;

public class RealtimeNetworkManager : MonoBehaviour, IConnectionCallbacks//, IMatchmakingCallbacks, IInRoomCallbacks
{
    private LoadBalancingClient loadBalancingClient = new LoadBalancingClient();
    private EnterRoomParams enterRoomParams = new EnterRoomParams();

    public LoadBalancingClient LoadBalancingClient
    {
        get { return loadBalancingClient; }
    }

    void Start()
    {
        Application.runInBackground = true;
        //loadBalancingClient = new LoadBalancingClient();
        loadBalancingClient.AddCallbackTarget(this);
        loadBalancingClient.AppId = "11994ad1-fc3b-4426-909c-a80511e56d57"; // Replace with your Photon App ID
        loadBalancingClient.AppVersion = "1.0";
        loadBalancingClient.ConnectToNameServer();
        loadBalancingClient.ConnectToRegionMaster("usw");
        enterRoomParams.RoomName = "XRPressureRoom";
        //loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
    }

    void Update()
    {
        if (loadBalancingClient != null)
        {
            loadBalancingClient.Service();
        }
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to MasterServer. Now can join or create a room.");
        loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
        Debug.Log("Joined room");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Joined a room. Now connected to the GameServer.");
    }

    public void OnCreateRoom()
    {
        Debug.Log("Created a room.");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause.ToString());
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRoomFailed: {message}");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnCreateRoomFailed: {message}");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"OnJoinRandomFailed: {message}");
    }

    void OnApplicationQuit()
    {
        if (loadBalancingClient != null)
        {
            loadBalancingClient.Disconnect();
        }
    }


    // Implement other interface methods as needed...
}