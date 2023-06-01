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

    private PhotonView photonView;
    private List<int> gameObjectIDs;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void ApplyOnClick()
    {
        //PhotonView[] views = FindObjectsOfType<PhotonView>();
        //Debug.Log(views.Length);
        gameObjectIDs = new List<int>();

        foreach (GameObject gameObject in gameObjects){
            if (gameObject.activeSelf)
            {
                int id = gameObject.GetComponent<PhotonView>().ViewID;
                gameObjectIDs.Add(id);
            }
        }
        int[] objects = gameObjectIDs.ToArray();

        if(PhotonNetwork.IsMasterClient)
        {
            //Debug.Log(gameObjectIDs.Count);
            photonView.RPC("ModifyObjects", RpcTarget.All, objects, resizeInputField.text, transformZInputField.text, colliderSizeInputField.text, shapeDropdown.options[shapeDropdown.value].text, gravityToggle.isOn, kinematicsToggle.isOn, farManipulationToggle.isOn, colliderToggle.isOn);
        }
    }

    [PunRPC]
    private void ModifyObjects(int[] gameObjectIDs, string resizeInput, string transformZInput, string colliderInput, string shapeInput, bool gravity, bool kinematics, bool farManipulation, bool collider)
    {
        float size = float.Parse(resizeInput);
        float zPosition = float.Parse(transformZInput);
        float colliderSize = float.Parse(colliderInput);
        string shapeName = shapeInput;
        Debug.Log(size + " " + zPosition + " " + colliderSize + " " + shapeName);
        //Debug.Log("Dropdown value " + shapeName);

        for (int i = 0; i < gameObjectIDs.Length; i++)
        {
            GameObject gameObject = PhotonNetwork.GetPhotonView(gameObjectIDs[i]).gameObject;
            //Debug.Log(gameObject.name);
            float additionSize = (size - (float)0.2)/2;
            gameObject.transform.localScale = new Vector3(size, size, size);

            if (gameObject.name == "SphereGhost" || gameObject.name == "CubeGhost")
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + additionSize, gameObject.transform.position.z);
            } else
            {
                gameObject.transform.position = new Vector3(positionX, gameObject.transform.position.y + additionSize, zPosition);
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
        //if(gameObject.name.StartsWith("Cube")) {
        //    var boxCollider = gameObject.GetComponent<BoxCollider>();
        //    boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
        //} else if (gameObject.name.StartsWith("Sphere"))
        //{
        //    var sphereCollider = gameObject.GetComponent<SphereCollider>();
        //    if (sphereCollider != null)
        //    {
        //        sphereCollider.radius = colliderSize;
        //    }
        //}

        var boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
    }

    private void SetColliderBounds(GameObject gameObject, bool bounds)
    {
        var boundsControl = gameObject.GetComponent<BoundsControl>();
        boundsControl.enabled = bounds;
    }
}
