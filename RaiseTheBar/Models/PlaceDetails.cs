using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Models
{
    public class Review
    {
        public int Rating { get; }
        public string Text { get; }
        public string Author { get; }
        public Review(int rating, string text, string author)
        {
            Rating = rating;
            Text = text;
            Author = author; 
        }

    }
    public class PlaceDetails : Place
    {
        public class PlaceDetailBuilder : PlaceBuilderBase<PlaceDetailBuilder>
        {
            private string _phoneNumber;
            private string _photo;
            private IEnumerable<Review> _reviews;

            public PlaceDetailBuilder(string placeId) : base(placeId)
            {
            }



            public PlaceDetailBuilder WithPhoneNumber(string number)
            {
                _phoneNumber = number;
                return this;
            }

            public PlaceDetailBuilder WithReviews(IEnumerable<Review> reviews)
            {
                _reviews = reviews;
                return this;
            }

            public PlaceDetailBuilder WithPhoto(string photo)
            {
                _photo = photo;
                return this;
            }

            public new PlaceDetails Build()
            {
                Place place = base.Build();
                PlaceDetails placeDetails = new PlaceDetails(place);
                placeDetails.PhoneNumber = _phoneNumber;
                placeDetails.Reviews = _reviews;
                placeDetails.Photo = _photo;
                return placeDetails;
            }
        }

        public PlaceDetails(Place place)
        {
            PlaceId = place.PlaceId;
            Rating = place.Rating;
            Location = place.Location;
            Address = place.Address;
            Name = place.Name;
            PhoneNumber = PhoneNumber;
            Photo = Photo;
            
        }

        public string Photo { get; private set; }
        public string PhoneNumber { get; private set; }
        public IEnumerable<Review> Reviews { get; private set; }
    }
}