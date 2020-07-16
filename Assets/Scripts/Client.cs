using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int MAX_USER = 100;
    private const int PORT = 2445;
    private const int WEB_PORT = 2446;
    private const string SERVER_IP = "127.0.0.1";
    private const int BUFF_SIZE = 1024;

    private byte reliableChannel;
    private int connectId;
    private int hostId;
    private byte error;
    private bool isStarted = false;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        UpdateMessagePump();
    }

    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        hostId = NetworkTransport.AddHost(topo, 0);

#if !UNITY_WEBGL || UNITY_EDITOR
        connectId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
#else
        connectId = NetworkTransport.Connect(hostId, SERVER_IP, WEB_PORT, 0, out error);
#endif
        Debug.Log("Attempting to connect on " + SERVER_IP);

        isStarted = true;
    }

    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;

        int outHostId, connectId, chanId;
        byte[] recBuffer = new byte[BUFF_SIZE];
        int dataSize;

        NetworkEventType eventType = NetworkTransport.Receive(out outHostId, out connectId, out chanId, recBuffer, BUFF_SIZE, out dataSize, out error);

        switch (eventType)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Connected to the server");
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnection from the server");

                break;
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected event type received");
                break;
            case NetworkEventType.DataEvent:

                //Deserialize data
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetworkMsg msg = (NetworkMsg)bf.Deserialize(ms);

                OnData(connectId, chanId, outHostId, msg);
                break;
        }
    }

    #region ONDATA
    private void OnData(int connId, int channelId, int recHostId, NetworkMsg msg)
    {
        switch (msg.OperationCode)
        {
            case NetworkOP.None:
                break;

            case NetworkOP.OnCreateAccount:
                OnCreateAccount((Net_OnCreateAccount)msg);
                break;

            case NetworkOP.OnLoginRequest:
                OnLoginRequest((Net_OnLoginRequest)msg);
                break;
        }
    }

    private void OnCreateAccount(Net_OnCreateAccount msg)
    {
        if (msg.Sucess == 0) 
            Debug.Log("Account Created you can now login !");
    }

    private void OnLoginRequest(Net_OnLoginRequest msg)
    {
        if (msg.Sucess == 0)
            Debug.Log("User "+ msg.Username +" Logged in you can now login !");
    }

    #endregion

    #region SEND
    public void SendServer(NetworkMsg msg)
    {
        //data
        byte[] buffer = new byte[BUFF_SIZE];

        //Serialize data
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        bf.Serialize(ms, msg);

        NetworkTransport.Send(hostId, connectId, reliableChannel, buffer, BUFF_SIZE, out error);
    }

    public void SendCreateAccount(string username, string email, string password)
    {
        Net_CreateAccount msg = new Net_CreateAccount
        {
            Username = username,
            Email = email,
            Password = password
        };

        SendServer(msg);
    }

    public void SendLoginRequest(string username)
    {
        Net_LoginRequest msg = new Net_LoginRequest
        {
            Username = username
        };

        SendServer(msg);
    }

    public void SendLoginAccountRequest(string usernameOrEmail, string password)
    {
        Net_LoginAccountRequest msg = new Net_LoginAccountRequest
        {
            Username = usernameOrEmail,
            Password = password
        };

        SendServer(msg);
    }

    #endregion
}
