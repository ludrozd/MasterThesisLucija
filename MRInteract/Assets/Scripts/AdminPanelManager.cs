using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminPanelManager : MonoBehaviour
{
    public Canvas pcCanvas;
    void Start()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        pcCanvas.gameObject.SetActive(true);
#elif UNITY_UWP
        pcCanvas.gameObject.SetActive(false);
#endif
    }
}
