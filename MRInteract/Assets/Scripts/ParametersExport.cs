using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ParametersExport : MonoBehaviour
{
    [Serializable]
    public struct InputParameter
    {
        public TextMeshProUGUI tmpText;
        public Text inputText;
    }
    public InputParameter[] inputParameters;

    [Serializable]
    public struct ToggleParameter
    {
        public TextMeshProUGUI tmpText;
        public Toggle toggle;
    }
    public ToggleParameter[] toggleParameters;

    [Serializable]
    public struct DropdownParameter
    {
        public TextMeshProUGUI tmpText;
        public Dropdown dropdown;
    }
    public DropdownParameter dropdownParameter;


    public void ExportParameters()
    {
        Debug.Log("Exporting...");
        Debug.Log("Room name: " + PhotonNetwork.CurrentRoom.Name);
        string path = "C:\\Users\\Korisnica\\Desktop\\MRInteract_Resources\\InputParameters\\" + PhotonNetwork.CurrentRoom.Name + ".txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Input parameters for the scenario:");
        writer.WriteLine();
        
        bool minWritten = false;
        foreach(InputParameter ip in inputParameters)
        {   
            if(ip.tmpText.text == "Slider Values:" && minWritten == false) {
                writer.WriteLine("Slider min value: " + ip.inputText.text);
                minWritten = true;
            } else if (ip.tmpText.text == "Slider Values:" && minWritten == true) {
                writer.WriteLine("Slider max value: " + ip.inputText.text);
            } else {
                writer.WriteLine(ip.tmpText.text + " " + ip.inputText.text);
            }
        }
        foreach(ToggleParameter tp in toggleParameters)
        {
            writer.WriteLine(tp.tmpText.text + " " + tp.toggle.isOn.ToString());
        }
        writer.WriteLine(dropdownParameter.tmpText.text + " " + dropdownParameter.dropdown.options[dropdownParameter.dropdown.value].text);
        writer.WriteLine();
        writer.Close();
    }
}
