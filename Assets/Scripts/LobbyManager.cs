using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region SINGLETON
    private static LobbyManager _instance = null;
    public static LobbyManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] private byte maxPlayerPerRoom = 8; 
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private WordListDisplay _playersDisplayLobby;
    [SerializeField] private WordListDisplay _playersDisplayGame;
    [SerializeField] private Text _roomNameText;

    private string _gameVersion = "1.0";
    private bool _isConnecting = false;
    private string _roomName = string.Empty;
    private bool _isRandomJoin = true;

    #region UNITY

    private void Start()
    {
        PhotonNetwork.GameVersion = this._gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region PUN CALLBACKS
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room : " + PhotonNetwork.CurrentRoom.Name);

        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        _gameManager.GoToLobbyScreen();

        foreach(Player p in PhotonNetwork.PlayerList) {
            _playersDisplayLobby.AddWordToList(p.NickName);
            _playersDisplayGame.AddWordToList(p.NickName);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("Join named room failed creating a new room");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayerPerRoom;
        options.IsVisible = false;

        PhotonNetwork.CreateRoom(_roomName, options);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join random room failed creating a new room");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayerPerRoom;

        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected because of " + cause);
        //_isConnecting = false;

        _gameManager.GoToConnectScreen();
    }

    public override void OnLeftRoom()
    {
        _gameManager.GoToConnectScreen();
        _playersDisplayLobby.ClearList();
        _playersDisplayGame.ClearList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player " + newPlayer.NickName + " entered the room");
        _playersDisplayLobby.AddWordToList(newPlayer.NickName);
        _playersDisplayGame.AddWordToList(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player : " + otherPlayer.NickName + " left the room");
        _playersDisplayLobby.RemoveWord(otherPlayer.NickName);
        _playersDisplayGame.RemoveWord(otherPlayer.NickName);
    }

    #endregion

    public void Connect(bool isRandomJoin, string roomName)
    {
       // _isConnecting = true;

        if (!isRandomJoin) {
            _roomName = roomName;
        }

        if (PhotonNetwork.IsConnected)
        {
            if (isRandomJoin) {
                PhotonNetwork.JoinRandomRoom();
            } else {
                PhotonNetwork.JoinRoom(roomName);
            }
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
