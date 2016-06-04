using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RiseTheBar.Models;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading;

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
            int retryCount = 0;
            while (true)
            {
                string query = _qsBuilder.Build(requestParams);
                string stringResult = _client.GetStringData("nearbysearch/json?" + query);
                JObject deserialized = JObject.Parse(stringResult);

                //according to places API next_page_token starts working with slight delay
                //reisuing the same requrst is required
                if (deserialized["status"].ToString() != "OK")
                {
                    continue;
                }

                retryCount = 0;

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

            JToken result = deserialized["result"];

            if (result == null)
            {
                throw new NoSuchPlaceException();
            }

            PlaceDetails.PlaceDetailBuilder placeBuilder = _mapPlace(result);

            placeBuilder.WithPhoneNumber(result["international_phone_number"].ToString());
            if (deserialized["photos"] != null)
            {
                string photo = _getPhoto(deserialized);
                placeBuilder.WithPhoto(photo);
            }

            var reviews = result["reviews"].Select(x =>
            {
                return new Review(int.Parse(x["rating"].ToString()), x["text"].ToString(), x["author"].ToString());
            });

            placeBuilder.WithReviews(reviews);
            return placeBuilder.Build();

        }

        private IEnumerable<Place> _processResults(JObject deserialized)
        {
            return  deserialized["results"].Select(result =>
            {
                return _mapPlace(result).Build();
            });
        }

        private PlaceDetails.PlaceDetailBuilder _mapPlace(JToken result)
        {
            var locationToken = result["geometry"]["location"];
            var location = new Location(
                double.Parse(locationToken["lat"].ToString()),
                double.Parse(locationToken["lng"].ToString()));

            var placeBuilder = new PlaceDetails.PlaceDetailBuilder(result["place_id"].ToString())
                .WithAddress(result["vicinity"].ToString())
                .WithLocation(location)
                .WithName(result["name"].ToString());
            
            var rating = result["rating"];
            if (rating != null)
            {
                placeBuilder.WithRating(decimal.Parse(rating.ToString()));
            }

            return placeBuilder;
        }

        private string _getPhoto(JToken result)
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