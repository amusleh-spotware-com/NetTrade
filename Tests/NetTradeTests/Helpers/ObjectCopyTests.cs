using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class ObjectCopyTests
    {
        private class TestClass
        {
            public TestClass(double thirdProperty)
            {
                ThirdProperty = thirdProperty;
            }

            public string FirstProperty { get; set; }

            public int SecondProperty { get; set; }

            public double ThirdProperty { get; }
        }

        [TestMethod()]
        public void CopyPropertiesTest()
        {
            var firstObject = new TestClass(12.1) { FirstProperty = "Test", SecondProperty = 2 };
            var secondObject = new TestClass(24.2);

            ObjectCopy.CopyProperties(firstObject, secondObject);

            Assert.AreEqual(firstObject.FirstProperty, secondObject.FirstProperty);
            Assert.AreEqual(firstObject.SecondProperty, secondObject.SecondProperty);
            Assert.AreNotEqual(firstObject.ThirdProperty, secondObject.ThirdProperty);
        }
    }
}