using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput, joinInput;
    public TextMeshProUGUI createText, joinText;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void CreateRoomHolo()
    {
        PhotonNetwork.CreateRoom(createText.text);
    }

    public void JoinRoomHolo()
    {
        PhotonNetwork.JoinRoom(joinText.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MenuScene");
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
