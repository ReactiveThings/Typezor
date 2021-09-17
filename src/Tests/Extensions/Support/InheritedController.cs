using System.Collections.Generic;
using Typezor.Tests.Support;

namespace Typezor.Tests.Extensions.Support
{
    public class InheritedController : BaseController
    {
        [HttpGet("inherited/{id}")]
        public IEnumerable<string> RoutePrefixFromBaseHttpGetWithParameter(int id)
        {
            return null; // just for testing
        }
    }
}