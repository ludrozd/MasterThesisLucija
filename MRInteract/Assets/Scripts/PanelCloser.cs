using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCloser : MonoBehaviour
{
    public GameObject panel;
    private PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void CloseOnClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int panelID = panel.GetComponent<PhotonView>().ViewID;
            photonView.RPC("ClosePanel", RpcTarget.All, panelID);
        }
    }

    [PunRPC]
    private void ClosePanel(int panelID)
    {
        GameObject settingPanel = PhotonNetwork.GetPhotonView(panelID).gameObject;
        settingPanel.SetActive(false);
    }
}
