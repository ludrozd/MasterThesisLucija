using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
            photonView.RPC("ModifyObjects", RpcTarget.All, (object)objects);
        }
    }

    [PunRPC]
    private void ModifyObjects(int[] gameobjectIDs)
    {
        float size = float.Parse(resizeInputField.text);
        float ghostSize = float.Parse(ghostResizeInputField.text);
        float zPosition = float.Parse(transformZInputField.text);
        float yPosition = float.Parse(transformYInputField.text);
        float xRotate = float.Parse(rotateXInputField.text);
        float yRotate = float.Parse(rotateYInputField.text);
        float colliderSize = float.Parse(colliderSizeInputField.text);
        string shapeName = shapeDropdown.options[shapeDropdown.value].text;
        //Debug.Log("Dropdown value " + shapeName);

        for (int i = 0; i < gameobjectIDs.Length; i++)
        {
            GameObject gameObject = PhotonNetwork.GetPhotonView(gameObjectIDs[i]).gameObject;

            if (gameObject.name == shapeName+"Ghost")
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
                SetColliderBounds(gameObject);
                AllowFarManipulation(gameObject);
            }

            SetObjectShape(gameObject, shapeName);
        }
    }

    private void SetObjectShape(GameObject gameObject, string shapeName)
    {
        if (!(shapeName.Equals("Cube") && gameObject.name.StartsWith("Cube") || shapeName.Equals("Sphere") && gameObject.name.StartsWith("Sphere")))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void AllowFarManipulation(GameObject gameObject)
    {
        var objectManipulator = gameObject.GetComponent<ObjectManipulator>();
        objectManipulator.AllowFarManipulation = farManipulationToggle.isOn;
    }

    private void ChangeColliderSize(GameObject gameObject, float colliderSize)
    {
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
    }

    private void SetColliderBounds(GameObject gameObject)
    {
        var boundsControl = gameObject.GetComponent<BoundsControl>();
        boundsControl.enabled = colliderToggle.isOn;
    }
}
