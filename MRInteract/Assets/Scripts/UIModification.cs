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

    private PhotonView photonView;
    private List<int> buttonSetIDs;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ApplyOnClick()
    {
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
        int scrollViewID = scrollView.GetComponent<PhotonView>().ViewID;

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetUIForm", RpcTarget.All, slateID, buttonObjectsID, sliderID, scrollViewID, resizeInputField.text, transformZInputField.text, transformYInputField.text, angleInputField.text, sliderMinInputField.text, sliderMaxInputField.text, elasticityInputField.text, decelerationInputField.text, buttonSizeInputField.text, buttonShapeDropdown.options[buttonShapeDropdown.value].text, buttonFeedbackToggle.isOn);
        }
    }

    [PunRPC]
    private void SetUIForm(int slateID, int[] buttonObjectsID, int sliderID, int scrollViewID, string resizeInput, string transformZInput, string transformYInput, string angleInput, string sliderMinInput, string sliderMaxInput, string elasticityInput, string decelerationInput, string buttonSizeInput, string shapeInput, bool hasFeedback)
    {
        SetSlateSizeDistanceRotation(slateID, resizeInput, transformZInput, transformYInput, angleInput);
        SetButtonShape(buttonObjectsID, shapeInput);
        ButtonFeedbackAndSize(buttonObjectsID, sliderID, buttonSizeInput, hasFeedback);
        MinMaxSlider(sliderID, sliderMinInput, sliderMaxInput);
        ScrollSetup(scrollViewID, elasticityInput, decelerationInput);
    }

    private void SetSlateSizeDistanceRotation(int slateID, string resizeInput, string transformZInput, string transformYInput, string angleInput)
    {
        GameObject slateObj = PhotonNetwork.GetPhotonView(slateID).gameObject;

        float size = float.Parse(resizeInput);
        float zPosition = float.Parse(transformZInput);
        float yPosition = float.Parse(transformYInput);
        float anglePanel = float.Parse(angleInput);

        slateObj.transform.localScale = new Vector3(size, size, size);
        slateObj.transform.position = new Vector3(slateObj.transform.position.x, yPosition, zPosition);
        slateObj.transform.Rotate(anglePanel, slateObj.transform.rotation.y, slateObj.transform.rotation.z);
    }

    private void SetButtonShape(int[] buttonObjectsID, string shapeName)
    {

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

    private void ButtonFeedbackAndSize(int[] buttonObjectsID, int sliderID, string buttonSize, bool hasFeedback)
    {
        float btnSize = float.Parse(buttonSize);

        foreach (int buttonSetID in buttonObjectsID)
        {
            GameObject buttonSet = PhotonNetwork.GetPhotonView(buttonSetID).gameObject;
            Button[] buttons = buttonSet.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                if(hasFeedback == false)
                {
                    button.transition = Selectable.Transition.None;
                }

                button.transform.localScale = new Vector3(btnSize, btnSize, btnSize);
            }
        }

        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();

        if (hasFeedback == false)
        {
            sliderObj.transition = Selectable.Transition.None;
        }

    }

    private void MinMaxSlider(int sliderID, string sliderMinInput, string sliderMaxInput)
    {
        Slider sliderObj = PhotonNetwork.GetPhotonView(sliderID).GetComponent<Slider>();

        int minValue = int.Parse(sliderMinInput);
        int maxValue = int.Parse(sliderMaxInput);

        sliderObj.minValue = minValue;
        sliderObj.maxValue = maxValue;
    }

    private void ScrollSetup(int scrollViewID, string elasticityInput, string decelerationInput)
    {
        float elasticity = float.Parse(elasticityInput);
        float deceleration = float.Parse(decelerationInput);

        GameObject scrollViewObj = PhotonNetwork.GetPhotonView(scrollViewID).gameObject;

        var scrollRect = scrollViewObj.GetComponent<ScrollRect>();
        scrollRect.decelerationRate = deceleration;
        scrollRect.elasticity = elasticity;
    }
}
