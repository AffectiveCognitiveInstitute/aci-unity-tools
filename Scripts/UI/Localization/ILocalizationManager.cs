// <copyright file=ILocalizationManager.cs/>
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
// <date>08/01/2018 06:16</date>

using System.Collections.Generic;

namespace Aci.Unity.UI.Localization
{
    /// <summary>
    ///     Interface for localization managers. Used by Zenject for injection.
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        ///     The current selected language represented by an IETF identifier string.
        /// </summary>
        string currentLocalization { get; set; }

        /// <summary>
        ///     The current selected language represented by an ISO 639-2 decorator string.
        /// </summary>
        string currentLocalizationDecorator { get; }

        /// <summary>
        ///     The selected string replacement pattern.
        /// </summary>
        string replacementPattern { get; set; }

        /// <summary>
        ///     Checks whether a string contains localization identifiers.
        /// </summary>
        /// <param name="str">Target string.</param>
        /// <returns>True if string contains localization identifiers, False otherwise.</returns>
        bool IsLocalized(string str);

        /// <summary>
        ///     Gets the localized string of a corresponding string identifier.
        /// </summary>
        /// <param name="id">Target string.</param>
        /// <returns>The localized string, null if not found.</returns>
        string GetLocalized(string id);

        /// <summary>
        ///     Gets a list of currently suppoerted languages as IETF identifier strings.
        /// </summary>
        /// <returns>List of IETF identfifiers.</returns>
        List<string> GetCapabilities();

        /// <summary>
        ///     Adds localization data from a <see cref="LocalizationData"/> instance to the managers registry.
        /// </summary>
        /// <param name="data">Target data to add.</param>
        /// <returns>True if succeeds, False otherwise.</returns>
        bool AddLocalizationData(LocalizationData data);

        /// <summary>
        ///     Removes data using another <see cref="LocalizationData"/> instance as a reference.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool RemoveLocalizationData(LocalizationData data);

        /// <summary>
        ///     Loads a localization file from target url and adds its data to the manager's registry.
        /// </summary>
        /// <param name="url">Target file url.</param>
        void LoadLocalizationFile(string url);

        /// <summary>
        ///     Clears all localization data.
        /// </summary>
       void ClearData();
    }
}