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
        private IDefaultSearchSettings _settings;
        public PlaceController(IPlaceRetriever retriever, IDefaultSearchSettings settings)
        {
            _retriever = retriever;
            _settings = settings;

        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List()
        {
            double lat = _settings.DefaultLat;
            double lon = _settings.DefaultLon;
            var result = _retriever.GetPlaces(lat, lon).Select(
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
            PlaceDetails details;
            try
            {
                details = _retriever.GetPlaceDetails(id);
            }
            catch(NoSuchPlaceException)
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
