using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTheBar.Services
{
    public interface IPlaceHttpClient : IDisposable
    {
        string GetStringData(string url);
    }
}
