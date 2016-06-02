using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Services
{
    public class QueryStringBuilder : IQueryStringBuilder
    {
        public string Build(Dictionary<string, object> parameters)
        {
            return string.Join("&", parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }
    }
}