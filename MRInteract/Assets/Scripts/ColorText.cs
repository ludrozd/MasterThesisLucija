using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ColorText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color text_color;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ChangeColorOnClick()
    {
        int textID = text.GetComponent<PhotonView>().ViewID;
        photonView.RPC("ChangeColorText", RpcTarget.All, textID);
    }

    [PunRPC]
    private void ChangeColorText(int textID){

        TextMeshProUGUI textObj = PhotonNetwork.GetPhotonView(textID).GetComponent<TextMeshProUGUI>();
        textObj.color = text_color;
    }
}
