using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static System.Net.Mime.MediaTypeNames;

public class ObjectModifier : MonoBehaviour
{
    public float positionX;
    public GameObject[] gameObjects;
    public InputField resizeInputField, ghostResizeInputField, transformZInputField, transformYInputField, rotateXInputField, rotateYInputField, colliderSizeInputField;
    public Dropdown shapeDropdown;
    public Toggle farManipulationToggle, colliderToggle;

    private PhotonView photonView;
    private List<int> gameObjectIDs;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ApplyOnClick()
    {
        gameObjectIDs = new List<int>();
       

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.activeSelf)
            {
                int id = gameObject.GetComponent<PhotonView>().ViewID;
                gameObjectIDs.Add(id);
            }
        }
        int[] objects = gameObjectIDs.ToArray();

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log(gameObjectIDs.Count);
            photonView.RPC("ModifyManipulatedObjects", RpcTarget.All, objects, resizeInputField.text, ghostResizeInputField.text, transformZInputField.text, transformYInputField.text, rotateXInputField.text, rotateYInputField.text, colliderSizeInputField.text, shapeDropdown.options[shapeDropdown.value].text, farManipulationToggle.isOn, colliderToggle.isOn);
        }
    }

    [PunRPC]
    private void ModifyManipulatedObjects(int[] gameobjectIDs, string resizeInput, string ghostResizeInput, string transformZInput, string transformYInput, string rotateXInput, string rotateYInput, string colliderSizeInput, string shapeInput, bool hasFarManipulation, bool hasCollider)
    {
        float size = float.Parse(resizeInput);
        float ghostSize = float.Parse(ghostResizeInput);
        float zPosition = float.Parse(transformZInput);
        float yPosition = float.Parse(transformYInput);
        float xRotate = float.Parse(rotateXInput);
        float yRotate = float.Parse(rotateYInput);
        float colliderSize = float.Parse(colliderSizeInput);
        string shapeName = shapeInput;
        //Debug.Log("Dropdown value " + shapeName);

        for (int i = 0; i < gameobjectIDs.Length; i++)
        {
            GameObject gameObject = PhotonNetwork.GetPhotonView(gameobjectIDs[i]).gameObject;

            if (gameObject.name == shapeName + "Ghost")
            {
                gameObject.transform.localRotation = Quaternion.Euler(xRotate, yRotate, gameObject.transform.localRotation.z);
                gameObject.transform.localScale = new Vector3(ghostSize, ghostSize, ghostSize);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPosition, zPosition);
            }

            if (gameObject.name == shapeName)
            {
                gameObject.transform.position = new Vector3(positionX, yPosition, zPosition);
                gameObject.transform.localScale = new Vector3(size, size, size);
                ChangeColliderSize(gameObject, colliderSize);
                SetColliderBounds(gameObject, hasCollider);
                AllowFarManipulation(gameObject, hasFarManipulation);
            }

            SetObjectShape(gameObject, shapeName);
        }
    }

    private void SetObjectShape(GameObject gameObject, string shapeName)
    {
        if (!(shapeName.Equals("Cube") && gameObject.name.StartsWith("Cube") || shapeName.Equals("Cylinder") && gameObject.name.StartsWith("Cylinder")))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void AllowFarManipulation(GameObject gameObject, bool hasFarManipulation)
    {
        var objectManipulator = gameObject.GetComponent<ObjectManipulator>();
        objectManipulator.AllowFarManipulation = hasFarManipulation;
    }

    private void ChangeColliderSize(GameObject gameObject, float colliderSize)
    {
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        if(gameObject.name == "Cube")
        {
            boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
        } else if (gameObject.name == "Cylinder")
        {
            boxCollider.size = new Vector3(colliderSize, colliderSize * 2, colliderSize);
        }

    }

    private void SetColliderBounds(GameObject gameObject, bool hasCollider)
    {
        var boundsControl = gameObject.GetComponent<BoundsControl>();
        boundsControl.enabled = hasCollider;
    }
}
