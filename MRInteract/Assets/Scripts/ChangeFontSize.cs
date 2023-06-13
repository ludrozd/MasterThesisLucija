using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeFontSize : MonoBehaviour
{
    public Button applyButton;
    public TextMeshProUGUI text;
    public Text minText, maxText;
    public Slider slider;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        applyButton.onClick.AddListener(OnClickChangeSlider);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnClickChangeSlider()
    {
        int minSliderID = minText.GetComponent<PhotonView>().ViewID;
        int maxSliderID = maxText.GetComponent<PhotonView>().ViewID;
        int sliderID = slider.GetComponent<PhotonView>().ViewID;
        photonView.RPC("ChangeMinMaxSlider", RpcTarget.All, minSliderID, maxSliderID, sliderID);
    }

    public void OnSliderValueChanged(float value)
    {
        int textID = text.GetComponent<PhotonView>().ViewID;
        int sliderID = slider.GetComponent<PhotonView>().ViewID;
        photonView.RPC("SyncSliderValue", RpcTarget.All, value, sliderID, textID);
    }

    [PunRPC]
    private void SyncSliderValue(float value, int sliderID, int textID)
    {
        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();
        sliderObj.value = value;
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
