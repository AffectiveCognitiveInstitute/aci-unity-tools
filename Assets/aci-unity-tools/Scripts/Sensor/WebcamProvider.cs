// <copyright file=WebcamProvider.cs/>
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
// <date>11/19/2018 08:49</date>

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Aci.Unity.Events;
using Aci.Unity.Util;
using UnityEngine;
using Zenject;

namespace Aci.Unity.Sensor
{
    /// <summary>
    ///     Wraps a <see cref="WebcamTexture"/> instance to provide multiple consumers with the same camera texture.
    /// </summary>
    public class WebcamProvider : MonoBehaviour
    {
        private WebCamTexture m_CamTex;
        private IAciEventManager m_EventBroker;
        private IConfigProvider m_ConfigProvider;

        private CancellationTokenSource m_Cts = new CancellationTokenSource();

        /// <summary>
        ///     Current webcam state.
        /// </summary>
        public bool running => m_CamTex != null && m_CamTex.isPlaying;

        /// <summary>
        ///     Current webcam state.
        /// </summary>
        public Texture textureBuffer => m_CamTex as Texture;

        [SerializeField]
        private string m_WebcamDevice;

        /// <summary>
        ///     Selected webcam device identifier.
        /// </summary>
        [ConfigValue]
        public string webcamDevice
        {
            get { return m_WebcamDevice; }
            set
            {
                if (webcamDevice == value)
                    return;
                m_WebcamDevice = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return;
#endif
                StartOrRefreshCamera();
            }
        }

        [SerializeField]
        private int m_ResolutionWidth;

        /// <summary>
        ///     Requested frame width;
        /// </summary>
        [ConfigValue]
        public int resolutionWidth
        {
            get { return m_ResolutionWidth; }
            set
            {
                if (m_ResolutionWidth == value)
                    return;
                m_ResolutionWidth = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return;
#endif
                StartOrRefreshCamera();
            }
        }

        [SerializeField]
        private int m_ResolutionHeight;

        /// <summary>
        ///     Requested frame height.
        /// </summary>
        [ConfigValue]
        public int resolutionHeight
        {
            get { return m_ResolutionHeight; }
            set
            {
                if (m_ResolutionHeight == value)
                    return;
                m_ResolutionHeight = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return;
#endif
                StartOrRefreshCamera();
            }
        }

        [SerializeField]
        private int m_Fps;

        /// <summary>
        ///     Requested frames per second.
        /// </summary>
        [ConfigValue]
        public int fps
        {
            get { return m_Fps; }
            set
            {
                if (m_Fps == value)
                    return;
                m_Fps = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return;
#endif
                StartOrRefreshCamera();
            }
        }

        [Zenject.Inject]
        private void Construct([InjectOptional]IConfigProvider config, IAciEventManager broker)
        {
            m_EventBroker = broker;
            m_ConfigProvider = config;
            m_ConfigProvider?.RegisterClient(this);
        }

        private void OnDestroy()
        {
            m_ConfigProvider?.UnregisterClient(this);
        }

        void Start()
        {
            StartOrRefreshCamera();
        }

        /// <summary>
        ///     Initializes or restarts the webcam.
        /// </summary>
        private void StartOrRefreshCamera()
        {
            if(m_CamTex == null)
                m_CamTex = new WebCamTexture();
            if (m_CamTex.isPlaying)
            {
                m_Cts.Cancel();
                m_CamTex.Stop();
            }
            m_CamTex.deviceName = m_WebcamDevice;
            // if our preferred device is not avaiable fall back to default device webcam
            if (WebCamTexture.devices.Length == 0)
            {
                Logging.AciLog.LogError("WebcamProvider", "No webcam available. Please check if reserved by other application or connected at all.");
                return;
            }
            if(WebCamTexture.devices.All(x => x.name != m_WebcamDevice))
                m_CamTex.deviceName = WebCamTexture.devices[0].name;
            m_CamTex.requestedWidth = m_ResolutionWidth;
            m_CamTex.requestedHeight = m_ResolutionHeight;
            m_CamTex.requestedFPS = m_Fps;
            m_CamTex.Play();
            m_EventBroker?.Invoke(new WebcamStatusChangedArgs());
        }
    }
}