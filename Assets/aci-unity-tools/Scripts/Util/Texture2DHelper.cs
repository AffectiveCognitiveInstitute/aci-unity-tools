// <copyright file=Texture2DHelper.cs/>
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
// <date>07/12/2018 05:59</date>

using System;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Helper class containing methods for handling Texture2D.
    /// </summary>
    public class Texture2DHelper
    {
        /// <summary>
        /// Converts a <see cref="Texture2D"/> to a base64 encoded string in png format.
        /// </summary>
        /// <param name="tex">Target <see cref="Texture2D"/> instance.</param>
        /// <returns>Serialized texture data as <see cref="string"/>, empty if conversion fails.</returns>
        public static string Serialize(Texture2D tex)
        {
            if (tex == null)
                return "";
            // create byte array
            byte[] data = tex.EncodeToPNG();
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Converts a base64 encoded string in png format to <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="text">Texture data as <see cref="string"/>.</param>
        /// <returns><see cref="Texture2D"/> instance, null if failed.</returns>
        public static Texture2D DeSerialize(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            // convert to byte array
            byte[] data = Convert.FromBase64String(text);
            // read dimensions (see https://www.w3.org/TR/PNG/)
            // datastream structure: image signature (8 bytes) + 1st chunk( 4 bytes length + 4 bytes type + ImgHeader data(4 bytes width + 4 bytes Height)
            // width byte position = 8 + 4 + 4 + 4 - 1(account for 0 index) = 19
            int width = GetIntAt(data, 19);
            // height byte position = 8 + 4 + 4 + 4 + 4 - 1 = 23
            int height = GetIntAt(data, 23);
            // convert byte data
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            tex.LoadRawTextureData(data);
            return tex;
        }

        //Reads int data from position in byte array
        private static int GetIntAt(byte[] data, uint offset)
        {
            int ret = 0;
            for (int i = 0; i < 4 && i + offset < data.Length; i++)
            {
                ret <<= 8;
                ret |= data[i] & 0xFF;
            }

            return ret;
        }
    }
}