using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.Unity.Events
{  
    public struct UserLoginArgs
    {
        public bool   success;
        public string msg;
    }

    public struct UserLogoutArgs
    {
    }

    public struct UserRegisterArgs
    {
        public bool   success;
        public string msg;
    }

    public struct UserUpdateArgs
    {
        public bool   success;
        public string msg;
    }

    public struct DbUpdateArgs
    {
        public bool success;
    }

    public struct DbRemoveArgs
    {
        public bool   success;
        public string msg;
    }

    public struct LocalizationChangedArgs
    {
        public string ietf;
        public string localeDecorator;
    }

    public struct WebcamStatusChangedArgs
    {
    }
}
