using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    [Header("References")]
    [Space]
    public Client client;
    public InputField loginUsername;
    public InputField loginAccountUsername;
    public InputField loginAccountPassword;
    public InputField createUsername;
    public InputField createEmail;
    public InputField createPassword;

    public void OnClickLoginButton()
    {
        string username = loginUsername.text;

        client.SendLoginRequest(username);
    }

    public void OnClickLoginAccountButton()
    {
        string username = loginAccountUsername.text;
        string password = loginAccountPassword.text;

        client.SendLoginAccountRequest(username, password);
    }

    public void OnClickCreateAccountButton()
    {
        string username = createUsername.text;
        string email = createEmail.text;
        string password = loginAccountPassword.text;

        client.SendCreateAccount(username, email, password);
    }
}
