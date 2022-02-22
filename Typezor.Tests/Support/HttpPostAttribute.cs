using System;

namespace Typezor.Tests.Support
{
    public class HttpPostAttribute : Attribute
    {
        public HttpPostAttribute()
        {
        }

        public HttpPostAttribute(string route)
        {
        }
    }
}