using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToggleTexts : MonoBehaviour
{
    public Toggle toggle1, toggle2, toggle3;
    public TextMeshProUGUI text1, text2, text3;

    void Update()
    {
        if (toggle1.isOn)
        {
            text1.text = "Dva";
        } else
        {
            text1.text = "Jedan";
        }

        if (toggle2.isOn)
        {
            text2.text = "Tri";
        }
        else
        {
            text2.text = "Dva";
        }

        if (toggle3.isOn)
        {
            text3.text = "Cetiri";
        }
        else
        {
            text3.text = "Tri";
        }
    }
}
