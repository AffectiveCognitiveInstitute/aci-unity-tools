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

using System.Threading;
using Aci.Unity.Events;
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

        [Inject]
        private IAciEventManager m_EventBroker;

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
        public string webcamDevice
        {
            get { return m_WebcamDevice; }
            set
            {
                m_WebcamDevice = value;
#if UNITY_EDITOR
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
        public int resolutionWidth
        {
            get { return m_ResolutionWidth; }
            set
            {
                m_ResolutionWidth = value;
#if UNITY_EDITOR
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
        public int resolutionHeight
        {
            get { return m_ResolutionHeight; }
            set
            {
                m_ResolutionHeight = value;
#if UNITY_EDITOR
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
        public int fps
        {
            get { return m_Fps; }
            set
            {
                m_Fps = value;
#if UNITY_EDITOR
                return;
#endif
                StartOrRefreshCamera();
            }
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
            m_CamTex.requestedWidth = m_ResolutionWidth;
            m_CamTex.requestedHeight = m_ResolutionHeight;
            m_CamTex.requestedFPS = m_Fps;
            m_CamTex.Play();
            m_EventBroker.Invoke(new WebcamStatusChangedArgs());
        }
    }
}