using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiseTheBar.Services;
using System.Collections.Generic;

namespace RaiseTheBar.Tests.Services
{
    [TestClass]
    public class QueryStringBuilderTest
    {
        private QueryStringBuilder sut = new QueryStringBuilder();

        [TestMethod]
        public void TestBuild()
        {
            string result = sut.Build(new Dictionary<string, object>()
            {
                { "a", "b" },
                { "c", 1 }
            });

            Assert.AreEqual("a=b&c=1", result);
        }
    }
}
