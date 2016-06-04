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
            var syncResponse = _client.GetAsync(url).Result;
            var content = syncResponse.Content.ReadAsStringAsync().Result;
            return content;
        }
    }
}