using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Services
{
    public interface IDefaultSearchSettings
    {
        double DefaultLat { get;  }
        double DefaultLon { get;  }

    }
}