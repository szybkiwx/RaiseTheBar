using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTheBar.Services
{
    public interface IQueryStringBuilder
    {
        string Build(Dictionary<string, object> parameters);
    }
}
