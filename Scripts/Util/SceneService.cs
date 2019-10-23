// <copyright file=SceneService.cs/>
// <copyright>
//   Copyright (c) 2019, Affective & Cognitive Institute
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
// <date>10/23/2019 10:05</date>

using Aci.Unity.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aci.Unity.Util
{
    public class SceneService : MonoBehaviour, ISceneService
    {
        [SerializeField]
        private string[] scenesToLoad;

        void Awake()
        {
            foreach(string scene in scenesToLoad)
                SwitchScene(null, scene);

            if (!Debug.isDebugBuild)
                Debug.unityLogger.logEnabled = false;
        }

        public async void SwitchScene(string sceneToUnload, string sceneToLoad)
        {
            if (sceneToUnload != null && IsSceneLoaded(sceneToUnload))
            {
                AciLog.Log("SceneService", $"Unloading scene {sceneToUnload}...");
                await SceneManager.UnloadSceneAsync(sceneToUnload, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }

            if (sceneToLoad != null && !IsSceneLoaded(sceneToLoad))
            {
                AciLog.Log("SceneService", $"Loading scene {sceneToLoad}...");
                await SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            }
        }

        private bool IsSceneLoaded(string sceneName)
        {
            bool sceneLoaded = false;
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    sceneLoaded = true;
                    break;
                }
            }
            return sceneLoaded;
        }
    }
}
