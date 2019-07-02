// <copyright file=IConfigurable.cs/>
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
// <date>07/02/2019 11:27</date>

namespace Aci.Unity.Util
{
    /// <summary>
    ///     Provides configuration data for registered clients. Writes directly to fields/properties w/ <see cref="ConfigValueAttribute"/>.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        ///     Name of the configuration file.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        ///     Registers a client w/ the provider. This triggers a write process of existing data to the client
        ///     or creates data in the config from preexisting data on the client if no data exists.
        ///     Creation of new data will also trigger saving the configuration file.
        /// </summary>
        /// <param name="client">Target client instance.</param>
        void RegisterClient(object client);

        /// <summary>
        ///     Removes a client.
        /// </summary>
        /// <param name="client">Target client instance.</param>
        void UnregisterClient(object client);

        /// <summary>
        ///     Signals that target client has data changes.
        /// </summary>
        /// <param name="client">Target client instance.</param>
        void ClientDirty(object client);

        /// <summary>
        ///     Saves the configuration to the specified file. Creates a config file if it does not exist.
        /// </summary>
        void SaveConfig();

        /// <summary>
        ///     Tries to load data from the specified configuration file. Will trigger writing new configuration data to clients.
        /// </summary>
        void LoadConfig();
    }
}
