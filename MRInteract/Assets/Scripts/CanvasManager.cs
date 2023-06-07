using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public Canvas pcCanvas;
    //public EventSystem eventSystem;
    public Canvas holoCanvas;
    //public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE_WIN  || UNITY_EDITOR_WIN
        pcCanvas.gameObject.SetActive(true);
        //eventSystem.gameObject.SetActive(true);
        holoCanvas.gameObject.SetActive(false);
        //camera.GetComponent<EventSystem>().enabled = false;
#elif UNITY_UWP
        pcCanvas.gameObject.SetActive(false);
        //eventSystem.gameObject.SetActive(false);
        holoCanvas.gameObject.SetActive(true);
        //camera.GetComponent<EventSystem>().enabled = true;
#endif
    }

}
