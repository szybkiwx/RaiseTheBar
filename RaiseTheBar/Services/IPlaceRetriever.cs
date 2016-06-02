using RiseTheBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Services
{
    public interface IPlaceRetriever
    {
        IEnumerable<Place> GetPlaces(double lat, double lon);
    }
}