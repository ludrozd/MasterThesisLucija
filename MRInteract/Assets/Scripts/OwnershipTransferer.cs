using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OwnershipTransferer : MonoBehaviour
{
    public GameObject[] gameObjects;

    void Awake()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            foreach (GameObject go in gameObjects)
            {
                go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[1].ActorNumber);
            }
        }

    }

}
