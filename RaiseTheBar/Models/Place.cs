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
        public class PlaceBuilderBase<T> where T: PlaceBuilderBase<T>
        {
            protected T _self;

            string _placeId;
            Location _location;
            string _name;
            decimal _rating;
            string _address;


            public PlaceBuilderBase(string placeId)
            {
                _self = (T)this;
                _placeId = placeId;
            }

            public T WithName(string name)
            {
                _name = name;
                return _self;
            }

            public T WithLocation(Location location)
            {
                _location = location;
                return _self;
            }

            public T WithRating(decimal rating)
            {
                _rating = rating;
                return _self;
            }

            public T WithAddress(string address)
            {
                _address = address;
                return _self;
            }
            

            public Place Build()
            {
                return new Place()
                {
                    PlaceId = _placeId,
                    Location = _location,
                    Name = _name,
                    Rating = _rating,
                    Address = _address
                    
                };
            }
        }

        public class PlaceBuilder : PlaceBuilderBase<PlaceBuilder>
        {
            public PlaceBuilder(string placeId) : base(placeId)
            {

            }
        }



        public string PlaceId { get; protected set; }
        public Location Location { get; protected set; }
        public string Name { get; protected set; }
        public decimal Rating { get; protected set; }
        public string Address { get; protected set; }

    }



/*
    public class Place
    {
        public class PlaceBuilder
        {
            string _placeId;
            Location _location;
            string _name;
            decimal _rating;
            string _address;


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

            

            public Place Build()
            {
                return new Place()
                {
                    PlaceId = _placeId,
                    Location = _location,
                    Name = _name,
                    Rating = _rating,
                    Address = _address
                    
                };
            }
        }

        public string PlaceId { get; protected set; }
        public Location Location { get; protected set; }
        public string Name { get; protected set; }
        public decimal Rating { get; protected set; }
        public string Address { get; protected set; }

    }
    */
   
}