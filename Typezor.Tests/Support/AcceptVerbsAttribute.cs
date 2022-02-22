using System;

namespace Typezor.Tests.Support
{
    public class AcceptVerbsAttribute : Attribute
    {
        public AcceptVerbsAttribute(params string[] verbs)
        {
        }
    }
}
