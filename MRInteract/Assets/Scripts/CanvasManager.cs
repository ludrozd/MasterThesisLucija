using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public Canvas pcCanvas;
    public Canvas holoCanvas;
    void Start()
    {
#if UNITY_STANDALONE_WIN  || UNITY_EDITOR_WIN
        pcCanvas.gameObject.SetActive(true);
        holoCanvas.gameObject.SetActive(false);
#elif UNITY_UWP
        pcCanvas.gameObject.SetActive(false);
        holoCanvas.gameObject.SetActive(true);
#endif
    }

}
