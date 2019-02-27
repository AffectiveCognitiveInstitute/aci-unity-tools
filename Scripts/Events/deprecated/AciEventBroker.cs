﻿// <copyright file=AciEventBroker.cs/>
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
// <date>07/16/2018 15:24</date>

using System;
using UnityEngine;

namespace Aci.Unity.Events.deprecated
{
    /// <summary>
    /// Manager holding possible event types.
    /// </summary>
    [Obsolete("AciEventBroker is deprecated, please use IAciEventManager instead.")]
    public class AciEventBroker : MonoBehaviour
    {
        public LocalizationChangedEvent localizationEvent = new LocalizationChangedEvent();
        
        public UserLoginEvent           loginEvent        = new UserLoginEvent();
        public UserLogoutEvent          logoutEvent       = new UserLogoutEvent();
        public UserRegisterEvent        registerEvent     = new UserRegisterEvent();
        public UserUpdateEvent          updateEvent       = new UserUpdateEvent();

        public DbRemoveEvent    dbRemoveEvent    = new DbRemoveEvent();
        public DbUpdateEvent    dbUpdateEvent    = new DbUpdateEvent();
    }
}