// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    /// <summary>
    /// An example script that delegates keyboard API access either to the WMR workaround
    /// (MixedRealityKeyboard) or Unity's TouchScreenKeyboard API depending on the platform.
    /// </summary>
    /// <remarks>
    /// <para>Note that like Unity's TouchScreenKeyboard API, this script only supports WSA, iOS, and Android.</para>
    /// <para>If using Unity 2019 or 2020, make sure the version >= 2019.4.25 or 2020.3.2 to ensure the latest fixes for Unity keyboard bugs are present.</para>
    /// </remarks>
    [AddComponentMenu("Scripts/MRTK/Examples/SystemKeyboardExample")]
    public class SystemKeyboardExample : MonoBehaviour
    {
#if WINDOWS_UWP
        private MixedRealityKeyboard wmrKeyboard;
#elif UNITY_IOS || UNITY_ANDROID
        private TouchScreenKeyboard touchscreenKeyboard;
#endif

        [SerializeField]
        private TextMeshProUGUI debugMessage = null;

#pragma warning disable 0414
        //[SerializeField]
        //private MixedRealityKeyboardPreview mixedRealityKeyboardPreview = null;
        //[SerializeField, Tooltip("Whether disable user's interaction with other UI elements while typing. Use this option to decrease the chance of keyboard getting accidentally closed.")]
        //private bool disableUIInteractionWhenTyping = false;
#pragma warning restore 0414

        /// <summary>
        /// Opens a platform specific keyboard.
        /// </summary>
        public void OpenSystemKeyboard()
        {
#if WINDOWS_UWP
            wmrKeyboard.ShowKeyboard(wmrKeyboard.Text, false);
#elif UNITY_IOS || UNITY_ANDROID
            touchscreenKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false, false, false);
#endif
        }


        private void Start()
        {
            // Initially hide the preview.
            //if (mixedRealityKeyboardPreview != null)
            //{
            //    mixedRealityKeyboardPreview.gameObject.SetActive(false);
            //}

#if WINDOWS_UWP
            // Windows mixed reality keyboard initialization goes here
            wmrKeyboard = gameObject.AddComponent<MixedRealityKeyboard>();
            //wmrKeyboard.DisableUIInteractionWhenTyping = disableUIInteractionWhenTyping;
#endif
        }

        private void Update()
        {
#if WINDOWS_UWP
            // Windows mixed reality keyboard update goes here
            if (wmrKeyboard.Visible)
            {
                if (debugMessage != null)
                {
                    debugMessage.text = wmrKeyboard.Text;
                }

            }
            else
            {
                var keyboardText = wmrKeyboard.Text;

                if (string.IsNullOrEmpty(keyboardText))
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text = "Open keyboard to type text.";
                    }
                }
                else
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text = keyboardText;
                    }
                }

            }

#endif
        }

    }
}
