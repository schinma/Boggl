using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    public string playerNamePrefKey = "playerName"; 
    public Button continueButton;
    public bool isRandomJoin;

    [SerializeField]
    private InputField _nameInputField;
    [SerializeField]
    private InputField _roomInputField;

    private string _roomName;

    // Start is called before the first frame update
    void Start()
    {
        string defaultName = string.Empty;
        _roomName = string.Empty;

        if (PlayerPrefs.HasKey(playerNamePrefKey)) {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            _nameInputField.text = defaultName;
        }
    }

    public void SetPlayerName()
    {
        if (isRandomJoin) {
            continueButton.interactable = !string.IsNullOrEmpty(_nameInputField.text);
        } else {
            continueButton.interactable = !string.IsNullOrEmpty(_nameInputField.text) && !string.IsNullOrEmpty(_roomInputField.text);
        }
    }

    public void SetRoomName()
    {
        continueButton.interactable = !string.IsNullOrEmpty(_roomInputField.text) && !string.IsNullOrEmpty(_nameInputField.text);
        _roomName = _roomInputField.text;
    }

    public void ClickPlayButton()
    {
        string name = _nameInputField.text;
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(playerNamePrefKey, name);
        LobbyManager.Instance.Connect(isRandomJoin, _roomName);
    }
}
