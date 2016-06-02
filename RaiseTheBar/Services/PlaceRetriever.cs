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
        private IQueryStringBuilder _qsBuilder;

        public PlaceRetriever(IPlaceHttpClient client, IQueryStringBuilder qsBuilder, string apiKey)
        {
            _client = client;
            _qsBuilder = qsBuilder;
            _apiKey = apiKey;
        }

        public IEnumerable<Place> GetPlaces(double lat, double lon)
        {

            List<Place> returnValue = new List<Place>();
            Dictionary<string, object> requestParams = new Dictionary<string, object>
                {
                    { "key",  _apiKey },
                    { "location", string.Format(new CultureInfo("us-US"), "{0},{1}", lat, lon) },
                    { "types", "bar" },
                    { "radius", "2000" }
                };
            string nextPageToken = null;
            
            while(true)
            {
                string query = _qsBuilder.Build(requestParams);
                string stringResult = _client.GetStringData("nearbysearch/json?" + query);
                JObject deserialized = JObject.Parse(stringResult);
                var listPart = _processResults(deserialized);
                returnValue.AddRange(listPart);
                var nextPageTokenObject = deserialized["next_page_token"];
                if (nextPageTokenObject != null)
                {
                    nextPageToken = nextPageTokenObject.ToString();
                    requestParams = new Dictionary<string, object>
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

        public PlaceDetails GetPlaceDetails(string id)
        {
            var requestParams = new Dictionary<string, object>
            {
                { "key",  _apiKey },
                { "placeid", id }
            };

            string query = _qsBuilder.Build(requestParams);

            string stringResult = _client.GetStringData("details/json?" + query);
            JObject deserialized = JObject.Parse(stringResult);

            PlaceDetails.PlaceDetailBuilder builder = (PlaceDetails.PlaceDetailBuilder)_mapPlace(deserialized["result"]);

            builder.WithPhoneNumber(deserialized["formatted_phone_number"].ToString());
            var reviews = deserialized["reviews"].Select(x =>
            {
                return new Review(int.Parse(x["rating"].ToString()), x["text"].ToString(), x["author"].ToString());
            });
            builder.WithReviews(reviews);
            return builder.Build();

        }

        private IEnumerable<Place> _processResults(JObject deserialized)
        {
            return  deserialized["results"].Select(result =>
            {
                return _mapPlace(result).Build();
            });
        }

        private Place.PlaceBuilder _mapPlace(JToken result)
        {
            var locationToken = result["geometry"]["location"];
            var location = new Location(
                double.Parse(locationToken["lat"].ToString()),
                double.Parse(locationToken["lng"].ToString()));

            var placeBuilder = new Place.PlaceBuilder(result["place_id"].ToString())
                .WithAddress(result["vicinity"].ToString())
                .WithLocation(location)
                .WithName(result["name"].ToString());

            if (result["photos"] != null)
            {
                string photoUrl = _getPhotoUrl(result);
                placeBuilder.WithPhotoUrl(photoUrl);
            }

            var rating = result["rating"];
            if (rating != null)
            {
                placeBuilder.WithRating(decimal.Parse(rating.ToString(), new CultureInfo("us-US")));
            }

            return placeBuilder;
        }

        private string _getPhotoUrl(JToken result)
        {
            var photoQuery = _qsBuilder.Build(new Dictionary<string, object>()
                    {
                        { "photoreference", result["photos"][0]["photo_reference"].ToString()},
                        { "key", _apiKey },
                        { "width", 200 }
                    });
            return _client.GetStringData("photo?" + photoQuery);
        }

        
    }
}