using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Photon.Pun;

public class InteractionModification : MonoBehaviour
{
    public float positionX;
    public GameObject[] gameObjects;
    public InputField resizeInputField, transformZInputField, colliderSizeInputField;
    public Dropdown shapeDropdown;
    public Toggle gravityToggle, kinematicsToggle, farManipulationToggle, colliderToggle;
    public GameObject targetPlane, startPlane;

    private PhotonView photonView;
    private List<int> gameObjectIDs;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ApplyOnClick()
    {
        gameObjectIDs = new List<int>();

        foreach (GameObject gameObject in gameObjects){
            if (gameObject.activeSelf)
            {
                int id = gameObject.GetComponent<PhotonView>().ViewID;
                gameObjectIDs.Add(id);
            }
        }
        int[] objects = gameObjectIDs.ToArray();
        int planeID = targetPlane.GetComponent<PhotonView>().ViewID;
        int startPlaneID = startPlane.GetComponent<PhotonView>().ViewID;

        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ModifyObjects", RpcTarget.All, objects, planeID, startPlaneID, resizeInputField.text, transformZInputField.text, colliderSizeInputField.text, shapeDropdown.options[shapeDropdown.value].text, gravityToggle.isOn, kinematicsToggle.isOn, farManipulationToggle.isOn, colliderToggle.isOn);
        }
    }

    [PunRPC]
    private void ModifyObjects(int[] gameObjectIDs, int planeID, int startPlaneID, string resizeInput, string transformZInput, string colliderInput, string shapeInput, bool gravity, bool kinematics, bool farManipulation, bool collider)
    {
        float size = float.Parse(resizeInput);
        float zPosition = float.Parse(transformZInput);
        float colliderSize = float.Parse(colliderInput);
        string shapeName = shapeInput;

        GameObject targetPlane = PhotonNetwork.GetPhotonView(planeID).gameObject;
        if (collider)
        {
            targetPlane.transform.position = new Vector3(targetPlane.transform.position.x, targetPlane.transform.position.y - (colliderSize * size) / 2, targetPlane.transform.position.z);
        }
        else
        {
            targetPlane.transform.position = new Vector3(targetPlane.transform.position.x, targetPlane.transform.position.y - (colliderSize * size) / 2, targetPlane.transform.position.z);
        }
        
        GameObject startPlane = PhotonNetwork.GetPhotonView(startPlaneID).gameObject;
        startPlane.transform.position = new Vector3(startPlane.transform.position.x, startPlane.transform.position.y - (colliderSize * size)/2, startPlane.transform.position.z);

        for (int i = 0; i < gameObjectIDs.Length; i++)
        {
            GameObject gameObject = PhotonNetwork.GetPhotonView(gameObjectIDs[i]).gameObject;
            gameObject.transform.localScale = new Vector3(size, size, size);

            if (gameObject.name == "SphereGhost" || gameObject.name == "CubeGhost")
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            } else
            {
                gameObject.transform.position = new Vector3(positionX, gameObject.transform.position.y, zPosition); 
                ChangeGravityAndKinematics(gameObject, gravity, kinematics);
                AllowFarManipulation(gameObject, farManipulation);
                ChangeColliderSize(gameObject, colliderSize);
                SetColliderBounds(gameObject, collider);
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

    private void ChangeGravityAndKinematics(GameObject gameObject, bool gravity, bool kinematics)
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = gravity;
        rigidbody.isKinematic = kinematics;
    }

    private void AllowFarManipulation(GameObject gameObject, bool farManipulation)
    {
        var objectManipulator = gameObject.GetComponent<ObjectManipulator>();
        objectManipulator.AllowFarManipulation = farManipulation;
    }

    private void ChangeColliderSize(GameObject gameObject, float colliderSize) 
    {
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
    }

    private void SetColliderBounds(GameObject gameObject, bool bounds)
    {
        var boundsControl = gameObject.GetComponent<BoundsControl>();
        boundsControl.enabled = bounds;
    }
}
