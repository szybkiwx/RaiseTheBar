using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RiseTheBar.Models
{
    public class Location
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
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


}