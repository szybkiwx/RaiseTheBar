using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaiseTheBar.Services
{
    public interface ICashingService
    {
        void Set<T>(string key, T cachedObject);
        T Get<T>(string key);
    }
}
