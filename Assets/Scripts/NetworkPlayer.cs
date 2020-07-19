using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [HideInInspector] public string name;
    [HideInInspector] public int score;
    [HideInInspector] public List<string> wordList;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) {
            name = PhotonNetwork.LocalPlayer.NickName;
        }
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
    }
}
