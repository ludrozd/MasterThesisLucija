using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;

public class ReturnToMenu : MonoBehaviour
{
    public int numberOfScene;
    private DateTime startTime;
    private float distance;
    private float scale;
    private Quaternion rotation;
    private int count;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (numberOfScene.Equals(1)) {
                Scene1ObjectiveMeasuresExport();
            }
            else if (numberOfScene.Equals(2)) {
                Scene2ObjectiveMeasuresExport();
            } 
            else if (numberOfScene.Equals(3)) {
                Scene3ObjectiveMeasuresExport();
            }
            PhotonNetwork.LoadLevel(2);
        }
    }

    public void StartTimeMeasure()
    {
        startTime = System.DateTime.UtcNow;
    }

    public void StartClickCount()
    {
        count = 0;
    }

    public void IncreaseClickCount()
    {
        count += 1;
    }

    private void Scene1ObjectiveMeasuresExport()
    {
        System.TimeSpan ts = System.DateTime.UtcNow - startTime;
        string timePassed = ts.Seconds.ToString();

        GameObject cube = GameObject.Find("Cube");
        GameObject sphere = GameObject.Find("Sphere");
        if (cube != null)
        {
            GameObject cubeGhost = GameObject.Find("CubeGhost"); 
            distance = Vector3.Distance(cube.transform.position, cubeGhost.transform.position);
        } else if (sphere != null)
        {
            GameObject sphereGhost = GameObject.Find("SphereGhost");
            distance = Vector3.Distance(cube.transform.position, sphereGhost.transform.position);
        }

        string path = "C:\\Users\\Korisnica\\Desktop\\MRInteract_Resources\\ObjectiveParameters\\" + PhotonNetwork.CurrentRoom.Name + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Objective parameters for the Pick And Place scenario:");
        writer.WriteLine();
        writer.WriteLine("Time passed (in seconds): " + timePassed);
        writer.WriteLine("Object position difference: " + distance);
        writer.Close();
    }

    private void Scene2ObjectiveMeasuresExport()
    {
        System.TimeSpan ts = System.DateTime.UtcNow - startTime;
        string timePassed = ts.Seconds.ToString();
        TextMeshProUGUI colorText = GameObject.Find("ColorText (TMP)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fontText = GameObject.Find("FontText (TMP)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI sizeText = GameObject.Find("SizeText (TMP)").GetComponent<TextMeshProUGUI>();
        string color = colorText.color.ToString();
        string font = fontText.font.name;
        string size = sizeText.fontSize.ToString();

        string path = "C:\\Users\\Korisnica\\Desktop\\MRInteract_Resources\\ObjectiveParameters\\" + PhotonNetwork.CurrentRoom.Name + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Objective parameters for the UI Form scenario:");
        writer.WriteLine();
        writer.WriteLine("Time passed (in seconds): " + timePassed);
        writer.WriteLine("Nuber of button clicks: " + count);
        writer.WriteLine("Preferable color for text: " + color);
        writer.WriteLine("Preferable font type for text: " + font);
        writer.WriteLine("Preferable font size for text: " + size);
        writer.Close();
    }

    private void Scene3ObjectiveMeasuresExport()
    {
        System.TimeSpan ts = System.DateTime.UtcNow - startTime;
        string timePassed = ts.Seconds.ToString();

        GameObject cube = GameObject.Find("Cube");
        GameObject cylinder = GameObject.Find("Cylinder");
        if (cube != null)
        {
            GameObject cubeGhost = GameObject.Find("CubeGhost");
            distance = Vector3.Distance(cube.transform.position, cubeGhost.transform.position);
            scale = cube.transform.localScale.x - cubeGhost.transform.localScale.x;
            rotation = Quaternion.Inverse(cube.transform.localRotation) * cubeGhost.transform.localRotation;
        }
        else if (cylinder != null)
        {
            GameObject cylinderGhost = GameObject.Find("CylinderGhost");
            distance = Vector3.Distance(cylinder.transform.position, cylinderGhost.transform.position);
            scale = cylinder.transform.localScale.x - cylinder.transform.localScale.x;
            rotation = Quaternion.Inverse(cylinder.transform.localRotation) * cylinderGhost.transform.localRotation;
        }
        Vector3 differenceEulerAngles = rotation.eulerAngles;

        string path = "C:\\Users\\Korisnica\\Desktop\\MRInteract_Resources\\ObjectiveParameters\\" + PhotonNetwork.CurrentRoom.Name + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Objective parameters for the Pick And Place scenario:");
        writer.WriteLine();
        writer.WriteLine("Time passed (in seconds): " + timePassed);
        writer.WriteLine("Object position difference: " + distance);
        writer.WriteLine("Object scale difference: " + scale);
        writer.WriteLine("Object Euler Angle difference: " + differenceEulerAngles.ToString());
        writer.Close();
    }
}
