﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Typezor.Tests.Support;

namespace Typezor.Tests.Extensions.Support
{
    public class RouteController
    {
        [Route("api/{controller}/a/{name}/{id?}")]
        public string GetRoute(string name, string filter = null, int count = 0)
        {
            return null;
        }

        [Route("api/{*key:string}")]
        public string WildcardRoute(string key)
        {
            return null;
        }

        [Route("api/{id}", Name = "name")]
        public string NamedRoute(int id)
        {
            return null;
        }

        [HttpGet("api/{id}")]
        public string HttpGetRoute(int id)
        {
            return null;
        }

        public string NoRoute()
        {
            return null;
        }

        public string NoRouteWithId(int id)
        {
            return null;
        }
    }
}
