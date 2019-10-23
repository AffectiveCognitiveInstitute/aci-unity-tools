using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.Services
{
    public class InMemoryCachedImageProvider : ICachedResourceProvider<Sprite, string>
    {
        private static Vector2 HalfVector = new Vector2(0.5f, 0.5f);

        private Dictionary<string, Sprite> m_Resources = new Dictionary<string, Sprite>(10);

        public async Task<Sprite> Get(string param)
        {
            if (string.IsNullOrEmpty(param))
                throw new ArgumentNullException(nameof(param));

            Sprite sprite = null;
            if (m_Resources.TryGetValue(param, out sprite))
                return sprite;
            else
            {
                sprite = await GetImageFromUrl(param);
                if(sprite != null && !m_Resources.ContainsKey(param))                
                    m_Resources.Add(param, sprite);

                return sprite;
            }
        }

        private async Task<Sprite> GetImageFromUrl(string url)
        {
            Texture2D tex = (await new WWW(url)).texture;
            return Create(tex);            
        }

        private Sprite Create(Texture2D tex)
        {
            if (tex == null)
                return null;

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), HalfVector, 1f);
        }

        public void Clear()
        {
            m_Resources.Clear();
        }
    }
}
