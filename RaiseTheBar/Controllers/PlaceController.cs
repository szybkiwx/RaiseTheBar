using RiseTheBar.Models;
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
    [RoutePrefix("bars")]
    public class PlaceController : ApiController
    {
        private IPlaceRetriever _retriever;
        private IDefaulSearchSettings _settings;
        public PlaceController(IPlaceRetriever retriever, IDefaulSearchSettings settings)
        {
            _retriever = retriever;
            _settings = settings;

        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List(double? lat, double? lon)
        {
            if(!lat.HasValue || !lon.HasValue)
            {
                lat = _settings.DefaultLat;
                lon = _settings.DefaultLon;
            }
            var result = _retriever.GetPlaces(lat.Value, lon.Value).Select(
                x => new Resource<Place>()
                {
                    ID = x.PlaceId,
                    Type = "bars",
                    Value = x
                });

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetPlace(string id)
        {
            var details = _retriever.GetPlaceDetails(id);
            if (details == null)
            {
                return NotFound();
            }

            var result = new Resource<PlaceDetails>()
            {
                ID = details.PlaceId,
                Type = "bars",
                Value = details
            };

            return Ok(result);
        }

    }
}
