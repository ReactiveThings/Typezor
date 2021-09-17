using System;

namespace Typezor.Tests.Support
{
    public class HttpGetAttribute : Attribute
    {
        public HttpGetAttribute()
        {
        }

        public HttpGetAttribute(string route)
        {
        }
    }
}