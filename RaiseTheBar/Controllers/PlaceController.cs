using RaiseTheBar.Services;
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
        private ICashingService _cache;
        public PlaceController(IPlaceRetriever retriever, IDefaultSearchSettings settings, ICashingService cache)
        {
            _retriever = retriever;
            _settings = settings;
            _cache = cache;

        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult List()
        {
            double lat = _settings.DefaultLat;
            double lon = _settings.DefaultLon;

            string cacheKey = string.Format("places-{0}-{1}", lat, lon);

            IEnumerable<Place> places = _cache.Get<IEnumerable<Place>>(cacheKey);
            if (places == null)
            {
                places = _retriever.GetPlaces(lat, lon);
                _cache.Set(cacheKey, places);
                
            }
            IEnumerable<Resource<Place>> result = places.Select(
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
            string cacheKey = string.Format("place-{0}", id);
            PlaceDetails details;
            try
            {
                details = _cache.Get<PlaceDetails>(cacheKey);
                if (details == null)
                {
                    details = _retriever.GetPlaceDetails(id);
                }
            }
            catch (NoSuchPlaceException)
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
