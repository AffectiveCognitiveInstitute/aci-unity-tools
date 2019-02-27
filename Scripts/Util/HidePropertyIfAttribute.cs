// <copyright file=HidePropertyIfAttribute.cs/>
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
// <date>02/12/2019 09:45</date>

using System;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Indicates usage of custom <see cref="HidePropertyIfDrawer"/>. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class HidePropertyIfAttribute : PropertyAttribute
    {
        public enum Visibility
        {
            Lock,
            Hide
        }

        public string conditionName { get; private set; }
        public object conditionValue { get; private set; }
        public Visibility visibilityBehaviour { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="conditionName">Target property name.</param>
        /// <param name="conditionValue">Target value to check against.</param>
        /// <param name="visibilityBehaviour">Indicates whether property should be hidden completely or just locked.</param>
        public HidePropertyIfAttribute(string conditionName, object conditionValue, Visibility visibilityBehaviour = Visibility.Hide)
        {
            this.conditionName = conditionName;
            this.conditionValue = conditionValue;
            this.visibilityBehaviour = visibilityBehaviour;
        }
    }
}
