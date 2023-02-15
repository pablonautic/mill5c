using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mill5C.Core.Path;

namespace Mill5C.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PathTest
    {
        public PathTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PathPointTest()
        {
            float x = 1.5f, y = 3.1f, z = 5.66f, i = 1.000f, j = 0.000f, k = 0.000f;

            string line =
                "X" + SingleExtension.ToString(x) + 
                "Y" + SingleExtension.ToString(y) + 
                "Z" + SingleExtension.ToString(z) + 
                "I" + SingleExtension.ToString(i) + 
                "J" + SingleExtension.ToString(j) + 
                "K" + SingleExtension.ToString(k);

            PathPoint pp1 = PathPoint.FromLine(line, PathPoint.Zero);

            Assert.AreEqual(pp1.Position.X, x);
            Assert.AreEqual(pp1.Position.Y, y);
            Assert.AreEqual(pp1.Position.Z, z);

            Assert.AreEqual(pp1.Orientation.X, i);
            Assert.AreEqual(pp1.Orientation.Y, j);
            Assert.AreEqual(pp1.Orientation.Z, k);
        }


    }
}
