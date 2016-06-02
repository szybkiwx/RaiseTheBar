using RiseTheBar.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RiseTheBar.Controllers
{
    [RoutePrefix("bar")]
    public class PlaceController : ApiController
    {
        private IPlaceRetriever _retriever;
        private static double defaultLat = double.Parse(ConfigurationManager.AppSettings["defaultLat"]);
        private static double defaultLon = double.Parse(ConfigurationManager.AppSettings["defaultLon"]);

        public PlaceController(IPlaceRetriever retriever)
        {
            _retriever = retriever;

        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List(double? lat, double? lon)
        {
            if(!lat.HasValue || !lon.HasValue)
            {
                lat = defaultLat;
                lon = defaultLon;
            }



            return Ok();
        }

    }
}
