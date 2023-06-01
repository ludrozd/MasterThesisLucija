using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectScaler : MonoBehaviour
{
    public InputField maxScaleSizeInputField;
    private float maxScaleSize;

    private void Update()
    {
        if (maxScaleSizeInputField.text != "")
        {
            maxScaleSize = float.Parse(maxScaleSizeInputField.text);
            Debug.Log(maxScaleSize);
            if (maxScaleSize > 0.2)
            {
                Vector3 currentSize = gameObject.transform.localScale;
                Debug.Log(currentSize);
                if (currentSize.x > maxScaleSize || currentSize.y > maxScaleSize || currentSize.z > maxScaleSize)
                {
                    gameObject.transform.localScale = Vector3.one * maxScaleSize;
                }
            }
        }

    }
}
