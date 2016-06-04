using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiseTheBar.Controllers;
using Moq;
using RiseTheBar.Services;
using RiseTheBar.Models;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace RaiseTheBar.Tests.Controllers
{
    [TestClass]
    public class PlaceControllerTests
    {
        private PlaceController sut;
        private Mock<IPlaceRetriever> _placeRetrieverMock;
        private Mock<IDefaultSearchSettings> _settingsMock;
        public PlaceControllerTests()
        {
            _placeRetrieverMock = new Mock<IPlaceRetriever>();
            _settingsMock = new Mock<IDefaultSearchSettings>();
            sut = new PlaceController(_placeRetrieverMock.Object, _settingsMock.Object);
        }

        [TestMethod]
        public void PlaceController_ListTest_OK()
        {
            _placeRetrieverMock.Setup(x => x.GetPlaces(It.IsAny<double>(), It.IsAny<double>())).Returns(
                new List<Place>()
                {
                    new Place.PlaceBuilder("abc1").Build(),
                    new Place.PlaceBuilder("abc2").Build(),
                }
            );
            var response = sut.List();
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IEnumerable<Resource<Place>>>));
            IEnumerable<Resource<Place>> result = ((OkNegotiatedContentResult<IEnumerable<Resource<Place>>>)response).Content;
            Assert.AreEqual(2, new List<Resource<Place>>(result).Count);
        }

        [TestMethod]
        public void PlaceController_Single_OK()
        {
            _placeRetrieverMock.Setup(x => x.GetPlaceDetails("abc1")).Returns(
                new PlaceDetails.PlaceDetailBuilder("abc1").WithPhoneNumber("123").Build()
            );
            var response = sut.GetPlace("abc1");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Resource<PlaceDetails>>));
            Resource<PlaceDetails> result = ((OkNegotiatedContentResult<Resource<PlaceDetails>>)response).Content;
            Assert.AreEqual("123", result.Value.PhoneNumber);
        }

        [TestMethod]
        public void PlaceController_Single_WhenBadId_NotFound()
        {
            _placeRetrieverMock.Setup(x => x.GetPlaceDetails("abc1")).Throws(new NoSuchPlaceException());
            var response = sut.GetPlace("abc1");
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
    }
}
