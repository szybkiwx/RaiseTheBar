using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RiseTheBar.Models;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace RiseTheBar.Services
{
    public class PlaceRetriever : IPlaceRetriever
    {
        private static string _googleApiBase = "https://maps.googleapis.com/maps/api/place";

        private string _apiKey;

        private IPlaceHttpClient _client;
        private IQueryStringBuilder _builder;

        public PlaceRetriever(IPlaceHttpClient client, IQueryStringBuilder builder, string apiKey)
        {
            _client = client;
            _builder = builder;
            _apiKey = apiKey;
        }

        public IEnumerable<Place> GetPlaces(double lat, double lon)
        {

            List<Place> returnValue = new List<Place>();
            Dictionary<string, object> req = new Dictionary<string, object>
                {
                    { "key",  _apiKey },
                    { "location", string.Format(new CultureInfo("us-US"), "{0},{1}", lat, lon) },
                    { "types", "bar" },
                    { "radius", "2000" }
                };
            string nextPageToken = null;
            
            while(true)
            {
                string query = _builder.Build(req);

                string stringResult = _client.GetStringData("nearbysearch/json?" + query);
                JObject deserialized = JObject.Parse(stringResult);
                
                var listPart = _processResults(deserialized);
                returnValue.AddRange(listPart);


                var nextPageTokenObject = deserialized["next_page_token"];
                if (nextPageTokenObject != null)
                {
                    nextPageToken = nextPageTokenObject.ToString();
                
                    req = new Dictionary<string, object>
                    {
                        { "key",  _apiKey },
                        { "pagetoken", nextPageToken }
                    };
                }
                else
                {
                    break;
                }
            }
            return returnValue;
        }

        private IEnumerable<Place> _processResults(JObject deserialized)
        {

            var retVal = deserialized["results"].Select(result =>
            {
                var locationToken = result["geometry"]["location"];
                var location = new Location(
                    double.Parse(locationToken["lat"].ToString()),
                    double.Parse(locationToken["lng"].ToString()));


                var builder = new Place.PlaceBuilder(result["place_id"].ToString())
                    .WithAddress(result["vicinity"].ToString())
                    .WithLocation(location)
                    .WithName(result["name"].ToString());

                if (result["photos"] != null)
                {
                    var photoQuery = _builder.Build(new Dictionary<string, object>()
                    {
                        { "photoreference", result["photos"][0]["photo_reference"].ToString()},
                        { "key", _apiKey },
                        { "width", 200 }
                    });
                    string photoUrl = _client.GetStringData("photo?" + photoQuery);
                    builder.WithPhotoUrl(photoUrl);
                }

                var rating = result["rating"];
                if(rating != null)
                {
                    builder.WithRating(decimal.Parse(rating.ToString(), new CultureInfo("us-US")));
                }
                return builder.Build();
            }).ToList();
            return retVal;
        }
    }
}