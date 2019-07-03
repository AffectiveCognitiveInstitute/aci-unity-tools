// <copyright file=JsonConfigProvider.cs/>
// <copyright>
//   Copyright (c) 2019, Affective & Cognitive Institute
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
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
// <date>07/02/2019 10:25</date>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ModestTree;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    ///     JSON implementation of a config provider that stores/reads config data from a json file.
    /// </summary>
    public class JsonConfigProvider : MonoBehaviour, IConfigProvider
    {
        private List<object> m_Clients = new List<object>();

        private JObject m_Data = new JObject();

        [SerializeField]
        private string m_Filename;

        /// <inheritdoc />
        public string Filename
        {
            get => m_Filename;
            set
            {
                if (value == m_Filename)
                    return;
                m_Filename = value;
                m_Data = null;
                LoadConfig();
                SaveConfig();
            }
        }

        /// <inheritdoc />
        public void RegisterClient(object client)
        {
            if (m_Clients.Contains(client))
                return;
            m_Clients.Add(client);
            if (m_Data == null)
                return;
            WriteToClient(client);
        }

        /// <inheritdoc />
        public void UnregisterClient(object client)
        {
            if (!m_Clients.Contains(client))
                return;
            m_Clients.Remove(client);
        }

        /// <inheritdoc />
        public void ClientDirty(object client)
        {
            ReadFromClient(client);
            SaveConfig();
        }

        /// <inheritdoc />
        public async void SaveConfig()
        {
            if (!File.Exists("./" + m_Filename))
            {
                FileStream fs = File.Create("./" + m_Filename);
                fs.Close();
            }
            File.WriteAllText("./" + m_Filename, m_Data.ToString(Formatting.Indented), Encoding.UTF8);
        }

        /// <inheritdoc />
        public async void LoadConfig()
        {
            if (!File.Exists("./" + m_Filename))
            {
                //create new JObject so we can save existing data
                m_Data = new JObject();
                return;
            }
            string jsonData = File.ReadAllText("./" + m_Filename, Encoding.UTF8);
            if (jsonData.IsEmpty())
            {
                m_Data = new JObject();
            }
            else
            {
                m_Data = JObject.Parse(jsonData);
            } 

            for (int i = 0; i < m_Clients.Count; ++i)
                WriteToClient(m_Clients[i]);
        }

        private void WriteToClient(object client)
        {
            PropertyInfo[] pInfos = client.GetType().GetProperties();
            FieldInfo[] fInfos = client.GetType().GetFields();

            foreach (FieldInfo info in fInfos)
            {
                if (!TryGetAttribute(info, out ConfigValueAttribute attr))
                    continue;
                JToken value = m_Data.GetValue(attr.identifier == null ? info.Name : attr.identifier);
                if (value == null)
                    continue;
                info.SetValue(client, value.ToObject(info.FieldType));
            }

            foreach (PropertyInfo info in pInfos)
            {
                if (!TryGetAttribute(info, out ConfigValueAttribute attr))
                    continue;
                JToken value = m_Data.GetValue(attr.identifier == null ? info.Name : attr.identifier);
                if (value == null)
                    continue;
                info.SetValue(client, value.ToObject(info.PropertyType));
            }
        }

        private void ReadFromClient(object client)
        {
            PropertyInfo[] pInfos = client.GetType().GetProperties();
            FieldInfo[] fInfos = client.GetType().GetFields();

            foreach (FieldInfo info in fInfos)
            {
                if (!TryGetAttribute(info, out ConfigValueAttribute attr))
                    continue;
                object value = info.GetValue(client);
                JToken token = value == null ? JValue.CreateNull() : JToken.FromObject(value);
                m_Data[attr.identifier == null ? info.Name : attr.identifier] = token;
            }

            foreach (PropertyInfo info in pInfos)
            {
                if (!TryGetAttribute(info, out ConfigValueAttribute attr))
                    continue;
                object value = info.GetValue(client);
                JToken token = value == null ? JValue.CreateNull() : JToken.FromObject(value);
                m_Data[attr.identifier == null ? info.Name : attr.identifier] = token;
            }
        }

        private bool TryGetAttribute(MemberInfo info, out ConfigValueAttribute attribute)
        {
            attribute = (ConfigValueAttribute) info.GetCustomAttributes()
                                                   .FirstOrDefault(x => x.GetType() == typeof(ConfigValueAttribute));
            return attribute != null;
        }

        private void OnValidate()
        {
            m_Data = new JObject();
            LoadConfig();
            SaveConfig();
        }
    }

}
