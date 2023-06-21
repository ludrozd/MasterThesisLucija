using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public int level;
    public Text buttonText;

    public void ChangeScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonText.text = "Loading...";
            PhotonNetwork.LoadLevel(level);
        }

    }

}
