﻿using System;

namespace Typezor.Tests.Support
{
    internal class RoutePrefixAttribute : Attribute
    {
        private string v;

        public RoutePrefixAttribute(string v)
        {
            this.v = v;
        }
    }
}