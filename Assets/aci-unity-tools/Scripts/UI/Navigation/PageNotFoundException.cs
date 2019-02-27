using System;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Exception thrown when a page could be found.
    /// </summary>
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException(string page) : base(page) { }
    }
}
