using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace RiseTheBar.Services
{
    public class PlaceHttpClient : IPlaceHttpClient
    {
        private HttpClient _client; 

        public PlaceHttpClient(HttpClient client)
        {
            _client = client;
           // _client.BaseAddress 
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public string GetStringData(string url)
        {
            return _client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }
    }
}