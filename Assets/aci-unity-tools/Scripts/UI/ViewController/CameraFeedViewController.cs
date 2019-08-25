// <copyright file=CameraFeedViewController.cs/>
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
//   James Gay, Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>08/24/2018 13:28</date>

using Aci.Unity.Events;
using Aci.Unity.Sensor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Aci.Unity.UI.ViewControllers
{
    /// <summary>
    ///     Displays a camera feed provided by a <see cref="WebcamProvider"/> instance.
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class CameraFeedViewController : MonoBehaviour
                                          , IAciEventHandler<WebcamStatusChangedArgs>
    {
        private RawImage _rawImage;
        private IAciEventManager _broker;
        private bool running = false;

        [Tooltip("Autoplay webcam texture on startup?")]
        public bool playOnAwake;

        [Tooltip("Crops and centers the camera feed image to preserve the aspect ratio.")]
        public bool preserveAspect;

        public Sprite placeholder;

        [Inject]
        public WebcamProvider provider;

        [Inject]
        public IAciEventManager broker
        {
            get { return _broker; }
            set
            {
                UnregisterFromEvents();
                _broker = value;
                RegisterForEvents();
            }
        }

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            if(playOnAwake)
                StartCameraFeed();
        }

        /// <summary>
        ///     Updates the aspect ratio of the feed display.
        /// </summary>
        public void UpdateAspectRatio()
        {
            if (preserveAspect)
            {
                Vector2 localDim = _rawImage.rectTransform.rect.size;
                Vector2 texSize = new Vector2(_rawImage.texture.width, _rawImage.texture.height);
                Vector2 relative = localDim / texSize;
                float scale = relative.x > relative.y ? relative.x : relative.y;
                Vector2 targetSize = texSize * scale;
                relative = localDim / targetSize;
                _rawImage.uvRect = new Rect((1- relative.x)*0.5f, (1 - relative.y) * 0.5f, relative.x, relative.y);
            }
            else
            {
                _rawImage.uvRect = new Rect(0, 0, 1, 1);
            }
        }

        /// <summary>
        ///     Starts displaying the Camera feed. Needs a valid <see cref="WebcamProvider"/> instance.
        /// </summary>
        public void StartCameraFeed()
        {
            running = true;
            _rawImage.texture = provider?.textureBuffer;
            if (_rawImage.texture == null)
            {
                _rawImage.texture = placeholder.texture;
                return;
            }
            UpdateAspectRatio();
        }

        /// <summary>
        ///     Stops displaying the camera feed.
        /// </summary>
        public void StopCameraFeed()
        {
            running = false;
            _rawImage.texture = null;
        }

        /// <inheritdoc />
        public void RegisterForEvents()
        {
            broker?.AddHandler<WebcamStatusChangedArgs>(this);
        }

        /// <inheritdoc />
        public void UnregisterFromEvents()
        {
            broker?.RemoveHandler<WebcamStatusChangedArgs>(this);
        }

        /// <inheritdoc />
        public void OnEvent(WebcamStatusChangedArgs arg)
        {
            if (!running)
                return;
            _rawImage.texture = provider?.textureBuffer;
            if (_rawImage.texture == null)
            {
                _rawImage.texture = placeholder.texture;
                return;
            }
            UpdateAspectRatio();
        }
    }
}