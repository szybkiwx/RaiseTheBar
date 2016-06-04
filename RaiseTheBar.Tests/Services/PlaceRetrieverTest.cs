using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiseTheBar.Services;
using Moq;
using System.IO;
using RiseTheBar.Models;
using System.Collections.Generic;

namespace RaiseTheBar.Tests.Services
{
    [TestClass]
    public class PlaceRetrieverTest
    {

        private PlaceRetriever sut;

        private Mock<IPlaceHttpClient> clientMock;

        [TestInitialize]
        public void SetUp()
        {
            clientMock = new Mock<IPlaceHttpClient>();
            sut = new PlaceRetriever(clientMock.Object, new QueryStringBuilder(), "MY_KEY");
        }

        [TestMethod]
        public void PlaceRetrieverTest_TestGetPlaces()
        {

            string url1 = "nearbysearch/json?" +
                "key=MY_KEY&location=50.0612826,19.9371411&types=bar&radius=2000";
            string url2 = "nearbysearch/json?"+
                "key=MY_KEY&pagetoken=token1";
            string url3 = "nearbysearch/json?" +
              "key=MY_KEY&pagetoken=token2";

            clientMock.Setup(x => x.GetStringData(url1)).Returns(File.ReadAllText("Services\\page1.json"));
            clientMock.Setup(x => x.GetStringData(url2)).Returns(File.ReadAllText("Services\\page2.json"));
            clientMock.Setup(x => x.GetStringData(url3)).Returns(File.ReadAllText("Services\\page3.json"));

            IEnumerable<Place> result = sut.GetPlaces(50.0612826, 19.9371411);

            Assert.AreEqual(60, new List<Place>(result).Count);
        }

        [TestMethod]
        public void PlaceRetrieverTest_TestGetPlaceDetail()
        {

            string url= "details/json?key=MY_KEY&placeid=abc";
            

            clientMock.Setup(x => x.GetStringData(url)).Returns(File.ReadAllText("Services\\details.json"));
        
            PlaceDetails result = sut.GetPlaceDetails("abc");

            Assert.AreEqual("Andel's Vienna House", result.Name);
            Assert.AreEqual("+48 12 660 01 00", result.PhoneNumber);
        }
    }
}
