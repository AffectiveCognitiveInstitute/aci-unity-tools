using Aci.Unity.Services;
using System;
using UnityEngine;
using UIImage = UnityEngine.UI.Image;

namespace Aci.Unity.UI
{
    [RequireComponent(typeof(UIImage))]
    public class CachedImageComponent : MonoBehaviour
    {
        private ICachedResourceProvider<Sprite, string> m_ImageService;
        public event Action<Sprite> spriteLoaded;
        private string m_Url;
        private Sprite m_Sprite;
        private UIImage m_Image;


        public string url
        {
            get { return m_Url; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                m_Url = value;
                LoadSprite();
            }
        }

        public Sprite sprite => m_Sprite;

        [Zenject.Inject]
        public void Construct(ICachedResourceProvider<Sprite,string> imageService)
        {
            m_ImageService = imageService;
        }

        private void Awake()
        {
            m_Image = GetComponent<UIImage>();
        }

        private async void LoadSprite()
        {
            try
            {
                m_Sprite = await m_ImageService.Get(m_Url);
                if (m_Sprite != null)
                    spriteLoaded?.Invoke(m_Sprite);

                m_Image.overrideSprite = m_Sprite;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
