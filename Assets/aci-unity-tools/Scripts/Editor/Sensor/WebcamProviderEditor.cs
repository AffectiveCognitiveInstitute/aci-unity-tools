// <copyright file=WebcamProviderEditor.cs/>
// <copyright>
//   Copyright (c) 2018, Affective & Cognitive Institute
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software andassociated documentation files
//   (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
//   merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
//   IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// </copyright>
// <license>MIT License</license>
// <main contributors>
//   Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>11/19/2018 10:34</date>

using Aci.Unity.Util;
using UnityEditor;
using UnityEngine;

namespace Aci.Unity.Sensor
{
    [CustomEditor(typeof(WebcamProvider))]
    public class WebcamProviderEditor : UnityEditor.Editor
    {
        private void OnCameraChanged(object deviceName)
        {
            WebcamProvider target = (WebcamProvider)this.target;
            target.webcamDevice = (deviceName as string);

            if (!Application.isPlaying)
            {
                // ugly, but this is the only way i can think of to get this without zenject in edit mode
                IConfigProvider provider = GameObject.FindObjectOfType<JsonConfigProvider>();
                provider?.RegisterClient(target);
                provider?.ClientDirty(target);
            }
        }

        private void OnFpsChanged(object fps)
        {
            WebcamProvider target = (WebcamProvider)this.target;
            target.fps = (int)fps;

            if (!Application.isPlaying)
            {
                // ugly, but this is the only way i can think of to get this without zenject in edit mode
                IConfigProvider provider = GameObject.FindObjectOfType<JsonConfigProvider>();
                provider?.RegisterClient(target);
                provider?.ClientDirty(target);
            }
        }

        private void OnResolutionChanged(object resolution)
        {
            WebcamProvider target = (WebcamProvider)this.target;
            string[] dimensions = (resolution as string).Split('x');
            target.resolutionWidth = int.Parse(dimensions[0]);
            target.resolutionHeight = int.Parse(dimensions[1]);

            if (!Application.isPlaying)
            {
                // ugly, but this is the only way i can think of to get this without zenject in edit mode
                IConfigProvider provider = GameObject.FindObjectOfType<JsonConfigProvider>();
                provider?.RegisterClient(target);
                provider?.ClientDirty(target);
            }
        }

        public override void OnInspectorGUI()
        {
            WebcamProvider target = (WebcamProvider)this.target;

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            WebCamDevice[] deviceList = WebCamTexture.devices;

            GenericMenu webcamMenu = new GenericMenu
            {
                allowDuplicateNames = false
            };

            foreach (WebCamDevice device in deviceList)
            {
                string deviceName = device.name;
                webcamMenu.AddItem(new GUIContent(deviceName), deviceName == target.webcamDevice, OnCameraChanged, deviceName);
            }
            
            GenericMenu fpsMenu = new GenericMenu
            {
                allowDuplicateNames = false
            };
            
            fpsMenu.AddItem(new GUIContent("60"), 60 == target.fps, OnFpsChanged, 60);
            fpsMenu.AddItem(new GUIContent("30"), 30 == target.fps, OnFpsChanged, 30);
            fpsMenu.AddItem(new GUIContent("20"), 20 == target.fps, OnFpsChanged, 20);
            fpsMenu.AddItem(new GUIContent("15"), 15 == target.fps, OnFpsChanged, 15);

            GenericMenu resolutionMenu = new GenericMenu
            {
                allowDuplicateNames = false
            };

            resolutionMenu.AddItem(new GUIContent("320x240"), 320 == target.resolutionWidth && 240 == target.resolutionHeight, OnResolutionChanged, "320x240");
            resolutionMenu.AddItem(new GUIContent("640x480"), 640 == target.resolutionWidth && 480 == target.resolutionHeight, OnResolutionChanged, "640x480");
            resolutionMenu.AddItem(new GUIContent("1280x720"), 1280 == target.resolutionWidth && 720 == target.resolutionHeight, OnResolutionChanged, "1280x720");

            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUILayout.PrefixLabel("Target Webcam");
            if (EditorGUILayout.DropdownButton(new GUIContent(target.webcamDevice), FocusType.Keyboard))
                webcamMenu.ShowAsContext();
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(target.webcamDevice));

            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUILayout.PrefixLabel("Resolution");
            if (EditorGUILayout.DropdownButton(new GUIContent(target.resolutionWidth + "x" + target.resolutionHeight), FocusType.Keyboard))
                resolutionMenu.ShowAsContext();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUILayout.PrefixLabel("FPS");
            if (EditorGUILayout.DropdownButton(new GUIContent(target.fps.ToString()), FocusType.Keyboard))
                fpsMenu.ShowAsContext();
            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }
    }
}