using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    //public InputField usernameInput;
    public Text text;

    //public void OnClickConnect()
    //{
    //    if (usernameInput.text.Length > 0)
    //    {
    //        PhotonNetwork.NickName = usernameInput.text;
    //        buttonText.text = "Connecting...";
    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            text.text = "Connecting...";
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
