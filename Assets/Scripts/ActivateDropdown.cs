using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ActivateDropdown : MonoBehaviour
{

    [SerializeField] private Dropdown langageMenu;
    [SerializeField] private Dropdown roundMenu;
    [SerializeField] private Dropdown timerMenu;
    [SerializeField] private Button startButton;

    private void OnEnable()
    {
        if(PhotonNetwork.IsMasterClient) {
            langageMenu.interactable = true;
            roundMenu.interactable = true;
            timerMenu.interactable = true;
            startButton.interactable = true;
        } else {
            langageMenu.interactable = false;
            roundMenu.interactable = false;
            timerMenu.interactable = false;
            startButton.interactable = false;
        }
    }
}
