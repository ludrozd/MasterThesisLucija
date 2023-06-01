using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeFontSize : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Text minText, maxText;
    public Slider slider;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        int minSliderID = minText.GetComponent<PhotonView>().ViewID;
        int maxSliderID = maxText.GetComponent<PhotonView>().ViewID;
        int sliderID = slider.GetComponent<PhotonView>().ViewID;
        photonView.RPC("ChangeMinMaxSlider", RpcTarget.All, minSliderID, maxSliderID, sliderID);
    }

    public void ChangeSizeOnClick()
    {
        int textID = text.GetComponent<PhotonView>().ViewID;
        int sliderID = slider.GetComponent<PhotonView>().ViewID;
        photonView.RPC("ChangeFontTextSize", RpcTarget.All, textID, sliderID);
    }

    [PunRPC]
    private void ChangeFontTextSize(int textID, int sliderID)
    {
        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();
        TextMeshProUGUI textObj = PhotonNetwork.GetPhotonView(textID).GetComponent<TextMeshProUGUI>();
        textObj.fontSize = sliderObj.value;
    }

    [PunRPC]
    private void ChangeMinMaxSlider(int minSliderID, int maxSliderID, int sliderID)
    {
        Text minTextObj = PhotonNetwork.GetPhotonView(minSliderID).GetComponent<Text>();
        Text maxTextObj = PhotonNetwork.GetPhotonView(maxSliderID).GetComponent<Text>();
        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();
        minTextObj.text = sliderObj.minValue.ToString();
        maxTextObj.text = sliderObj.maxValue.ToString();
    }
}
