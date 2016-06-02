using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Models
{
    public class Location
    {
        public Location(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
        public double Lat { get;  }
        public double Lon { get;  }
    }

    public class Place
    {
        public class PlaceBuilder
        {
            string _placeId;
            Location _location;
            string _name;
            decimal _rating;
            string _address;
            string _photoUrl;


            public PlaceBuilder(string placeId)
            {
                _placeId = placeId;
            }

            public PlaceBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public PlaceBuilder WithLocation(Location location)
            {
                _location = location;
                return this;
            }

            public PlaceBuilder WithRating(decimal rating)
            {
                _rating = rating;
                return this;
            }

            public PlaceBuilder WithAddress(string address)
            {
                _address = address;
                return this;
            }

            public PlaceBuilder WithPhotoUrl(string photoUrl)
            {
                _photoUrl = photoUrl;
                return this;
            }

            public Place Build()
            {
                return new Place()
                {
                    PlaceId = _placeId,
                    Location = _location,
                    Name = _name,
                    Rating = _rating,
                    Address = _address,
                    PhotoUrl = _photoUrl
                };
            }
        }

        public string PlaceId { get; private set; }
        public Location Location { get; private set; }
        public string Name { get; private set; }
        public decimal Rating { get; private set; }
        public string Address { get; private set; }
        public string PhotoUrl { get; private set; }

    }

   
}