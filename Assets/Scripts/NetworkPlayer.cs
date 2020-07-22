using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public string name;
    public int score;
    public List<string> wordList;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) {
            name = PhotonNetwork.LocalPlayer.NickName;
        }

        FindObjectOfType<GameManager>().players.Add(this);
    }

    public void SendWorldList(List<string> wl)
    {
        if (PV.IsMine) {
            PV.RPC("RPC_SetWorldList", RpcTarget.OthersBuffered, string.Join(",", wl));
            wordList.Clear();
            wordList = wl;
        }
    }

    [PunRPC]
    private void RPC_SetWorldList(string wl)
    {
        wordList.Clear();
        string[] splits = wl.Split(',');
        foreach(string split in splits) {
            wordList.Add(split);
        }
        Debug.Log("List received " + string.Join(",",wordList));
    }
}
