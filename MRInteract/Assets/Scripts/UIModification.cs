using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModification : MonoBehaviour
{
    public GameObject slate;
    public InputField resizeInputField, transformZInputField, transformYInputField, angleInputField, sliderMinInputField, sliderMaxInputField, elasticityInputField, decelerationInputField, buttonSizeInputField;
    public Dropdown buttonShapeDropdown;
    public Toggle buttonFeedbackToggle;
    public Slider slider;
    public GameObject scrollView;
    public GameObject[] buttonSets;
    public GameObject toggles;

    private PhotonView photonView;
    private List<int> buttonSetIDs;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ApplyOnClick()
    {
        //PhotonView[] views = FindObjectsOfType<PhotonView>();
        //Debug.Log(views.Length);
        buttonSetIDs = new List<int>();

        foreach (GameObject gameObject in buttonSets)
        {
            if (gameObject.activeSelf)
            {
                int id = gameObject.GetComponent<PhotonView>().ViewID;
                buttonSetIDs.Add(id);
            }
        }
        int[] buttonObjectsID = buttonSetIDs.ToArray();
        int slateID = slate.GetComponent<PhotonView>().ViewID;
        int sliderID = slider.GetComponent<PhotonView>().ViewID;
        int togglesID = toggles.GetComponent<PhotonView>().ViewID;
        int scrollViewID = scrollView.GetComponent<PhotonView>().ViewID;

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log(gameObjectIDs.Count);
            photonView.RPC("SetUIForm", RpcTarget.All, slateID, (object)buttonObjectsID, sliderID, togglesID, scrollViewID);
        }
    }

    [PunRPC]
    private void SetUIForm(int slateID, int[] buttonObjectsID, int sliderID, int togglesID, int scrollViewID)
    {
        SetSlateSizeDistanceRotation(slateID);
        SetButtonShape(buttonObjectsID);
        ButtonFeedbackAndSize(buttonObjectsID, sliderID, togglesID);
        MinMaxSlider(sliderID);
        ScrollSetup(scrollViewID);
    }

    private void SetSlateSizeDistanceRotation(int slateID)
    {
        GameObject slateObj = PhotonNetwork.GetPhotonView(slateID).gameObject;

        float size = float.Parse(resizeInputField.text);
        float zPosition = float.Parse(transformZInputField.text);
        float yPosition = float.Parse(transformYInputField.text);
        float anglePanel = float.Parse(angleInputField.text);

        slateObj.transform.localScale = new Vector3(size, size, size);
        slateObj.transform.position = new Vector3(slateObj.transform.position.x, yPosition, zPosition);
        slateObj.transform.Rotate(anglePanel, slateObj.transform.rotation.y, slateObj.transform.rotation.z);
    }

    private void SetButtonShape(int[] buttonObjectsID)
    {
        string shapeName = buttonShapeDropdown.options[buttonShapeDropdown.value].text;

        foreach (int buttonSetID in buttonObjectsID)
        {
            GameObject buttonSet = PhotonNetwork.GetPhotonView(buttonSetID).gameObject;

            if (buttonSet.name.StartsWith("Buttons") && shapeName.Equals("Square 2D"))
            {
                buttonSet.SetActive(true);
            }
            else if (buttonSet.name.StartsWith("Square3D") && shapeName.Equals("Square 3D"))
            {
                buttonSet.SetActive(true);
            }
            else if (buttonSet.name.StartsWith("Round3D") && shapeName.Equals("Circle 3D"))
            {
                buttonSet.SetActive(true);
            }
            else
            {
                buttonSet.SetActive(false);
            }
        }

    }

    private void ButtonFeedbackAndSize(int[] buttonObjectsID, int sliderID, int togglesID)
    {
        float btnSize = float.Parse(buttonSizeInputField.text);

        foreach (int buttonSetID in buttonObjectsID)
        {
            GameObject buttonSet = PhotonNetwork.GetPhotonView(buttonSetID).gameObject;
            Button[] buttons = buttonSet.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                if(buttonFeedbackToggle.isOn == false)
                {
                    button.transition = Selectable.Transition.None;
                }

                button.transform.localScale = new Vector3(btnSize, btnSize, btnSize);
            }
        }

        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();

        if (buttonFeedbackToggle.isOn == false)
        {
            sliderObj.transition = Selectable.Transition.None;
        }

        GameObject toggleSet = PhotonNetwork.GetPhotonView(togglesID).gameObject;

        Toggle[] toggleButtons = toggleSet.GetComponentsInChildren<Toggle>();
        foreach(Toggle toggle in toggleButtons)
        {
            if (buttonFeedbackToggle.isOn == false)
            {
                toggle.transition = Selectable.Transition.None;
            }
        }
    }

    private void MinMaxSlider(int sliderID)
    {
        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();

        int minValue = int.Parse(sliderMinInputField.text);
        int maxValue = int.Parse(sliderMaxInputField.text);

        sliderObj.minValue = minValue;
        sliderObj.maxValue = maxValue;
    }

    private void ScrollSetup(int scrollViewID)
    {
        float elasticity = float.Parse(elasticityInputField.text);
        float deceleration = float.Parse(decelerationInputField.text);

        GameObject scrollViewObj = PhotonNetwork.GetPhotonView(scrollViewID).gameObject;

        var scrollRect = scrollViewObj.GetComponent<ScrollRect>();
        scrollRect.decelerationRate = deceleration;
        scrollRect.elasticity = elasticity;
    }
}
