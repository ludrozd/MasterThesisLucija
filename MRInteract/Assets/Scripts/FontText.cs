using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TMP_FontAsset fontAsset;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ChangeFontOnClick()
    {
        int textID = text.GetComponent<PhotonView>().ViewID;
        photonView.RPC("ChangeTextFont", RpcTarget.All, textID);
    }

    [PunRPC]
    private void ChangeTextFont(int textID)
    {
        TextMeshProUGUI textObj = PhotonNetwork.GetPhotonView(textID).GetComponent<TextMeshProUGUI>();
        textObj.font = fontAsset;
    }
}
